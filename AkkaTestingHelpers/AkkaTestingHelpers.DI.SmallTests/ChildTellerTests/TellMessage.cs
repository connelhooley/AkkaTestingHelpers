using System;
using Akka.Actor;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;
using FluentAssertions;
using Moq;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.ChildTellerTests
{
    internal class TellMessage : TestBase
    {
        #region null checks
        [Fact]
        public void ChildTeller_TellMessageWithNullChildWaiter_ThrowsArgumentNullException()
        {
            //arrange
            ChildTeller sut = CreateChildTeller();

            //act
            Action act = () => sut.TellMessage(null, this, Recipient, Message, ExpectedChildrenCount, Sender);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }
        [Fact]
        public void ChildTeller_TellMessageWithNullTestKitBase_ThrowsArgumentNullException()
        {
            //arrange
            ChildTeller sut = CreateChildTeller();

            //act
            Action act = () => sut.TellMessage(ChildWaiter, null, Recipient, Message, ExpectedChildrenCount, Sender);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void ChildTeller_TellMessageWithNullRecipient_ThrowsArgumentNullException()
        {
            //arrange
            ChildTeller sut = CreateChildTeller();

            //act
            Action act = () => sut.TellMessage(ChildWaiter, this, null, Message, ExpectedChildrenCount, Sender);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void ChildTeller_TellMessageWithNullMessage_ThrowsArgumentNullException()
        {
            //arrange
            ChildTeller sut = CreateChildTeller();

            //act
            Action act = () => sut.TellMessage<object>(ChildWaiter, this, Recipient, null, ExpectedChildrenCount, Sender);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void ChildTeller_TellMessageWithNullChildWaiterAndTestKitBaseAndRecipientAndMessage_ThrowsArgumentNullException()
        {
            //arrange
            ChildTeller sut = CreateChildTeller();

            //act
            Action act = () => sut.TellMessage<object>(null, null, null, null, ExpectedChildrenCount, Sender);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        #endregion

        [Fact]
        public void ChildTeller_TellMessage_StartsWaitingForChildren()
        {
            //arrange
            ChildTeller sut = CreateChildTeller();

            //act
            sut.TellMessage(ChildWaiter, this, Recipient, Message, ExpectedChildrenCount, Sender);

            //assert
            ChildWaiterMock.Verify(
                waiter => waiter.Start(this, ExpectedChildrenCount),
                Times.Once);
        }

        [Fact]
        public void ChildTeller_TellMessageWithNoSender_StartsWaitingForChildren()
        {
            //arrange
            ChildTeller sut = CreateChildTeller();

            //act
            sut.TellMessage(ChildWaiter, this, Recipient, Message, ExpectedChildrenCount);

            //assert
            ChildWaiterMock.Verify(
                waiter => waiter.Start(this, ExpectedChildrenCount),
                Times.Once);
        }

        [Fact]
        public void ChildTeller_TellMessage_WaitsForChildren()
        {
            //arrange
            ChildTeller sut = CreateChildTeller();

            //act
            sut.TellMessage(ChildWaiter, this, Recipient, Message, ExpectedChildrenCount, Sender);

            //assert
            ChildWaiterMock.Verify(
                waiter => waiter.Wait(),
                Times.Once);
        }

        [Fact]
        public void ChildTeller_TellMessageWithNoSender_WaitsForChildren()
        {
            //arrange
            ChildTeller sut = CreateChildTeller();

            //act
            sut.TellMessage(ChildWaiter, this, Recipient, Message, ExpectedChildrenCount);

            //assert
            ChildWaiterMock.Verify(
                waiter => waiter.Wait(),
                Times.Once);
        }

        [Fact]
        public void ChildTeller_TellMessage_TellsRecipient()
        {
            //arrange
            ChildTeller sut = CreateChildTeller();

            //act
            sut.TellMessage(ChildWaiter, this, Recipient, Message, ExpectedChildrenCount, Sender);

            //assert
            RecipientMock.Verify(actorRef => actorRef.Tell(Message, Sender));
        }

        [Fact]
        public void ChildTeller_TellMessageWithNoSender_TellsRecipient()
        {
            //arrange
            ChildTeller sut = CreateChildTeller();

            //act
            sut.TellMessage(ChildWaiter, this, Recipient, Message, ExpectedChildrenCount);

            //assert
            RecipientMock.Verify(actorRef => actorRef.Tell(Message, TestActor));
        }

        [Fact]
        public void ChildTeller_TellMessage_StartsWaitingForChildrenBeforeTellingRecipient()
        {
            //arrange
            ChildTeller sut = CreateChildTeller();

            //act
            sut.TellMessage(ChildWaiter, this, Recipient, Message, ExpectedChildrenCount, Sender);

            //assert
            CallOrder.Should().ContainInOrder(nameof(IChildWaiter.Start), nameof(IActorRef.Tell) + "Sender");
        }

        [Fact]
        public void ChildTeller_TellMessageWithNoSender_StartsWaitingForChildrenBeforeTellingRecipient()
        {
            //arrange
            ChildTeller sut = CreateChildTeller();

            //act
            sut.TellMessage(ChildWaiter, this, Recipient, Message, ExpectedChildrenCount);

            //assert
            CallOrder.Should().ContainInOrder(nameof(IChildWaiter.Start), nameof(IActorRef.Tell));
        }

        [Fact]
        public void ChildTeller_TellMessage_WaitsForChildrenAfterTellingRecipient()
        {
            //arrange
            ChildTeller sut = CreateChildTeller();

            //act
            sut.TellMessage(ChildWaiter, this, Recipient, Message, ExpectedChildrenCount, Sender);

            //assert
            CallOrder.Should().ContainInOrder(nameof(IActorRef.Tell) + "Sender", nameof(IChildWaiter.Wait));
        }

        [Fact]
        public void ChildTeller_TellMessageWithNoSender_WaitsForChildrenAfterTellingRecipient()
        {
            //arrange
            ChildTeller sut = CreateChildTeller();

            //act
            sut.TellMessage(ChildWaiter, this, Recipient, Message, ExpectedChildrenCount);

            //assert
            CallOrder.Should().ContainInOrder(nameof(IActorRef.Tell), nameof(IChildWaiter.Wait));
        }
    }
}