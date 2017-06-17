using System;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.NUnit3;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.ResolvedTestProbeStoreTests
{
    internal class TestBase : TestKit
    {
        public TestBase() : base(AkkaConfig.Config) { }

        public ResolvedTestProbeStore CreateResolvedTestProbeStore() => new ResolvedTestProbeStore();

        public (ActorPath, Type, TestProbe, string) CreateChildVariables()
        {
            string name = TestUtils.Create<string>();
            ActorPath path = TestActor.Path.Child(name);
            Type type = TestUtils.Create<Type>();
            TestProbe testProbe = CreateTestProbe();
            return (path, type, testProbe, name);
        }
    }
}