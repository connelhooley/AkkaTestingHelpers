using System;
using FluentAssertions;
using Moq;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.TestProbeResolverTests
{
    public class TellMessage : TestBase
    {
        #region Null tests
        [Fact]
        public void TestProbeResolver_TellMessageNoSenderWithNullRecipient_ThrowsArgumentNullException()
        {
            //arrange
            TestProbeResolver sut = CreateTestProbeResolver();

            //act
            Action act = () => sut.TellMessage(
                null, 
                Message, 
                ExpectedChildCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeResolver_TellMessageNoSenderWithNullMessage_ThrowsArgumentNullException()
        {
            //arrange
            TestProbeResolver sut = CreateTestProbeResolver();

            //act
            Action act = () => sut.TellMessage<object>(
                Recipient, 
                null, 
                ExpectedChildCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeResolver_TellMessageNoSenderWithNullRecipientAndMessage_ThrowsArgumentNullException()
        {
            //arrange
            TestProbeResolver sut = CreateTestProbeResolver();

            //act
            Action act = () => sut.TellMessage<object>(
                null, 
                null, 
                ExpectedChildCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeResolver_TellMessageFromSenderWithNullRecipient_ThrowsArgumentNullException()
        {
            //arrange
            TestProbeResolver sut = CreateTestProbeResolver();

            //act
            Action act = () => sut.TellMessage(
                null, 
                Message, 
                Sender, 
                ExpectedChildCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeResolver_TellMessageFromSenderWithNullMessage_ThrowsArgumentNullException()
        {
            //arrange
            TestProbeResolver sut = CreateTestProbeResolver();

            //act
            Action act = () => sut.TellMessage<object>(
                Recipient, 
                null, 
                Sender, 
                ExpectedChildCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeResolver_TellMessageFromSenderWithNullSender_ThrowsArgumentNullException()
        {
            //arrange
            TestProbeResolver sut = CreateTestProbeResolver();

            //act
            Action act = () => sut.TellMessage(
                Recipient, 
                Message, 
                null, 
                ExpectedChildCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeResolver_TellMessageFromSenderWithNullRecipientAndMessageAndSender_ThrowsArgumentNullException()
        {
            //arrange
            TestProbeResolver sut = CreateTestProbeResolver();

            //act
            Action act = () => sut.TellMessage<object>(
                null, 
                null, 
                null, 
                ExpectedChildCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }
        #endregion

        [Fact]
        public void TestProbeResolver_TellMessageNoSender_TellsChild()
        {
            //arrange
            TestProbeResolver sut = CreateTestProbeResolver();

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
        public void TestProbeResolver_TellMessageSender_TellsChild()
        {
            //arrange
            TestProbeResolver sut = CreateTestProbeResolver();

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