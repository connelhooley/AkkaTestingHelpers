using Akka.Actor;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.TestProbeActorTests
{
    public class Actor : TestBase
    {
        [Fact]
        public void TestProbeActor_Actor_ReturnsSelfAsActor()
        {
            //arrange
            TestProbeActor sut = CreateTestProbeActorWithoutSupervisorStrategy().UnderlyingActor;

            //act
            ActorBase result = sut.Actor;

            //assert
            result.Should().BeSameAs(sut);
        }

        [Fact]
        public void TestProbeActor_Actor_ReturnsSameResultOnEveryCall()
        {
            //arrange
            TestProbeActor sut = CreateTestProbeActorWithoutSupervisorStrategy().UnderlyingActor;

            //act
            ActorBase result = sut.Actor;

            //assert
            result.Should().BeSameAs(sut.Actor);
        }
    }
}