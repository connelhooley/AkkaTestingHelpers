using Akka.Actor;
using Akka.TestKit;

namespace ConnelHooley.AkkaTestingHelpers.DI.Actors.Abstract
{
    internal interface ITestProbeActor
    {
        ActorPath ActorPath { get; }
        TestProbe TestProbe { get; }
        ActorBase Actor { get; }
    }
}