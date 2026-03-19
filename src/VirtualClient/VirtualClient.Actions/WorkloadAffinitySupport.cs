// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace VirtualClient.Actions
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using VirtualClient.Common;
    using VirtualClient.Common.ProcessAffinity;
    using VirtualClient.Common.Telemetry;
    using VirtualClient.Contracts;

    /// <summary>
    /// Shared helper for creating and running processes with optional CPU core affinity binding.
    /// Follows the pattern established by RedisServerExecutor.
    /// </summary>
    internal static class WorkloadAffinitySupport
    {
        /// <summary>
        /// Creates a process with optional CPU affinity binding and elevated privileges.
        /// On Linux, wraps the command with numactl via CreateElevatedProcessWithAffinity.
        /// On Windows, creates a normal elevated process (affinity applied post-start).
        /// </summary>
        /// <param name="processManager">The process manager used to create the process.</param>
        /// <param name="platform">The OS platform.</param>
        /// <param name="command">The command to run.</param>
        /// <param name="arguments">The command line arguments.</param>
        /// <param name="workingDirectory">The working directory.</param>
        /// <param name="coreAffinity">The core specification (e.g., "0-3", "0,2,4,6"). Null for no affinity.</param>
        /// <returns>A tuple of (process, affinityConfig). affinityConfig is null when no binding requested.</returns>
        public static (IProcessProxy Process, ProcessAffinityConfiguration AffinityConfig) CreateProcessWithOptionalAffinity(
            ProcessManager processManager,
            PlatformID platform,
            string command,
            string arguments,
            string workingDirectory,
            string coreAffinity)
        {
            if (string.IsNullOrWhiteSpace(coreAffinity))
            {
                // No affinity — standard elevated process
                IProcessProxy process = processManager.CreateElevatedProcess(platform, command, arguments, workingDirectory);
                return (process, null);
            }

            ProcessAffinityConfiguration affinityConfig = ProcessAffinityConfiguration.Create(platform, coreAffinity);

            if (platform == PlatformID.Unix)
            {
                // Linux: numactl wrapping handled by CreateElevatedProcessWithAffinity
                IProcessProxy process = processManager.CreateElevatedProcessWithAffinity(
                    platform, command, arguments, workingDirectory, affinityConfig);
                return (process, affinityConfig);
            }
            else
            {
                // Windows: create elevated, affinity applied post-start via ApplyAffinity
                IProcessProxy process = processManager.CreateElevatedProcess(platform, command, arguments, workingDirectory);
                return (process, affinityConfig);
            }
        }

        /// <summary>
        /// Starts a process and applies Windows CPU affinity if applicable.
        /// On Linux, the process already has numactl wrapping so just starts normally.
        /// On Windows, starts the process then applies the affinity bitmask.
        /// </summary>
        /// <param name="process">The process to start.</param>
        /// <param name="platform">The OS platform.</param>
        /// <param name="affinityConfig">The affinity config, or null if no binding.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        public static async Task StartAndWaitWithAffinityAsync(
            IProcessProxy process,
            PlatformID platform,
            ProcessAffinityConfiguration affinityConfig,
            CancellationToken cancellationToken)
        {
            if (affinityConfig != null && platform == PlatformID.Win32NT)
            {
                // Windows: start first, then apply affinity bitmask
                process.Start();
                process.ApplyAffinity((WindowsProcessAffinityConfiguration)affinityConfig);
                await process.WaitForExitAsync(cancellationToken).ConfigureAwait(false);
            }
            else
            {
                // Linux (with or without numactl) or no affinity
                await process.StartAndWaitAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Adds affinity-related context to telemetry for diagnostics.
        /// </summary>
        public static void AddAffinityContext(EventContext telemetryContext, bool bindToCores, string coreAffinity, ProcessAffinityConfiguration affinityConfig)
        {
            telemetryContext.AddContext("bindToCores", bindToCores);
            if (bindToCores)
            {
                telemetryContext.AddContext("coreAffinity", coreAffinity);
                if (affinityConfig != null)
                {
                    telemetryContext.AddContext("affinityDetails", affinityConfig.ToString());
                }
            }
        }
    }
}
