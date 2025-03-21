// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace VirtualClient.Actions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Http;
    using System.Net.Sockets;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using Newtonsoft.Json.Linq;
    using NUnit.Framework;
    using Polly;
    using VirtualClient.Common.Contracts;
    using VirtualClient.Common.Telemetry;
    using VirtualClient.Contracts;
    using static VirtualClient.Actions.NTttcpExecutor2;

    [TestFixture]
    [Category("Unit")]
    public class NTttcpClientExecutorTests2
    {
        private static readonly string ExamplesDirectory = MockFixture.GetDirectory(typeof(ScriptExecutorTests), "Examples", "NTttcp");

        private MockFixture mockFixture;
        private DependencyPath mockPackage;

        public void SetupTest(PlatformID platformID, Architecture architecture)
        {
            this.mockFixture = new MockFixture();
            this.mockFixture.Setup(platformID, architecture);
            this.mockPackage = new DependencyPath("networking", this.mockFixture.GetPackagePath("networking"));
            this.mockFixture.SetupPackage(this.mockPackage);
            this.mockFixture.File.Setup(f => f.Exists(It.IsAny<string>()))
                .Returns(true);

            this.mockFixture.Parameters["PackageName"] = "networking";
            this.mockFixture.Parameters["TestDuration"] = 300;
            this.mockFixture.Parameters["WarmupTime"] = 300;
            this.mockFixture.Parameters["Protocol"] = ProtocolType.Tcp.ToString();
            this.mockFixture.Parameters["ThreadCount"] = 1;
            this.mockFixture.Parameters["ClientBufferSize"] = "4k";
            this.mockFixture.Parameters["Port"] = 5500;
            this.mockFixture.Parameters["ReceiverMultiClientMode"] = true;
            this.mockFixture.Parameters["SenderLastClient"] = true;
            this.mockFixture.Parameters["ThreadsPerServerPort"] = 2;
            this.mockFixture.Parameters["ConnectionsPerThread"] = 2;
            this.mockFixture.Parameters["DevInterruptsDifferentiator"] = "mlx";

            string exampleResults = File.ReadAllText(Path.Combine(NTttcpClientExecutorTests2.ExamplesDirectory, "ClientOutput.xml"));

            NTttcpWorkloadState executionStartedState = new NTttcpWorkloadState(ClientServerStatus.ExecutionStarted);
            Item<NTttcpWorkloadState> expectedStateItem = new Item<NTttcpWorkloadState>(nameof(NTttcpWorkloadState), executionStartedState);

            this.mockFixture.FileSystem.Setup(rt => rt.File.ReadAllTextAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(exampleResults);

            this.mockFixture.ApiClient.Setup(client => client.GetHeartbeatAsync(It.IsAny<CancellationToken>(), It.IsAny<IAsyncPolicy<HttpResponseMessage>>()))
                .ReturnsAsync(this.mockFixture.CreateHttpResponse(System.Net.HttpStatusCode.OK));

            this.mockFixture.ApiClient.Setup(client => client.GetServerOnlineStatusAsync(It.IsAny<CancellationToken>(), It.IsAny<IAsyncPolicy<HttpResponseMessage>>()))
                .ReturnsAsync(this.mockFixture.CreateHttpResponse(System.Net.HttpStatusCode.OK));

            this.mockFixture.ApiClient.SetupSequence(client => client.GetStateAsync(nameof(NTttcpWorkloadState), It.IsAny<CancellationToken>(), It.IsAny<IAsyncPolicy<HttpResponseMessage>>()))
                .ReturnsAsync(this.mockFixture.CreateHttpResponse(System.Net.HttpStatusCode.NotFound))
                .ReturnsAsync(this.mockFixture.CreateHttpResponse(System.Net.HttpStatusCode.OK, expectedStateItem))
                .ReturnsAsync(this.mockFixture.CreateHttpResponse(System.Net.HttpStatusCode.NotFound));
        }

        [Test]
        [TestCase(PlatformID.Unix, Architecture.X64)]
        [TestCase(PlatformID.Win32NT, Architecture.X64)]
        [TestCase(PlatformID.Unix, Architecture.Arm64)]
        [TestCase(PlatformID.Win32NT, Architecture.Arm64)]
        public async Task NTttcpClientExecutorSendsExpectedInstructions(PlatformID platformID, Architecture architecture)
        {
            int sendInstructionsExecuted = 0;
            this.SetupTest(platformID, architecture);

            this.mockFixture.ApiClient.Setup(client => client.SendInstructionsAsync(It.IsAny<JObject>(), It.IsAny<CancellationToken>(), It.IsAny<IAsyncPolicy<HttpResponseMessage>>()))
                .Callback<JObject, CancellationToken, IAsyncPolicy<HttpResponseMessage>>((obj, can, pol) =>
                {
                    Item<Instructions> notification = obj.ToObject<Item<Instructions>>();
                    if (notification.Definition.Type == InstructionsType.ClientServerReset)
                    {
                        Assert.AreEqual(sendInstructionsExecuted, 0);

                        sendInstructionsExecuted++;
                    }

                    if (notification.Definition.Type == InstructionsType.ClientServerStartExecution)
                    {
                        Assert.AreEqual(sendInstructionsExecuted, 1);

                        sendInstructionsExecuted++;
                    }

                    Assert.AreEqual(notification.Definition.Properties["Type"], typeof(NTttcpServerExecutor2).Name);
                    Assert.AreEqual(notification.Definition.Properties["Scenario"], "AnyScenario");
                    Assert.AreEqual(notification.Definition.Properties["Protocol"], ProtocolType.Tcp.ToString());
                    Assert.AreEqual(notification.Definition.Properties["ThreadCount"], 1);
                    Assert.AreEqual(notification.Definition.Properties["ClientBufferSize"], "4k");
                    Assert.AreEqual(notification.Definition.Properties["TestDuration"], 300);
                    Assert.AreEqual(notification.Definition.Properties["Port"], 5500);
                    Assert.AreEqual(notification.Definition.Properties["ReceiverMultiClientMode"], true);
                    Assert.AreEqual(notification.Definition.Properties["SenderLastClient"], true);
                    Assert.AreEqual(notification.Definition.Properties["ThreadsPerServerPort"], 2);
                    Assert.AreEqual(notification.Definition.Properties["ConnectionsPerThread"], 2);
                    Assert.AreEqual(notification.Definition.Properties["DevInterruptsDifferentiator"], "mlx");
                })
                .ReturnsAsync(this.mockFixture.CreateHttpResponse(System.Net.HttpStatusCode.OK));

            using (TestNTttcpClientExecutor executor = new TestNTttcpClientExecutor(this.mockFixture.Dependencies, this.mockFixture.Parameters))
            {
                await executor.ExecuteAsync(CancellationToken.None);
                Assert.AreEqual(sendInstructionsExecuted, 2);
            }
        }

        [Test]
        [TestCase(PlatformID.Unix, Architecture.X64)]
        [TestCase(PlatformID.Win32NT, Architecture.X64)]
        [TestCase(PlatformID.Unix, Architecture.Arm64)]
        [TestCase(PlatformID.Win32NT, Architecture.Arm64)]
        public async Task NTttcpClientExecutorExecutesAsExpected(PlatformID platformID, Architecture architecture)
        {
            int processExecuted = 0;
            this.SetupTest(platformID, architecture);
            string expectedPath = this.mockFixture.PlatformSpecifics.ToPlatformSpecificPath(this.mockPackage, platformID, architecture).Path;
            List<string> commandsExecuted = new List<string>();

            this.mockFixture.ProcessManager.OnCreateProcess = (file, arguments, workingDirectory) =>
            {
                processExecuted++;
                commandsExecuted.Add($"{file} {arguments}".Trim());
                return this.mockFixture.Process;
            };

            using (TestNTttcpClientExecutor executor = new TestNTttcpClientExecutor(this.mockFixture.Dependencies, this.mockFixture.Parameters))
            {
                await executor.ExecuteAsync(CancellationToken.None);

                string exe = platformID == PlatformID.Win32NT ? "NTttcp.exe" : "ntttcp";
                if (platformID == PlatformID.Win32NT)
                {
                    Assert.AreEqual(1, processExecuted);
                    CollectionAssert.AreEqual(
                    new List<string>
                    {
                    this.mockFixture.Combine(expectedPath, exe) + " -s -m 1,*,1.2.3.5 -wu 10 -cd 10 -t 300 -l  -p 5500 -xml " + this.mockFixture.Combine(expectedPath, "AnyScenario", "ntttcp-results.xml") + "  -nic 1.2.3.4"
                    },
                    commandsExecuted);
                }
                else
                {
                    Assert.AreEqual(3, processExecuted);
                    CollectionAssert.AreEqual(
                    new List<string>
                    {
                    "sudo chmod +x \"" + this.mockFixture.Combine(expectedPath, exe) + "\"",
                    this.mockFixture.Combine(expectedPath, exe) + " -s -V -m 1,*,1.2.3.5 -W 10 -C 10 -t 300 -b  -x " + this.mockFixture.Combine(expectedPath, "AnyScenario", "ntttcp-results.xml") + " -p 5500  -L -M -n 2 -l 2 --show-dev-interrupts mlx",
                    "sysctl net.ipv4.tcp_rmem net.ipv4.tcp_wmem"
                    },
                    commandsExecuted);
                }
            }
        }

        private class TestNTttcpClientExecutor : NTttcpClientExecutor2
        {
            public TestNTttcpClientExecutor(IServiceCollection services, IDictionary<string, IConvertible> parameters = null)
                : base(services, parameters)
            {
            }

            public new Task InitializeAsync(EventContext telemetryContext, CancellationToken cancellationToken)
            {
                return base.InitializeAsync(telemetryContext, cancellationToken);
            }
        }
    }
}
