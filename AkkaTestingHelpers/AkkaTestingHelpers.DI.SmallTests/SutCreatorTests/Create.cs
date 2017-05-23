using System;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.SutCreatorTests
{
    internal class Create : TestBase
    {
        [Test]
        public void SutCreator_CreateWithNullChildWatcher_ThrowsArgumentNullException()
        {
            //arrange
            SutCreator sut = CreateSutCreator();

            //act
            Action act = () => sut.Create<DummyActor>(null, this, SutProps, ExpectedChildCount, Supervisor);
            
            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void SutCreator_CreateWithNullTestKitBase_ThrowsArgumentNullException()
        {
            //arrange
            SutCreator sut = CreateSutCreator();

            //act
            Action act = () => sut.Create<DummyActor>(ChildWaiter, null, SutProps, ExpectedChildCount, Supervisor);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public async Task SutCreator_CreateWithNullSupervisor_CreatesChildWithNoSupervisor()
        {
            //arrange
            IActorRef rootGuardian = await Sys.ActorSelection("akka://test/user").ResolveOne(TimeSpan.FromSeconds(5));
            SutCreator sut = CreateSutCreator();

            //act
            TestActorRef<DummyActor> actor = sut.Create<DummyActor>(ChildWaiter, this, SutProps, ExpectedChildCount);

            //assert
            actor.UnderlyingActor.Supervisor.Should().Be(rootGuardian);
        }

        [Test]
        public void SutCreator_Create_CreatesChildWithCorrectSupervisor()
        {
            //arrange
            SutCreator sut = CreateSutCreator();

            //act
            TestActorRef<DummyActor> actor = sut.Create<DummyActor>(ChildWaiter, this, SutProps, ExpectedChildCount, Supervisor);

            //assert
            actor.UnderlyingActor.Supervisor.Should().Be(Supervisor);
        }

        [Test]
        public void SutCreator_Create_StartsChildWaiterWithCorrectCount()
        {
            //arrange
            SutCreator sut = CreateSutCreator();

            //act
            sut.Create<DummyActor>(ChildWaiter, this, SutProps, ExpectedChildCount, Supervisor);

            //assert
            ChildWaiterMock.Verify(
                waiter => waiter.Start(this, ExpectedChildCount), 
                Times.Once);
        }

        [Test]
        public void SutCreator_Create_OnlyStartsChildWaiterOnce()
        {
            //arrange
            SutCreator sut = CreateSutCreator();

            //act
            sut.Create<DummyActor>(ChildWaiter, this, SutProps, ExpectedChildCount, Supervisor);

            //assert
            ChildWaiterMock.Verify(
                waiter => waiter.Start(this, It.IsAny<int>()),
                Times.Once);
        }

        [Test]
        public void SutCreator_Create_WaitsForChildrenUsingChildWaiter()
        {
            //arrange
            SutCreator sut = CreateSutCreator();

            //act
            sut.Create<DummyActor>(ChildWaiter, this, SutProps, ExpectedChildCount, Supervisor);

            //assert
            ChildWaiterMock.Verify(
                waiter => waiter.Wait(),
                Times.Once);
        }

        [Test]
        public void SutCreator_Create_CreatesChildAfterStartingChildWaiter()
        {
            //arrange
            SutCreator sut = CreateSutCreator();

            //act
            sut.Create<DummyActor>(ChildWaiter, this, SutProps, ExpectedChildCount, Supervisor);

            //assert
            WhenChildCreated.Should().BeAfter(WhenChildWaiterStart);
        }

        [Test]
        public void SutCreator_Create_CreatesChildBeforeWaitingForChildrenUsingTheChildWaiter()
        {
            //arrange
            SutCreator sut = CreateSutCreator();

            //act
            sut.Create<DummyActor>(ChildWaiter, this, SutProps, ExpectedChildCount, Supervisor);

            //assert
            WhenChildCreated.Should().BeBefore(WhenChildWaiterWait);
        }
    }
}