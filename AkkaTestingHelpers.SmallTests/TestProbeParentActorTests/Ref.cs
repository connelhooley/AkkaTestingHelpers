using Akka.Actor;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.TestProbeParentActorTests
{
    public class Ref : TestBase
    {
        [Fact]
        public void TestProbeParentActor_Ref_ReturnsCorrectActorReference()
        {
            //arrange
            TestActorRef<TestProbeParentActor> sut = CreateTestProbeParentActor();

            //act
            IActorRef result = sut.UnderlyingActor.Ref;

            //assert
            result.Should().Be(sut.Ref);
        }
    }
}
