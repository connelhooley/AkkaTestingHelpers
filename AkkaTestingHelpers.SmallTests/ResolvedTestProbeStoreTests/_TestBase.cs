using System;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.Xunit2;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;
using ConnelHooley.TestHelpers;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.ResolvedTestProbeStoreTests
{
    public class TestBase : TestKit
    {
        public TestBase() : base(AkkaConfig.Config) { }

        internal ResolvedTestProbeStore CreateResolvedTestProbeStore() => new ResolvedTestProbeStore();

        internal (ActorPath ActorPath, Type Type, TestProbe TestProbe, SupervisorStrategy SupervisorStrategy, string ActorName) CreateChildVariables()
        {
            string name = TestHelper.GenerateString();
            ActorPath path = TestActor.Path.Child(name);
            Type type = TestHelper.Generate<Type>();
            TestProbe testProbe = CreateTestProbe();
            AllForOneStrategy supervisorStrategy = new AllForOneStrategy(
                TestHelper.GenerateNumber(),
                TestHelper.GenerateNumber(),
                exception => TestHelper.Generate<Directive>());
            return (path, type, testProbe, supervisorStrategy, name);
        }
    }
}