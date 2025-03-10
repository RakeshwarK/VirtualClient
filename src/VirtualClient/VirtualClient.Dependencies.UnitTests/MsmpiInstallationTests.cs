// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace VirtualClient.Dependencies
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using NUnit.Framework;
    using VirtualClient.Common;
    using VirtualClient.Common.Telemetry;
    using VirtualClient.Contracts;

    [TestFixture]
    [Category("Unit")]
    public class MsmpiInstallationTests
    {
        private MockFixture mockFixture;

        public void SetupTest(PlatformID platform)
        {
            this.mockFixture = new MockFixture();
            this.mockFixture.Setup(platform);

            this.mockFixture.Directory.Setup(d => d.Exists(It.IsAny<string>()))
                .Returns(true);

            this.mockFixture.File.Reset();
            this.mockFixture.File.Setup(f => f.Exists(It.IsAny<string>()))
                .Returns(true);

            this.mockFixture.File.Setup(f => f.Exists(It.IsRegex(".*msmpisuccess.lock")))
                .Returns(false);

            DependencyPath package = new DependencyPath("msmpi", this.mockFixture.PlatformSpecifics.GetPackagePath("msmpi"));
            this.mockFixture.SetupPackage(package);

            this.mockFixture.Parameters = new Dictionary<string, IConvertible>()
            {
                { nameof(MsmpiInstallation.PackageName), "msmpi" }
            };
        }

        [Test]
        public async Task MsmpiInstallationRunsTheExpectedCommandInWindows()
        {
            this.SetupTest(PlatformID.Win32NT);

            ProcessStartInfo expectedInfo = new ProcessStartInfo();
            List<string> expectedCommands = new List<string>()
            {
                $@"msiexec.exe /i ""{this.mockFixture.GetPackagePath()}\msmpi\win-x64\msmpisdk.msi"" /qn"
            };

            int commandExecuted = 0;
            this.mockFixture.ProcessManager.OnCreateProcess = (exe, arguments, workingDir) =>
            {
                if (expectedCommands.Any(c => c == $"{exe} {arguments}"))
                {
                    commandExecuted++;
                }

                IProcessProxy process = new InMemoryProcess()
                {
                    ExitCode = 0,
                    OnStart = () => true,
                    OnHasExited = () => true
                };
                return process;
            };

            using (TestMsmpiInstallation installation = new TestMsmpiInstallation(this.mockFixture.Dependencies, this.mockFixture.Parameters))
            {
                await installation.ExecuteAsync(CancellationToken.None);
            }

            Assert.AreEqual(1, commandExecuted);
        }

        [Test]
        public void MsmpiInstallationDoesNotExecuteOnLinuxSystems()
        {
            this.SetupTest(PlatformID.Unix);

            using (TestMsmpiInstallation installation = new TestMsmpiInstallation(this.mockFixture.Dependencies, this.mockFixture.Parameters))
            {
                Assert.IsFalse(VirtualClientComponent.IsSupported(installation));
            }
        }

        private class TestMsmpiInstallation : MsmpiInstallation
        {
            public TestMsmpiInstallation(IServiceCollection dependencies, IDictionary<string, IConvertible> parameters)
                : base(dependencies, parameters)
            {
            }

            public new Task ExecuteAsync(EventContext context, CancellationToken cancellationToken)
            {
                return base.ExecuteAsync(context, cancellationToken);
            }
        }
    }
}