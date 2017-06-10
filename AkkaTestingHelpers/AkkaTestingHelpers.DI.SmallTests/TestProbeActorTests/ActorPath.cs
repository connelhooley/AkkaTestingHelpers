using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.DI.Actors.Concrete;
using FluentAssertions;
using NUnit.Framework;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.TestProbeActorTests
{
    internal class ActorPath : TestBase
    {
        [Test]
        public void TestProbeActor_ActorPath_ReturnsCorrectActorPath()
        {
            //arrange
            TestActorRef<TestProbeActor> sut = CreateTestProbeActor();

            //act
            Akka.Actor.ActorPath result = sut.UnderlyingActor.ActorPath;

            //assert
            result.Should().BeSameAs(sut.Path);
        }

        [Test]
        public void TestProbeActor_ActorPath__ReturnsSameResultOnEveryCall()
        {
            //arrange
            TestProbeActor sut = CreateTestProbeActor().UnderlyingActor;

            //act
            Akka.Actor.ActorPath result = sut.ActorPath;

            //assert
            result.Should().BeSameAs(sut.ActorPath);
        }
    }
}