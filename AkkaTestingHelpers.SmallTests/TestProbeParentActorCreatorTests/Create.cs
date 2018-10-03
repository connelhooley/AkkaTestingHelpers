using System;
using ConnelHooley.AkkaTestingHelpers.Helpers.Abstract;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;
using FluentAssertions;
using Xunit;
using Akka.TestKit;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.TestProbeParentActorCreatorTests
{
    public class Create : TestBase
    {
        #region Null tests
        [Fact]
        public void TestProbeParentActorCreator_CreateWithNullTestProbeCreator_ThrowsArgumentNullException()
        {
            //arrange
            TestProbeParentActorCreator sut = CreateTestProbeParentActorCreator();

            //act
            Action act = () => sut.Create(
                null,
                ExceptionWaiter,
                this,
                Decider,
                Handlers);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeParentActorCreator_CreateWithNullExceptionWaiter_ThrowsArgumentNullException()
        {
            //arrange
            TestProbeParentActorCreator sut = CreateTestProbeParentActorCreator();

            //act
            Action act = () => sut.Create(
                TestProbeCreator,
                null,
                this,
                Decider,
                Handlers);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeParentActorCreator_CreateWithNullTestKit_ThrowsArgumentNullException()
        {
            //arrange
            TestProbeParentActorCreator sut = CreateTestProbeParentActorCreator();

            //act
            Action act = () => sut.Create(
                TestProbeCreator,
                ExceptionWaiter,
                null,
                Decider,
                Handlers);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeParentActorCreator_CreateWithNullDecider_ThrowsArgumentNullException()
        {
            //arrange
            TestProbeParentActorCreator sut = CreateTestProbeParentActorCreator();

            //act
            Action act = () => sut.Create(
                TestProbeCreator,
                ExceptionWaiter,
                this,
                null,
                Handlers);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeParentActorCreator_CreateWithNullHandlers_ThrowsArgumentNullException()
        {
            //arrange
            TestProbeParentActorCreator sut = CreateTestProbeParentActorCreator();

            //act
            Action act = () => sut.Create(
                TestProbeCreator,
                ExceptionWaiter,
                this,
                Decider,
                null);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }
        #endregion

        [Fact]
        public void TestProbeParentActorCreator_Create_TheReturnedActorsRefPropertyCanBeUsedToSuprviseOtherActors()
        {
            //arrange
            TestProbeParentActorCreator sut = CreateTestProbeParentActorCreator();

            //act
            ITestProbeParentActor result = sut.Create(
                TestProbeCreator,
                ExceptionWaiter,
                this,
                Decider,
                Handlers);

            //assert
            Guid message = Guid.NewGuid();
            TestActorRef<DummyActor> childActor = ActorOfAsTestActorRef<DummyActor>(result.Ref);
            childActor.Tell(message);
            result.TestProbe.ExpectMsg(message);
        }
    }
}