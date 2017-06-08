using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.DI.Actors.Abstract;
using ConnelHooley.AkkaTestingHelpers.DI.Actors.Concrete;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract;

namespace ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete
{
    internal sealed class TestProbeActorCreator : ITestProbeActorCreator
    {
        public ITestProbeActor Create(TestKitBase testKit) => 
            new TestProbeActor(testKit);
    }
}