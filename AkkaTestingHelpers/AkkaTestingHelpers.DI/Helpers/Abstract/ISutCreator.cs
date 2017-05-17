using Akka.Actor;
using Akka.TestKit;

namespace ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract
{
    internal interface ISutCreator
    {
        TestActorRef<TActor> Create<TActor>(
            IChildWaiter childWaiter, 
            TestKitBase testKit, 
            Props props = null,
            IActorRef supervisor = null, 
            int expectedChildrenCount = 1) where TActor : ActorBase;
    }
}