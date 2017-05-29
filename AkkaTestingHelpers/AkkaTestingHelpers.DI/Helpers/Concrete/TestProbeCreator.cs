using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract;

namespace ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete
{
    internal class TestProbeCreator : ITestProbeCreator
    {
        public TestProbe Create(TestKitBase testKit) => testKit.CreateTestProbe();
    }
}