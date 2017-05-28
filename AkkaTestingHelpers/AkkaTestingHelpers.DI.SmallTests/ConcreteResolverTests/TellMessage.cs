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