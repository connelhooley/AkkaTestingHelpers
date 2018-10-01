using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.TestProbeParentActorTests
{
    public class UnhandledExceptions : TestBase
    {
        [Fact]
        public void TestProbeChildActorUnhandledExceptions_ReturnsEmptyResultWhenNoExceptionsHaveBeenThrown()
        {
            //arrange
            TestProbeParentActor sut = CreateTestProbeParentActor().UnderlyingActor;
            TestActorRef<DummyChildActor> child = ActorOfAsTestActorRef<DummyChildActor>(sut.Ref);
            child.Tell(new object());

            //act
            IEnumerable<Exception> result = sut.UnhandledExceptions;

            //assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void TestProbeChildActorUnhandledExceptions_ReturnsCorrectResultWhenMultipleExceptionsHaveBeenThrown()
        {
            //arrange
            TestProbeParentActor sut = CreateTestProbeParentActor().UnderlyingActor;
            TestActorRef<DummyChildActor> child = ActorOfAsTestActorRef<DummyChildActor>(sut.Ref);
            child.Tell(RestartChildException);
            child.Tell(RestartChildException);
            child.Tell(StopChildException);

            //act
            IEnumerable<Exception> result = sut.UnhandledExceptions;

            //assert
            result.Should().BeEquivalentTo(
                RestartChildException,
                RestartChildException,
                StopChildException);
        }
    }
}