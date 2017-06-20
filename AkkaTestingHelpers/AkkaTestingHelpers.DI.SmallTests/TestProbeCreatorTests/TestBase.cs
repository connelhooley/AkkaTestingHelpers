using Akka.TestKit.Xunit2;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.TestProbeCreatorTests
{
    public class TestBase : TestKit
    {
        public TestBase() : base(AkkaConfig.Config) { }

        internal TestProbeCreator CreateTestProbeCreator() => new TestProbeCreator();
    }
}