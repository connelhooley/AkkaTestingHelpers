using Akka.TestKit.NUnit3;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.TestProbeCreatorTests
{
    internal class TestBase : TestKit
    {
        public TestProbeCreator CreateTestProbeCreator() => new TestProbeCreator();
    }
}