using Akka.Actor;

namespace ConnelHooley.AkkaTestingHelpers.Helpers.Abstract
{
    internal interface ISutSupervisorStrategyGetter
    {
        SupervisorStrategy Get(ActorBase actor);
    }
}