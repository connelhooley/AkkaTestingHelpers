using Akka.Actor;

namespace ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract
{
    internal interface ISutSupervisorStrategyGetter
    {
        SupervisorStrategy Get(ActorBase actor);
    }
}