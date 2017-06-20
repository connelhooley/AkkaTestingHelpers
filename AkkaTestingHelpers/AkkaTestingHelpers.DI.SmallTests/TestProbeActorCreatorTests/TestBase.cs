using Akka.TestKit.Xunit2;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.TestProbeActorCreatorTests
{
    public class TestBase : TestKit
    {
        public TestBase() : base(AkkaConfig.Config) { }

        internal TestProbeActorCreator CreateTestProbeActorFactory() => new TestProbeActorCreator();
    }
}