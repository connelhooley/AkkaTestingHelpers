using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;
using ConnelHooley.TestHelpers;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.TestProbeChildActorTests
{
    public class TestProbe : TestBase
    {
        [Fact]
        public void TestProbeChildActor_TestProbe_ReturnsTestProbeFromTestProbeCreator()
        {
            //arrange
            TestActorRef<TestProbeChildActor> sut = CreateTestProbeChildActorWithoutSupervisorStrategy();

            //act
            global::Akka.TestKit.TestProbe result = sut.UnderlyingActor.TestProbe;

            //assert
            result.Should().BeSameAs(TestProbe);
        }

        [Fact]
        public void TestProbeChildActor_TestProbe_ReturnsSameResultOnEveryCall()
        {
            //arrange
            TestActorRef<TestProbeChildActor> sut = CreateTestProbeChildActorWithoutSupervisorStrategy();

            //act
            global::Akka.TestKit.TestProbe result = sut.UnderlyingActor.TestProbe;

            //assert
            result.Should().BeSameAs(sut.UnderlyingActor.TestProbe);
        }

        [Fact]
        public void TestProbeChildActor_TestProbe_TestProbeIsForwardedMessages()
        {
            //arrange
            TestActorRef<TestProbeChildActor> sut = CreateTestProbeChildActorWithoutSupervisorStrategy();
            object message = TestHelper.Generate<object>();
            global::Akka.TestKit.TestProbe sender = CreateTestProbe();

            //act
            global::Akka.TestKit.TestProbe result = sut.UnderlyingActor.TestProbe;

            //assert
            sut.Tell(message, sender);
            result.ExpectMsgFrom(sender, message);
        }
    }
}