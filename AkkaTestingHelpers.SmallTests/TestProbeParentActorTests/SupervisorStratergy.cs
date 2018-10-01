using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;
using Moq;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.TestProbeParentActorTests
{
    public class SupervisorStratergy : TestBase
    {
        [Fact]
        public void TestProbeParentActor_SupervisorStrategyWithDeciderThatReturnsStop_StopsChild()
        {
            //act
            TestProbeParentActor sut = CreateTestProbeParentActor().UnderlyingActor;

            //assert
            TestActorRef<DummyChildActor> child = ActorOfAsTestActorRef<DummyChildActor>(sut.Ref);
            Watch(child);
            child.Tell(StopChildException);
            ExpectTerminated(child);
        }

        [Fact]
        public void TestProbeParentActor_SupervisorStrategyWithDeciderThatReturnsRestart_RestartsChild()
        {
            //act
            TestProbeParentActor sut = CreateTestProbeParentActor().UnderlyingActor;

            //assert
            TestActorRef<DummyChildActor> child = ActorOfAsTestActorRef<DummyChildActor>(sut.Ref);
            child.Tell(RestartChildException);
            child.Tell(new object());
            ExpectMsg(DummyChildActor.ReplyAfterRestart);
        }

        [Fact]
        public void TestProbeParentActor_SupervisorStrategy_ResolvesEventInExceptionWaiter()
        {
            //act
            TestProbeParentActor sut = CreateTestProbeParentActor().UnderlyingActor;

            //assert
            TestActorRef<DummyChildActor> child = ActorOfAsTestActorRef<DummyChildActor>(sut.Ref);
            child.Tell(RestartChildException);
            AwaitAssert(() => ExceptionWaiterMock.Verify(
                waiter => waiter.ResolveEvent(),
                Times.Once));
        }
    }
}
