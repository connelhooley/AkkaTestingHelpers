using System;
using Akka.Actor;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.ConcreteResolverTests
{
    internal class TellMessage : TestBase
    {
        [Test]
        public void ConcreteResolver_TellMessageWithNullRecipient_ThrowsArgumentNullException()
        {
            //arrange
            ConcreteResolver sut = CreateConcreteResolver(ConcreteResolverSettings.Empty);

            //act
            Action act = () => sut.TellMessage(null, Message, ExpectedChildrenCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void ConcreteResolver_TellMessageWithNullMessage_ThrowsArgumentNullException()
        {
            //arrange
            ConcreteResolver sut = CreateConcreteResolver(ConcreteResolverSettings.Empty);

            //act
            Action act = () => sut.TellMessage<object>(Recipient, null, ExpectedChildrenCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }
        
        [Test]
        public void ConcreteResolver_TellMessageWithNullRecipientAndMessage_ThrowsArgumentNullException()
        {
            //arrange
            ConcreteResolver sut = CreateConcreteResolver(ConcreteResolverSettings.Empty);

            //act
            Action act = () => sut.TellMessage<object>(null, null, ExpectedChildrenCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void ConcreteResolver_TellMessageWithNullSender_ThrowsArgumentNullException()
        {
            //arrange
            ConcreteResolver sut = CreateConcreteResolver(ConcreteResolverSettings.Empty);

            //act
            Action act = () => sut.TellMessage(Recipient, Message, null, ExpectedChildrenCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void ConcreteResolver_TellMessageWithNullRecipientAndMessageAndSender_ThrowsArgumentNullException()
        {
            //arrange
            ConcreteResolver sut = CreateConcreteResolver(ConcreteResolverSettings.Empty);

            //act
            Action act = () => sut.TellMessage<object>(null, null, null, ExpectedChildrenCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void ConcreteResolver_TellMessage_StartsWaitingForChildren()
        {
            //arrange
            ConcreteResolver sut = CreateConcreteResolver(ConcreteResolverSettings.Empty);

            //act
            sut.TellMessage(Recipient, Message, ExpectedChildrenCount);

            //assert
            ChildWaiterMock.Verify(
                waiter => waiter.Start(this, ExpectedChildrenCount),
                Times.Once);
        }

        [Test]
        public void ConcreteResolver_TellMessage_WaitsForChildren()
        {
            //arrange
            ConcreteResolver sut = CreateConcreteResolver(ConcreteResolverSettings.Empty);

            //act
            sut.TellMessage(Recipient, Message, ExpectedChildrenCount);

            //assert
            ChildWaiterMock.Verify(
                waiter => waiter.Wait(),
                Times.Once);
        }

        [Test]
        public void ConcreteResolver_TellMessage_TellsRecipient()
        {
            //arrange
            ConcreteResolver sut = CreateConcreteResolver(ConcreteResolverSettings.Empty);

            //act
            sut.TellMessage(Recipient, Message, ExpectedChildrenCount);

            //assert
            RecipientMock.Verify(actorRef => actorRef.Tell(Message, TestActor));
        }

        [Test]
        public void ConcreteResolver_TellMessage_StartsWaitingForChildrenBeforeTellingRecipient()
        {
            //arrange
            ConcreteResolver sut = CreateConcreteResolver(ConcreteResolverSettings.Empty);

            //act
            sut.TellMessage(Recipient, Message, ExpectedChildrenCount);

            //assert
            CallOrder.Should().ContainInOrder(nameof(IChildWaiter.Start), nameof(IActorRef.Tell));
        }

        [Test]
        public void ConcreteResolver_TellMessage_WaitsForChildrenAfterTellingRecipient()
        {
            //arrange
            ConcreteResolver sut = CreateConcreteResolver(ConcreteResolverSettings.Empty);

            //act
            sut.TellMessage(Recipient, Message, ExpectedChildrenCount);

            //assert
            CallOrder.Should().ContainInOrder(nameof(IActorRef.Tell), nameof(IChildWaiter.Wait));
        }
        
        [Test]
        public void ConcreteResolver_TellMessageFromSender_StartsWaitingForChildren()
        {
            //arrange
            ConcreteResolver sut = CreateConcreteResolver(ConcreteResolverSettings.Empty);

            //act
            sut.TellMessage(Recipient, Message, CreatedActor, ExpectedChildrenCount);

            //assert
            ChildWaiterMock.Verify(
                waiter => waiter.Start(this, ExpectedChildrenCount),
                Times.Once);
        }

        [Test]
        public void ConcreteResolver_TellMessageFromSender_WaitsForChildren()
        {
            //arrange
            ConcreteResolver sut = CreateConcreteResolver(ConcreteResolverSettings.Empty);

            //act
            sut.TellMessage(Recipient, Message, CreatedActor, ExpectedChildrenCount);

            //assert
            ChildWaiterMock.Verify(
                waiter => waiter.Wait(),
                Times.Once);
        }

        [Test]
        public void ConcreteResolver_TellMessageFromSender_TellsRecipient()
        {
            //arrange
            ConcreteResolver sut = CreateConcreteResolver(ConcreteResolverSettings.Empty);

            //act
            sut.TellMessage(Recipient, Message, CreatedActor, ExpectedChildrenCount);

            //assert
            RecipientMock.Verify(actorRef => actorRef.Tell(Message, CreatedActor));
        }

        [Test]
        public void ConcreteResolver_TellMessageFromSender_StartsWaitingForChildrenBeforeTellingRecipient()
        {
            //arrange
            ConcreteResolver sut = CreateConcreteResolver(ConcreteResolverSettings.Empty);

            //act
            sut.TellMessage(Recipient, Message, CreatedActor, ExpectedChildrenCount);

            //assert
            CallOrder.Should().ContainInOrder(nameof(IChildWaiter.Start), nameof(IActorRef.Tell) + "Sender");
        }

        [Test]
        public void ConcreteResolver_TellMessageFromSender_WaitsForChildrenAfterTellingRecipient()
        {
            //arrange
            ConcreteResolver sut = CreateConcreteResolver(ConcreteResolverSettings.Empty);

            //act
            sut.TellMessage(Recipient, Message, CreatedActor, ExpectedChildrenCount);

            //assert
            CallOrder.Should().ContainInOrder(nameof(IActorRef.Tell) + "Sender", nameof(IChildWaiter.Wait));
        }
    }
}