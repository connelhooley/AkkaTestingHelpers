using System;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.ConcreteResolverTests
{
    internal class TellMessage : TestBase
    {
        #region null checks

        [Test]
        public void ConcreteResolver_TellMessageNoSenderWithNullRecipient_ThrowsArgumentNullException()
        {
            //arrange
            ConcreteResolver sut = CreateConcreteResolver(ConcreteResolverSettings.Empty);

            //act
            Action act = () => sut.TellMessage(null, Message, ExpectedChildrenCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void ConcreteResolver_TellMessageNoSenderWithNullMessage_ThrowsArgumentNullException()
        {
            //arrange
            ConcreteResolver sut = CreateConcreteResolver(ConcreteResolverSettings.Empty);

            //act
            Action act = () => sut.TellMessage<object>(Recipient, null, ExpectedChildrenCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }
        
        [Test]
        public void ConcreteResolver_TellMessageNoSenderWithNullRecipientAndMessage_ThrowsArgumentNullException()
        {
            //arrange
            ConcreteResolver sut = CreateConcreteResolver(ConcreteResolverSettings.Empty);

            //act
            Action act = () => sut.TellMessage<object>(null, null, ExpectedChildrenCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void ConcreteResolver_TellMessageFromSenderWithNullRecipient_ThrowsArgumentNullException()
        {
            //arrange
            ConcreteResolver sut = CreateConcreteResolver(ConcreteResolverSettings.Empty);

            //act
            Action act = () => sut.TellMessage(null, Message, Sender, ExpectedChildrenCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void ConcreteResolver_TellMessageFromSenderWithNullMessage_ThrowsArgumentNullException()
        {
            //arrange
            ConcreteResolver sut = CreateConcreteResolver(ConcreteResolverSettings.Empty);

            //act
            Action act = () => sut.TellMessage<object>(Recipient, null, Sender, ExpectedChildrenCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void ConcreteResolver_TellMessageFromSenderWithNullSender_ThrowsArgumentNullException()
        {
            //arrange
            ConcreteResolver sut = CreateConcreteResolver(ConcreteResolverSettings.Empty);

            //act
            Action act = () => sut.TellMessage(Recipient, Message, null, ExpectedChildrenCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void ConcreteResolver_TellMessageNoSenderWithNullRecipientAndMessageAndSender_ThrowsArgumentNullException()
        {
            //arrange
            ConcreteResolver sut = CreateConcreteResolver(ConcreteResolverSettings.Empty);

            //act
            Action act = () => sut.TellMessage<object>(null, null, null, ExpectedChildrenCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void ConcreteResolver_TellMessageSenderWithNullSender_ThrowsArgumentNullException()
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

        #endregion

        [Test]
        public void ConcreteResolver_TellMessageNoSender_TellsChild()
        {
            //arrange
            ConcreteResolver sut = CreateConcreteResolver(ConcreteResolverSettings.Empty);

            //act
            sut.TellMessage(Recipient, Message, ExpectedChildrenCount);

            //assert
            ChildTellerMock.Verify(
                teller => teller.TellMessage(ChildWaiterMock.Object, this, Recipient, Message, ExpectedChildrenCount, null),
                Times.Once);
        }
        
        [Test]
        public void ConcreteResolver_TellMessageSender_TellsChild()
        {
            //arrange
            ConcreteResolver sut = CreateConcreteResolver(ConcreteResolverSettings.Empty);

            //act
            sut.TellMessage(Recipient, Message, Sender, ExpectedChildrenCount);

            //assert
            ChildTellerMock.Verify(
                teller => teller.TellMessage(ChildWaiterMock.Object, this, Recipient, Message, ExpectedChildrenCount, Sender),
                Times.Once);
        }
    }
}