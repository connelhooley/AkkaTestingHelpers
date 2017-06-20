using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.DI.Actors.Concrete;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.TestProbeActorTests
{
    internal class TestProbe : TestBase
    {
        [Fact]
        public void TestProbeActor_TestProbe_ReturnedTestProbeIsForwardedMessages()
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

        [Fact]
        public void TestProbeActor_TestProbe_ReturnsSameResultOnEveryCall()
        {
            //arrange
            TestProbeActor sut = CreateTestProbeActor().UnderlyingActor;

            //act
            Akka.TestKit.TestProbe result = sut.TestProbe;

            //assert
            result.Should().BeSameAs(sut.TestProbe);
        }
    }
}