using System;
using Akka.Actor;
using ConnelHooley.AkkaTestingHelpers.Helpers.Abstract;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;
using FluentAssertions;
using Moq;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.TellWaiterTests
{
    public class TellMessage : TestBase
    {
        #region Null tests
        [Fact]
        public void TellWaiter_TellMessageWithNullWaiter_ThrowsArgumentNullException()
        {
            //arrange
            TellWaiter sut = CreateTellWaiter();

            //act
            Action act = () => sut.TellMessage(
                null, 
                this, 
                Recipient, 
                Message, 
                ExpectedEventCount, 
                Sender);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void TellWaiter_TellMessageWithNullTestKitBase_ThrowsArgumentNullException()
        {
            //arrange
            TellWaiter sut = CreateTellWaiter();

            //act
            Action act = () => sut.TellMessage(
                Waiter, 
                null, 
                Recipient, 
                Message, 
                ExpectedEventCount, 
                Sender);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void TellWaiter_TellMessageWithNullRecipient_ThrowsArgumentNullException()
        {
            //arrange
            TellWaiter sut = CreateTellWaiter();

            //act
            Action act = () => sut.TellMessage(
                Waiter, 
                this, 
                null, 
                Message, 
                ExpectedEventCount, 
                Sender);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void TellWaiter_TellMessageWithNullMessage_ThrowsArgumentNullException()
        {
            //arrange
            TellWaiter sut = CreateTellWaiter();

            //act
            Action act = () => sut.TellMessage<object>(
                Waiter, 
                this, 
                Recipient, 
                null, 
                ExpectedEventCount, 
                Sender);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void TellWaiter_TellMessageWithNullWaiterAndTestKitBaseAndRecipientAndMessage_ThrowsArgumentNullException()
        {
            //arrange
            TellWaiter sut = CreateTellWaiter();

            //act
            Action act = () => sut.TellMessage<object>(
                null, 
                null, 
                null, 
                null, 
                ExpectedEventCount, 
                Sender);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }
        #endregion

        [Fact]
        public void TellWaiter_TellMessage_StartsWaitingForEvents()
        {
            //arrange
            TellWaiter sut = CreateTellWaiter();

            //act
            sut.TellMessage(Waiter, this, Recipient, Message, ExpectedEventCount, Sender);

            //assert
            WaiterMock.Verify(
                waiter => waiter.Start(this, ExpectedEventCount),
                Times.Once);
        }

        [Fact]
        public void TellWaiter_TellMessageWithNoSender_StartsWaitingForEvents()
        {
            //arrange
            TellWaiter sut = CreateTellWaiter();

            //act
            sut.TellMessage(Waiter, this, Recipient, Message, ExpectedEventCount);

            //assert
            WaiterMock.Verify(
                waiter => waiter.Start(this, ExpectedEventCount),
                Times.Once);
        }

        [Fact]
        public void TellWaiter_TellMessage_WaitsForEvents()
        {
            //arrange
            TellWaiter sut = CreateTellWaiter();

            //act
            sut.TellMessage(Waiter, this, Recipient, Message, ExpectedEventCount, Sender);

            //assert
            WaiterMock.Verify(
                waiter => waiter.Wait(),
                Times.Once);
        }

        [Fact]
        public void TellWaiter_TellMessageWithNoSender_WaitsForEvents()
        {
            //arrange
            TellWaiter sut = CreateTellWaiter();

            //act
            sut.TellMessage(Waiter, this, Recipient, Message, ExpectedEventCount);

            //assert
            WaiterMock.Verify(
                waiter => waiter.Wait(),
                Times.Once);
        }

        [Fact]
        public void TellWaiter_TellMessage_TellsRecipient()
        {
            //arrange
            TellWaiter sut = CreateTellWaiter();

            //act
            sut.TellMessage(Waiter, this, Recipient, Message, ExpectedEventCount, Sender);

            //assert
            RecipientMock.Verify(actorRef => actorRef.Tell(Message, Sender));
        }

        [Fact]
        public void TellWaiter_TellMessageWithNoSender_TellsRecipient()
        {
            //arrange
            TellWaiter sut = CreateTellWaiter();

            //act
            sut.TellMessage(Waiter, this, Recipient, Message, ExpectedEventCount);

            //assert
            RecipientMock.Verify(actorRef => actorRef.Tell(Message, TestActor));
        }

        [Fact]
        public void TellWaiter_TellMessage_StartsWaitingForEventBeforeTellingRecipient()
        {
            //arrange
            TellWaiter sut = CreateTellWaiter();

            //act
            sut.TellMessage(Waiter, this, Recipient, Message, ExpectedEventCount, Sender);

            //assert
            CallOrder.Should().ContainInOrder(
                nameof(IWaiter.Start),
                nameof(IActorRef.Tell) + "Sender");
        }

        [Fact]
        public void TellWaiter_TellMessageWithNoSender_StartsWaitingForEventBeforeTellingRecipient()
        {
            //arrange
            TellWaiter sut = CreateTellWaiter();

            //act
            sut.TellMessage(Waiter, this, Recipient, Message, ExpectedEventCount);

            //assert
            CallOrder.Should().ContainInOrder(
                nameof(IWaiter.Start),
                nameof(IActorRef.Tell));
        }

        [Fact]
        public void TellWaiter_TellMessage_WaitsForEventAfterTellingRecipient()
        {
            //arrange
            TellWaiter sut = CreateTellWaiter();

            //act
            sut.TellMessage(Waiter, this, Recipient, Message, ExpectedEventCount, Sender);

            //assert
            CallOrder.Should().ContainInOrder(
                nameof(IActorRef.Tell) + "Sender",
                nameof(IWaiter.Wait));
        }

        [Fact]
        public void TellWaiter_TellMessageWithNoSender_WaitsForEventAfterTellingRecipient()
        {
            //arrange
            TellWaiter sut = CreateTellWaiter();

            //act
            sut.TellMessage(Waiter, this, Recipient, Message, ExpectedEventCount);

            //assert
            CallOrder.Should().ContainInOrder(
                nameof(IActorRef.Tell),
                nameof(IWaiter.Wait));
        }
    }
}