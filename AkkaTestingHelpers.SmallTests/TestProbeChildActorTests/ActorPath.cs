using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.TestProbeChildActorTests
{
    public class ActorPath : TestBase
    {
        [Fact]
        public void TestProbeChildActor_ActorPath_ReturnsCorrectActorPath()
        {
            //arrange
            TestActorRef<TestProbeChildActor> sut = CreateTestProbeChildActorWithoutSupervisorStrategy();

            //act
            global::Akka.Actor.ActorPath result = sut.UnderlyingActor.ActorPath;

            //assert
            result.Should().BeSameAs(sut.Path);
        }

        [Fact]
        public void TestProbeChildActor_ActorPath__ReturnsSameResultOnEveryCall()
        {
            //arrange
            TestProbeChildActor sut = CreateTestProbeChildActorWithoutSupervisorStrategy().UnderlyingActor;

            //act
            global::Akka.Actor.ActorPath result = sut.ActorPath;

            //assert
            result.Should().BeSameAs(sut.ActorPath);
        }
    }
}