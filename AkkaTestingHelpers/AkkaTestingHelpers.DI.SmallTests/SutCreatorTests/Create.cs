using System;
using Akka.Actor;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.Helpers.Abstract;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;
using FluentAssertions;
using Moq;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.SutCreatorTests
{
    public class Create : TestBase
    {
        #region Null tests
        [Fact]
        public void SutCreator_CreateWithNullChildWatcher_ThrowsArgumentNullException()
        {
            //arrange
            SutCreator sut = CreateSutCreator();

            //act
            Action act = () => sut.Create<DummyActor>(
                null, 
                this, 
                Props, 
                ExpectedChildCount, 
                Supervisor);
            
            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void SutCreator_CreateWithNullTestKitBase_ThrowsArgumentNullException()
        {
            //arrange
            SutCreator sut = CreateSutCreator();

            //act
            Action act = () => sut.Create<DummyActor>(
                ChildWaiter, 
                null, 
                Props, 
                ExpectedChildCount, 
                Supervisor);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void SutCreator_CreateWithNullProps_ThrowsArgumentNullException()
        {
            //arrange
            SutCreator sut = CreateSutCreator();

            //act
            Action act = () => sut.Create<DummyActor>(
                ChildWaiter, 
                this, 
                null, 
                ExpectedChildCount, 
                Supervisor);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void SutCreator_CreateWithNullChildWaiterAndTestKitBaseAndProps_ThrowsArgumentNullException()
        {
            //arrange
            SutCreator sut = CreateSutCreator();

            //act
            Action act = () => sut.Create<DummyActor>(
                null, 
                null, 
                null, 
                ExpectedChildCount, 
                Supervisor);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }
        #endregion

        [Fact]
        public void SutCreator_CreateWithNullSupervisor_CreatesChildWithNoSupervisor()
        {
            //arrange
            SutCreator sut = CreateSutCreator();
            
            //act
            TestActorRef<DummyActor> actor = sut.Create<DummyActor>(
                ChildWaiter, 
                this, 
                Props, 
                ExpectedChildCount, 
                null);

            //assert
            actor.Path.Parent.Should().Be(ActorPath.Parse("akka://test/user"));
        }

        [Fact]
        public void SutCreator_Create_CreatesChildWithCorrectSupervisor()
        {
            //arrange
            SutCreator sut = CreateSutCreator();

            //act
            TestActorRef<DummyActor> actor = sut.Create<DummyActor>(
                ChildWaiter, 
                this, 
                Props, 
                ExpectedChildCount, 
                Supervisor);

            //assert
            actor.UnderlyingActor.Supervisor.Should().Be(Supervisor);
        }

        [Fact]
        public void SutCreator_Create_StartsChildWaiterWithCorrectCount()
        {
            //arrange
            SutCreator sut = CreateSutCreator();

            //act
            sut.Create<DummyActor>(
                ChildWaiter, 
                this, 
                Props, 
                ExpectedChildCount, 
                Supervisor);

            //assert
            ChildWaiterMock.Verify(
                waiter => waiter.Start(this, ExpectedChildCount), 
                Times.Once);
        }

        [Fact]
        public void SutCreator_Create_OnlyStartsChildWaiterOnce()
        {
            //arrange
            SutCreator sut = CreateSutCreator();

            //act
            sut.Create<DummyActor>(
                ChildWaiter, 
                this, 
                Props, 
                ExpectedChildCount, 
                Supervisor);

            //assert
            ChildWaiterMock.Verify(
                waiter => waiter.Start(It.IsAny<TestKitBase>(), It.IsAny<int>()),
                Times.Once);
        }

        [Fact]
        public void SutCreator_Create_WaitsForChildrenUsingChildWaiter()
        {
            //arrange
            SutCreator sut = CreateSutCreator();

            //act
            sut.Create<DummyActor>(
                ChildWaiter, 
                this, 
                Props, 
                ExpectedChildCount, 
                Supervisor);

            //assert
            ChildWaiterMock.Verify(
                waiter => waiter.Wait(),
                Times.Once);
        }

        [Fact]
        public void SutCreator_Create_CreatesChildAfterStartingChildWaiter()
        {
            //arrange
            SutCreator sut = CreateSutCreator();

            //act
            sut.Create<DummyActor>(
                ChildWaiter, 
                this, 
                Props, 
                ExpectedChildCount, 
                Supervisor);

            //assert
            CallOrder.Should().ContainInOrder(
                nameof(IChildWaiter.Start), 
                "callback");
        }

        [Fact]
        public void SutCreator_Create_CreatesChildBeforeWaitingForChildrenUsingTheChildWaiter()
        {
            //arrange
            SutCreator sut = CreateSutCreator();

            //act
            sut.Create<DummyActor>(
                ChildWaiter, 
                this, 
                Props, 
                ExpectedChildCount, 
                Supervisor);

            //assert
            CallOrder.Should().ContainInOrder(
                "callback", 
                nameof(IChildWaiter.Wait));
        }
    }
}