using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;
using ConnelHooley.TestHelpers;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.TestProbeActorTests
{
    public class TestProbe : TestBase
    {
        [Fact]
        public void TestProbeActor_TestProbe_ReturnsTestProbeFromTestProbeCreator()
        {
            //arrange
            TestActorRef<TestProbeActor> sut = CreateTestProbeActorWithoutSupervisorStrategy();

            //act
            global::Akka.TestKit.TestProbe result = sut.UnderlyingActor.TestProbe;

            //assert
            result.Should().BeSameAs(TestProbe);
        }

        [Fact]
        public void TestProbeActor_TestProbe_ReturnsSameResultOnEveryCall()
        {
            //arrange
            TestActorRef<TestProbeActor> sut = CreateTestProbeActorWithoutSupervisorStrategy();

            //act
            global::Akka.TestKit.TestProbe result = sut.UnderlyingActor.TestProbe;

            //assert
            result.Should().BeSameAs(sut.UnderlyingActor.TestProbe);
        }

        [Fact]
        public void TestProbeActor_TestProbe_TestProbeIsForwardedMessages()
        {
            //arrange
            TestActorRef<TestProbeActor> sut = CreateTestProbeActorWithoutSupervisorStrategy();
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