// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace VirtualClient.Actions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Abstractions;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Polly;
    using VirtualClient.Common;
    using VirtualClient.Common.Extensions;
    using VirtualClient.Common.Platform;
    using VirtualClient.Common.Telemetry;
    using VirtualClient.Contracts;
    using VirtualClient.Contracts.Metadata;

    /// <summary>
    /// CPS Executor
    /// </summary>
    [SupportedPlatforms("linux-arm64,linux-x64,win-arm64,win-x64")]
    public class CPSExecutor2 : VirtualClientComponent
    {
        private const string OutputFileName = "cps-results.txt";
        private IFileSystem fileSystem;

        /// <summary>
        /// Initializes a new instance of the <see cref="CPSExecutor2"/> class.
        /// </summary>
        /// <param name="dependencies">Provides required dependencies to the component.</param>
        /// <param name="parameters">Parameters defined in the profile or supplied on the command line.</param>
        public CPSExecutor2(IServiceCollection dependencies, IDictionary<string, IConvertible> parameters)
            : base(dependencies, parameters)
        {
            this.fileSystem = dependencies.GetService<IFileSystem>();

            this.ProcessStartRetryPolicy = Policy.Handle<Exception>(exc => exc.Message.Contains("sockwiz")).Or<VirtualClientException>()
                .WaitAndRetryAsync(5, retries => TimeSpan.FromSeconds(retries * 3));
        }

        /// <summary>
        /// The interval (in seconds) to display CPS
        /// </summary>
        public int DisplayInterval
        {
            get
            {
                return this.Parameters.GetValue<int>(nameof(CPSExecutor2.DisplayInterval), 10);
            }
        }

        /// <summary>
        /// Client used to communicate with the locally self-hosted instance of the
        /// Virtual Client API.
        /// </summary>
        protected IApiClient LocalApiClient { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        protected string Results { get; set; }

        /// <summary>
        /// Client used to communicate with the target self-hosted instance of the
        /// Virtual Client API (i.e. the server-side instance).
        /// </summary>
        protected IApiClient ServerApiClient { get; private set; }

        /// <summary>
        /// Name of the scenario.
        /// </summary>
        protected string Name { get; set; }

        /// <summary>
        /// Tool executable path.
        /// </summary>
        protected string ExecutablePath { get; set; }

        /// <summary>
        /// Path to the metrics/results.
        /// </summary>
        protected string ResultsPath { get; set; }

        /// <summary>
        /// Process name of the tool.
        /// </summary>
        protected string ProcessName { get; set; }

        /// <summary>
        /// Name of the tool.
        /// </summary>
        protected string Tool { get; set; }

        /// <summary>
        /// Provides features for management of the system/environment.
        /// </summary>
        protected ISystemManagement SystemManager
        {
            get
            {
                return this.Dependencies.GetService<ISystemManagement>();
            }
        }

        /// <summary>
        /// Cancellation Token Source for Server.
        /// </summary>
        protected CancellationTokenSource ServerCancellationSource { get; set; }

        /// <summary>
        /// The retry policy to apply to the startup of the CPS workload to handle
        /// transient issues.
        /// </summary>
        protected IAsyncPolicy ProcessStartRetryPolicy { get; set; }

        /// <summary>
        /// Intialize CPS.
        /// </summary>
        protected override async Task InitializeAsync(EventContext telemetryContext, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(this.Scenario))
            {
                throw new WorkloadException(
                    $"Scenario parameter missing. The profile supplied is missing the required '{nameof(this.Scenario)}' parameter " +
                    $"for one or more of the '{nameof(CPSExecutor2)}' steps.",
                    ErrorReason.InvalidProfileDefinition);
            }

            ClientInstance clientInstance = this.GetLayoutClientInstance(this.AgentId);
            string layoutIPAddress = clientInstance.IPAddress;

            this.Logger.LogTraceMessage($"Layout-Defined IP Address: {layoutIPAddress}");
            this.Logger.LogTraceMessage($"Layout-Defined Role: {clientInstance.Role}");

            this.ThrowIfLayoutNotDefined();
            this.ThrowIfLayoutClientIPAddressNotFound(layoutIPAddress);

            DependencyPath workloadPackage = await this.GetPlatformSpecificPackageAsync(this.PackageName, cancellationToken);
            telemetryContext.AddContext("package", workloadPackage);
            string role = clientInstance.Role;

            this.InitializeApiClients();

            // e.g.
            // CPS_T16 Client, CPS_T16 Server
            this.Name = $"{this.Scenario} {role}";
            this.ProcessName = "cps";
            this.Tool = "CPS";

            string resultsDir = this.Combine(workloadPackage.Path, this.Scenario);
            this.fileSystem.Directory.CreateDirectory(resultsDir);

            this.ResultsPath = this.Combine(resultsDir, CPSExecutor2.OutputFileName);
            this.Results = null;

            if (this.Platform == PlatformID.Win32NT)
            {
                this.ExecutablePath = this.Combine(workloadPackage.Path, "cps.exe");
            }
            else if (this.Platform == PlatformID.Unix)
            {
                this.ExecutablePath = this.Combine(workloadPackage.Path, "cps");
            }
            else
            {
                throw new NotSupportedException($"{this.Platform} is not supported");
            }

            this.SystemManager.MakeFileExecutableAsync(this.ExecutablePath, this.Platform, cancellationToken);
        }

        /// <summary>
        /// Executes CPS workload.
        /// </summary>
        /// <param name="telemetryContext">Provides context information that will be captured with telemetry events.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
        protected override async Task ExecuteAsync(EventContext telemetryContext, CancellationToken cancellationToken)
        {
            if (!this.IsInRole(ClientRole.Client) && !this.IsInRole(ClientRole.Server))
            {
                throw new NotSupportedException($"The role is not supported for {this.TypeName}." +
                    $" Environment layout should contain only {ClientRole.Client} or {ClientRole.Server} as roles");
            }

            using (this.ServerCancellationSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
            {
                try
                {
                    CancellationToken serverCancellationToken = this.ServerCancellationSource.Token;

                    if (this.IsInRole(ClientRole.Server))
                    {
                        using (var serverExecutor = this.CreateWorkloadServer())
                        {
                            await serverExecutor.ExecuteAsync(serverCancellationToken)
                                .ConfigureAwait(false);

                            this.Logger.LogMessage($"{nameof(CPSExecutor2)}.ServerExecutionCompleted", telemetryContext);
                        }
                    }
                    else if (this.IsInRole(ClientRole.Client))
                    {
                        using (var clientExecutor = this.CreateWorkloadClient())
                        {
                            await clientExecutor.ExecuteAsync(serverCancellationToken)
                                .ConfigureAwait(false);

                            this.Logger.LogMessage($"{nameof(CPSExecutor2)}.ClientExecutionCompleted", telemetryContext);
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    // Expected whenever certain operations (e.g. Task.Delay) are cancelled.
                    this.Logger.LogMessage($"{nameof(CPSExecutor2)}.Cancelled", telemetryContext);
                }
            }
        }

        /// <summary>
        /// Get new CPS client instance.
        /// </summary>
        protected virtual VirtualClientComponent CreateWorkloadClient()
        {
            return new CPSClientExecutor2(this.Dependencies, this.Parameters);
        }

        /// <summary>
        /// Get new Networking workload server instance.
        /// </summary>
        protected virtual VirtualClientComponent CreateWorkloadServer()
        {
            return new NetworkingWorkloadProxy(this.Dependencies, this.Parameters);
        }

        /// <summary>
        /// Delete Results File
        /// </summary>
        /// <returns></returns>
        protected async Task DeleteResultsFileAsync()
        {
            if (this.SystemManager.FileSystem.File.Exists(this.ResultsPath))
            {
                await this.SystemManager.FileSystem.File.DeleteAsync(this.ResultsPath)
                    .ConfigureAwait(false);
            }

        }

        /// <summary>
        /// Enable the firewall rule for the tool executable.
        /// </summary>
        protected async Task EnableInboundFirewallAccessAsync(string exePath, ISystemManagement systemManagement, CancellationToken cancellationToken)
        {
            if (exePath != null)
            {
                FirewallEntry firewallEntry = new FirewallEntry(
                    $"Virtual Client: Allow {exePath}",
                    "Allows client and server instances of the Virtual Client to communicate via the self-hosted API service.",
                    exePath);

                await systemManagement.FirewallManager.EnableInboundAppAsync(firewallEntry, cancellationToken)
                    .ConfigureAwait(false);
            }

        }

        /// <summary>
        /// Returns true if results are found in the results file within the polling/timeout
        /// period specified.
        /// </summary>
        protected async Task WaitForResultsAsync(TimeSpan timeout, EventContext telemetryContext, CancellationToken cancellationToken)
        {
            IFile fileAccess = this.SystemManager.FileSystem.File;
            string resultsContent = null;
            DateTime pollingTimeout = DateTime.UtcNow.Add(timeout);

            while (DateTime.UtcNow < pollingTimeout && !cancellationToken.IsCancellationRequested)
            {
                if (fileAccess.Exists(this.ResultsPath))
                {
                    try
                    {
                        resultsContent = await this.SystemManager.FileSystem.File.ReadAllTextAsync(this.ResultsPath)
                            .ConfigureAwait(false);

                        if (!string.IsNullOrWhiteSpace(resultsContent))
                        {
                            this.Logger.LogMessage($"{this.TypeName}.WorkloadOutputFileContents", telemetryContext
                                .AddContext("results", resultsContent));

                            break;
                        }
                    }
                    catch (IOException)
                    {
                        // This can be hit if the application is exiting/cancelling while attempting to read
                        // the results file.
                    }
                }

                await this.WaitAsync(TimeSpan.FromSeconds(2), cancellationToken).ConfigureAwait(false);
            }

            if (string.IsNullOrWhiteSpace(resultsContent))
            {
                throw new WorkloadResultsException(
                    $"Results not found. The workload '{this.ExecutablePath}' did not produce any valid results.",
                    ErrorReason.WorkloadFailed);
            }
        }

        /// <summary>
        /// Executes the workload.
        /// </summary>
        /// <param name="commandArguments">The command arguments to use to run the workload toolset.</param>
        /// <param name="timeout">The absolute timeout for the workload.</param>
        /// <param name="telemetryContext">Provides context information to include with telemetry events emitted.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the operation.</param>
        protected Task<IProcessProxy> ExecuteWorkloadAsync(string commandArguments, TimeSpan timeout, EventContext telemetryContext, CancellationToken cancellationToken)
        {
            IProcessProxy process = null;

            EventContext relatedContext = telemetryContext.Clone()
               .AddContext("command", this.ExecutablePath)
               .AddContext("commandArguments", commandArguments);

            return this.Logger.LogMessageAsync($"{this.TypeName}.ExecuteWorkload", relatedContext, async () =>
            {
                using (BackgroundOperations profiling = BackgroundOperations.BeginProfiling(this, cancellationToken))
                {
                    await this.ProcessStartRetryPolicy.ExecuteAsync(async () =>
                    {
                        using (process = this.SystemManager.ProcessManager.CreateProcess(this.ExecutablePath, commandArguments))
                        {
                            try
                            {
                                this.CleanupTasks.Add(() => process.SafeKill());
                                await process.StartAndWaitAsync(cancellationToken, timeout);

                                if (!cancellationToken.IsCancellationRequested)
                                {
                                    await this.LogProcessDetailsAsync(process, relatedContext, "CPS", logToFile: true);

                                    process.ThrowIfWorkloadFailed();
                                    this.Results = process.StandardOutput.ToString();
                                }
                            }
                            catch (TimeoutException exc)
                            {
                                // We give this a best effort but do not want it to prevent the next workload
                                // from executing.
                                this.Logger.LogMessage($"{this.TypeName}.WorkloadTimeout", LogLevel.Warning, relatedContext.AddError(exc));
                                process.SafeKill();

                                throw new WorkloadException($"CPS workload did not exit within the timeout period defined (timeout={timeout}).", exc, ErrorReason.WorkloadFailed);
                            }
                            catch (Exception exc)
                            {
                                this.Logger.LogMessage($"{this.TypeName}.WorkloadStartupError", LogLevel.Warning, relatedContext.AddError(exc));
                                throw new WorkloadException($"CPS workload failed to start successfully", exc, ErrorReason.WorkloadFailed);
                            }
                        }
                    }).ConfigureAwait(false);
                }

                return process;
            });
        }

        private void InitializeApiClients()
        {
            IApiClientManager clientManager = this.Dependencies.GetService<IApiClientManager>();
            this.LocalApiClient = clientManager.GetOrCreateApiClient(IPAddress.Loopback.ToString(), IPAddress.Loopback);

            if (this.IsInRole(ClientRole.Client))
            {
                ClientInstance serverInstance = this.GetLayoutClientInstances(ClientRole.Server).First();
                IPAddress.TryParse(serverInstance.IPAddress, out IPAddress serverIPAddress);

                // It is important that we reuse the API client. The HttpClient created underneath will need to use a
                // new connection from the connection pool typically for each instance created. Especially for the case with
                // this workload that is testing network resources, we need to be very cognizant of our usage of TCP connections.
                this.ServerApiClient = clientManager.GetOrCreateApiClient(serverInstance.IPAddress, serverIPAddress);
                this.RegisterToSendExitNotifications($"{this.TypeName}.ExitNotification", this.ServerApiClient);
            }
        }

        /// <summary>
        /// CPS State Class.
        /// </summary>
        internal class CPSWorkloadState : State
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="CPSWorkloadState"/> class.
            /// </summary>
            public CPSWorkloadState(ClientServerStatus status, IDictionary<string, IConvertible> properties = null)
                : base(properties)
            {
                this.Status = status;
            }

            /// <summary>
            /// An identifier for the status of State. (e.g. ClientServerReset).
            /// </summary>
            [JsonProperty(PropertyName = "status", Required = Required.Always)]
            [System.Text.Json.Serialization.JsonConverter(typeof(StringEnumConverter))]
            public ClientServerStatus Status { get; set; }

        }
    }
}
