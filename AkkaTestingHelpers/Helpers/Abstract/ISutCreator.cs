using Akka.Actor;
using Akka.TestKit;

namespace ConnelHooley.AkkaTestingHelpers.Helpers.Abstract
{
    internal interface ISutCreator
    {
        TestActorRef<TActor> Create<TActor>(
            IWaiter childWaiter, 
            TestKitBase testKit, 
            Props props,
            int expectedChildrenCount,
            IActorRef supervisor = null) where TActor : ActorBase;
    }
}