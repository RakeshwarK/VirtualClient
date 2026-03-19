// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace VirtualClient.Actions
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;
    using VirtualClient.Contracts;

    [TestFixture]
    [Category("Functional")]
    public class NginxWrkProfileTests
    {
        private DependencyFixture mockFixture;

        [OneTimeSetUp]
        public void SetupFixture()
        {
            this.mockFixture = new DependencyFixture();
            ComponentTypeCache.Instance.LoadComponentTypes(TestDependencies.TestDirectory);
        }

        [Test]
        [TestCase("PERF-WEB-NGINX-WRK.json")]
        [TestCase("PERF-WEB-NGINX-WRK2.json")]
        public void NginxWrkProfileParametersAreInlinedCorrectly(string profile)
        {
            this.mockFixture.Setup(PlatformID.Unix);
            using (ProfileExecutor executor = TestDependencies.CreateProfileExecutor(profile, this.mockFixture.Dependencies))
            {
                WorkloadAssert.ParameterReferencesInlined(executor.Profile);
            }
        }

        [Test]
        [TestCase("PERF-WEB-NGINX-WRK.json")]
        [TestCase("PERF-WEB-NGINX-WRK2.json")]
        public void NginxWrkProfileParametersAreAvailable(string profile)
        {
            this.mockFixture.Setup(PlatformID.Unix);

            var serverPrams = new List<string> { "PackageName", "Role", "Timeout" };

            var reverseProxyPrams = new List<string> { "PackageName", "Role", "Timeout" };

            var clientPrams = new List<string> { "PackageName", "Role", "Timeout", "TestDuration", "FileSizeInKB", "Connection", "ThreadCount", "CommandArguments", "MetricScenario", "Scenario" };

            using (ProfileExecutor executor = TestDependencies.CreateProfileExecutor(profile, this.mockFixture.Dependencies))
            {
                foreach (var actionBlock in executor.Profile.Actions)
                {
                    string role = actionBlock.Parameters["Role"].ToString();

                    if (role.Equals("server", StringComparison.OrdinalIgnoreCase))
                    {
                        foreach (var pram in serverPrams)
                        {
                            if (!actionBlock.Parameters.ContainsKey(pram))
                            {
                                Assert.False(true, $"{actionBlock.Type} does not have {pram} parameter.");
                            }
                        }
                    }
                    else if (role.Equals("reverseproxy", StringComparison.OrdinalIgnoreCase))
                    {
                        foreach (var pram in reverseProxyPrams)
                        {
                            if (!actionBlock.Parameters.ContainsKey(pram))
                            {
                                Assert.False(true, $"{actionBlock.Type} does not have {pram} parameter.");
                            }
                        }
                    }
                    else
                    {
                        foreach (var pram in clientPrams)
                        {
                            if (!actionBlock.Parameters.ContainsKey(pram))
                            {
                                Assert.False(true, $"{actionBlock.Type} does not have {pram} parameter.");
                            }
                        }
                    }
                }
            }
        }
    }
}
