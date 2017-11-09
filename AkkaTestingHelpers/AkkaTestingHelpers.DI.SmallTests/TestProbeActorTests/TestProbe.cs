using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.TestProbeActorTests
{
    public class TestProbe : TestBase
    {
        [Fact]
        public void TestProbeActor_TestProbe_ReturnsTestProbeFromTestProbeCreator()
        {
            //arrange
            TestActorRef<TestProbeActor> sut = CreateTestProbeActor();

            //act
            Akka.TestKit.TestProbe result = sut.UnderlyingActor.TestProbe;

            //assert
            result.Should().BeSameAs(TestProbe);
        }

        [Fact]
        public void TestProbeActor_TestProbe_ReturnsSameResultOnEveryCall()
        {
            //arrange
            TestActorRef<TestProbeActor> sut = CreateTestProbeActor();

            //act
            Akka.TestKit.TestProbe result = sut.UnderlyingActor.TestProbe;

            //assert
            result.Should().BeSameAs(sut.UnderlyingActor.TestProbe);
        }

        [Fact]
        public void TestProbeActor_TestProbe_TestProbeIsForwardedMessages()
        {
            //arrange
            TestActorRef<TestProbeActor> sut = CreateTestProbeActor();
            object message = TestUtils.Create<object>();
            Akka.TestKit.TestProbe sender = CreateTestProbe();

            //act
            Akka.TestKit.TestProbe result = sut.UnderlyingActor.TestProbe;

            //assert
            sut.Tell(message, sender);
            result.ExpectMsgFrom(sender, message);
        }
    }
}