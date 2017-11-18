using System;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.Xunit2;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.ResolvedTestProbeStoreTests
{
    public class TestBase : TestKit
    {
        public TestBase() : base(AkkaConfig.Config) { }

        internal ResolvedTestProbeStore CreateResolvedTestProbeStore() => new ResolvedTestProbeStore();

        internal (ActorPath ActorPath, Type Type, TestProbe TestProbe, SupervisorStrategy SupervisorStrategy, string ActorName) CreateChildVariables()
        {
            string name = TestUtils.Create<string>();
            ActorPath path = TestActor.Path.Child(name);
            Type type = TestUtils.Create<Type>();
            TestProbe testProbe = CreateTestProbe();
            AllForOneStrategy supervisorStrategy = new AllForOneStrategy(
                TestUtils.Create<int>(),
                TestUtils.Create<int>(),
                exception => TestUtils.Create<Directive>());
            return (path, type, testProbe, supervisorStrategy, name);
        }
    }
}