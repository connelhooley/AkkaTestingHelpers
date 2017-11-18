using Akka.TestKit;

namespace ConnelHooley.AkkaTestingHelpers.Helpers.Abstract
{
    internal interface ITestProbeCreator
    {
        TestProbe Create(TestKitBase testKit);
    }
}