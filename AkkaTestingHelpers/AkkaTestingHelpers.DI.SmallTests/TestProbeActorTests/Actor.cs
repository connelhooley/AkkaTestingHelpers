using Akka.Actor;
using ConnelHooley.AkkaTestingHelpers.DI.Actors.Concrete;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.TestProbeActorTests
{
    public class Actor : TestBase
    {
        [Fact]
        public void TestProbeActor_Actror_ReturnsActor()
        {
            //arrange
            TestProbeActor sut = CreateTestProbeActor().UnderlyingActor;

            //act
            ActorBase result = sut.Actor;

            //assert
            result.Should().BeSameAs(sut);
        }

        [Fact]
        public void TestProbeActor_Actror_ReturnsSameResultOnEveryCall()
        {
            //arrange
            TestProbeActor sut = CreateTestProbeActor().UnderlyingActor;

            //act
            ActorBase result = sut.Actor;

            //assert
            result.Should().BeSameAs(sut.Actor);
        }
    }
}