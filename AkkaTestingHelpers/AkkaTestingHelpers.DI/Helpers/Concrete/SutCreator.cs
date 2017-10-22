using Akka.Actor;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract;
using NullGuard;

namespace ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete
{
    internal sealed class SutCreator : ISutCreator
    {
        public TestActorRef<TActor> Create<TActor>(IChildWaiter childWaiter, TestKitBase testKit, Props props, int expectedChildrenCount, [AllowNull] IActorRef supervisor = null) where TActor : ActorBase
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