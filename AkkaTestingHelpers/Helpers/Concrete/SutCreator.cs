using Akka.Actor;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.Helpers.Abstract;

namespace ConnelHooley.AkkaTestingHelpers.Helpers.Concrete
{
    internal sealed class SutCreator : ISutCreator
    {
        public TestActorRef<TActor> Create<TActor>(IWaiter childWaiter, TestKitBase testKit, Props props, int expectedChildrenCount, IActorRef supervisor) where TActor : ActorBase
        {
            childWaiter.Start(testKit, expectedChildrenCount);
            TestActorRef<TActor> sut = supervisor != null
                ? testKit.ActorOfAsTestActorRef<TActor>(props, supervisor)
                : testKit.ActorOfAsTestActorRef<TActor>(props);
            childWaiter.Wait();
            return sut;
        }
    }
}