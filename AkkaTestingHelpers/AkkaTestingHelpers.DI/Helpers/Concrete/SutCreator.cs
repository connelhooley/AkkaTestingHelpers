using Akka.Actor;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract;

namespace ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete
{
    internal class SutCreator : ISutCreator
    {
        public TestActorRef<TActor> Create<TActor>(IChildWaiter childWaiter, TestKitBase testKit, Props props = null, IActorRef supervisor = null, int expectedChildrenCount = 1) where TActor : ActorBase
        {
            TestActorRef<TActor> Create() => supervisor != null
                ? testKit.ActorOfAsTestActorRef<TActor>(props ?? Props.Create<TActor>(), supervisor)
                : testKit.ActorOfAsTestActorRef<TActor>(props ?? Props.Create<TActor>());

            if (expectedChildrenCount < 1)
            {
                return Create();
            }
            TestActorRef<TActor> sut = null;
            childWaiter.Wait(testKit, () =>
            {
                sut = Create();
            }, expectedChildrenCount);
            return sut;
        }
    }
}