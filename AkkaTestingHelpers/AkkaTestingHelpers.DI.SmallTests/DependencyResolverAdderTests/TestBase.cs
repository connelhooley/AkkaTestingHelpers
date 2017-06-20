using Akka.Actor;
using Akka.TestKit.Xunit2;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.DependencyResolverAdderTests
{
    public class TestBase : TestKit
    {
        public TestBase() : base(AkkaConfig.Config) { }

        internal DependencyResolverAdder CreateDependencyResolverAdder() => new DependencyResolverAdder();

        public class DummyActor : ReceiveActor { }
    }
}