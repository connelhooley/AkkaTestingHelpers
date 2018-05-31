using Akka.Actor;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.TestProbeChildActorTests
{
    public class Actor : TestBase
    {
        [Fact]
        public void TestProbeChildActor_Actor_ReturnsSelfAsActor()
        {
            //arrange
            TestProbeChildActor sut = CreateTestProbeChildActorWithoutSupervisorStrategy().UnderlyingActor;

            //act
            ActorBase result = sut.Actor;

            //assert
            result.Should().BeSameAs(sut);
        }

        [Fact]
        public void TestProbeChildActor_Actor_ReturnsSameResultOnEveryCall()
        {
            //arrange
            TestProbeChildActor sut = CreateTestProbeChildActorWithoutSupervisorStrategy().UnderlyingActor;

            //act
            ActorBase result = sut.Actor;

            //assert
            result.Should().BeSameAs(sut.Actor);
        }
    }
}