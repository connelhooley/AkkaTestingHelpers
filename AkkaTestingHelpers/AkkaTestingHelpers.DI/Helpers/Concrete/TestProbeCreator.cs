using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.Helpers.Abstract;

namespace ConnelHooley.AkkaTestingHelpers.Helpers.Concrete
{
    internal sealed class TestProbeCreator : ITestProbeCreator
    {
        public TestProbe Create(TestKitBase testKit) => testKit.CreateTestProbe();
    }
}