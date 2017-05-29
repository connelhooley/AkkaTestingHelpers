using Akka.TestKit;

namespace ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract
{
    internal interface ITestProbeCreator
    {
        TestProbe Create(TestKitBase testKit);
    }
}