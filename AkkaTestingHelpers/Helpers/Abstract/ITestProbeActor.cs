using Akka.Actor;
using Akka.TestKit;

namespace ConnelHooley.AkkaTestingHelpers.Helpers.Abstract
{
    internal interface ITestProbeActor
    {
        ActorPath ActorPath { get; }
        TestProbe TestProbe { get; }
        SupervisorStrategy PropsSupervisorStrategy { get; }
        ActorBase Actor { get; }
    }
}