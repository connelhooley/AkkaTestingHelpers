using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.TestProbeActorTests
{
    public class ActorPath : TestBase
    {
        [Fact]
        public void TestProbeActor_ActorPath_ReturnsCorrectActorPath()
        {
            //arrange
            TestActorRef<TestProbeActor> sut = CreateTestProbeActor();

            //act
            Akka.Actor.ActorPath result = sut.UnderlyingActor.ActorPath;

            //assert
            result.Should().BeSameAs(sut.Path);
        }

        [Fact]
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