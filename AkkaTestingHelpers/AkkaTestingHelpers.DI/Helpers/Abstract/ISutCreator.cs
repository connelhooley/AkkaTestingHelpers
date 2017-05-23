using Akka.Actor;
using Akka.TestKit;

namespace ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract
{
    internal interface ISutCreator
    {
        TestActorRef<TActor> Create<TActor>(
            IChildWaiter childWaiter, 
            TestKitBase testKit, 
            Props props,
            int expectedChildrenCount = 1,
            IActorRef supervisor = null) where TActor : ActorBase;
    }
}