using System;
using FluentAssertions;
using Moq;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.ConcreteResolverTests
{
    public class TellMessage : TestBase
    {
        #region Null tests
        [Fact]
        public void ConcreteResolver_TellMessageNoSenderWithNullRecipient_ThrowsArgumentNullException()
        {
            //arrange
            ConcreteResolver sut = CreateConcreteResolver();

            //act
            Action act = () => sut.TellMessage(null, Message, ExpectedChildCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void ConcreteResolver_TellMessageNoSenderWithNullMessage_ThrowsArgumentNullException()
        {
            //arrange
            ConcreteResolver sut = CreateConcreteResolver();

            //act
            Action act = () => sut.TellMessage<object>(Recipient, null, ExpectedChildCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }
        
        [Fact]
        public void ConcreteResolver_TellMessageNoSenderWithNullRecipientAndMessage_ThrowsArgumentNullException()
        {
            //arrange
            ConcreteResolver sut = CreateConcreteResolver();

            //act
            Action act = () => sut.TellMessage<object>(null, null, ExpectedChildCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void ConcreteResolver_TellMessageFromSenderWithNullRecipient_ThrowsArgumentNullException()
        {
            //arrange
            ConcreteResolver sut = CreateConcreteResolver();

            //act
            Action act = () => sut.TellMessage(null, Message, Sender, ExpectedChildCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void ConcreteResolver_TellMessageFromSenderWithNullMessage_ThrowsArgumentNullException()
        {
            //arrange
            ConcreteResolver sut = CreateConcreteResolver();

            //act
            Action act = () => sut.TellMessage<object>(Recipient, null, Sender, ExpectedChildCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void ConcreteResolver_TellMessageFromSenderWithNullSender_ThrowsArgumentNullException()
        {
            //arrange
            ConcreteResolver sut = CreateConcreteResolver();

            //act
            Action act = () => sut.TellMessage(Recipient, Message, null, ExpectedChildCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void ConcreteResolver_TellMessageNoSenderWithNullRecipientAndMessageAndSender_ThrowsArgumentNullException()
        {
            //arrange
            ConcreteResolver sut = CreateConcreteResolver();

            //act
            Action act = () => sut.TellMessage<object>(null, null, null, ExpectedChildCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void ConcreteResolver_TellMessageSenderWithNullSender_ThrowsArgumentNullException()
        {
            //arrange
            ConcreteResolver sut = CreateConcreteResolver();

            //act
            Action act = () => sut.TellMessage(Recipient, Message, null, ExpectedChildCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void ConcreteResolver_TellMessageWithNullRecipientAndMessageAndSender_ThrowsArgumentNullException()
        {
            //arrange
            ConcreteResolver sut = CreateConcreteResolver();

            //act
            Action act = () => sut.TellMessage<object>(null, null, null, ExpectedChildCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }
        #endregion

        [Fact]
        public void ConcreteResolver_TellMessageNoSender_TellsChild()
        {
            //arrange
            ConcreteResolver sut = CreateConcreteResolver();

            //act
            sut.TellMessage(Recipient, Message, ExpectedChildCount);

            //assert
            ChildTellerMock.Verify(
                teller => teller.TellMessage(
                    ChildWaiter, 
                    this, 
                    Recipient, 
                    Message, 
                    ExpectedChildCount, 
                    null),
                Times.Once);
        }
        
        [Fact]
        public void ConcreteResolver_TellMessageSender_TellsChild()
        {
            //arrange
            ConcreteResolver sut = CreateConcreteResolver();

            //act
            sut.TellMessage(Recipient, Message, Sender, ExpectedChildCount);

            //assert
            ChildTellerMock.Verify(
                teller => teller.TellMessage(
                    ChildWaiter, 
                    this, 
                    Recipient, 
                    Message, 
                    ExpectedChildCount, 
                    Sender),
                Times.Once);
        }
    }
}