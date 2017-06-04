using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.DI.Actors.Abstract;

namespace ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract
{
    internal interface ITestProbeActorCreator
    {
        ITestProbeActor Create(TestKitBase testKit);
    }
}