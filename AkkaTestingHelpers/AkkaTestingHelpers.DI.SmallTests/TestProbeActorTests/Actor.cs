using Akka.Actor;
using ConnelHooley.AkkaTestingHelpers.DI.Actors.Concrete;
using FluentAssertions;
using NUnit.Framework;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.TestProbeActorTests
{
    internal class Actor : TestBase
    {
        [Test]
        public void TestProbeActor_Actror_ReturnsActor()
        {
            //arrange
            TestProbeActor sut = CreateTestProbeActor().UnderlyingActor;

            //act
            ActorBase result = sut.Actor;

            //assert
            result.Should().BeSameAs(sut);
        }

        [Test]
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