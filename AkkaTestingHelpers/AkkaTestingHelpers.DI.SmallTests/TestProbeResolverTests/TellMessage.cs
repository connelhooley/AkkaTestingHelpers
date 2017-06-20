using System;
using FluentAssertions;
using Moq;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.TestProbeResolverTests
{
    internal class TellMessage : TestBase
    {
        [Fact]
        public void TestProbeResolver_TellMessageNoSenderWithNullRecipient_ThrowsArgumentNullException()
        {
            //arrange
            TestProbeResolver sut = CreateTestProbeResolver();

            //act
            Action act = () => sut.TellMessage(null, Message, ExpectedChildrenCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeResolver_TellMessageNoSenderWithNullMessage_ThrowsArgumentNullException()
        {
            //arrange
            TestProbeResolver sut = CreateTestProbeResolver();

            //act
            Action act = () => sut.TellMessage<object>(Recipient, null, ExpectedChildrenCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeResolver_TellMessageNoSenderWithNullRecipientAndMessage_ThrowsArgumentNullException()
        {
            //arrange
            TestProbeResolver sut = CreateTestProbeResolver();

            //act
            Action act = () => sut.TellMessage<object>(null, null, ExpectedChildrenCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeResolver_TellMessageFromSenderWithNullRecipient_ThrowsArgumentNullException()
        {
            //arrange
            TestProbeResolver sut = CreateTestProbeResolver();

            //act
            Action act = () => sut.TellMessage(null, Message, Sender, ExpectedChildrenCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeResolver_TellMessageFromSenderWithNullMessage_ThrowsArgumentNullException()
        {
            //arrange
            TestProbeResolver sut = CreateTestProbeResolver();

            //act
            Action act = () => sut.TellMessage<object>(Recipient, null, Sender, ExpectedChildrenCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeResolver_TellMessageFromSenderWithNullSender_ThrowsArgumentNullException()
        {
            //arrange
            TestProbeResolver sut = CreateTestProbeResolver();

            //act
            Action act = () => sut.TellMessage(Recipient, Message, null, ExpectedChildrenCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeResolver_TellMessageFromSenderWithNullRecipientAndMessageAndSender_ThrowsArgumentNullException()
        {
            //arrange
            TestProbeResolver sut = CreateTestProbeResolver();

            //act
            Action act = () => sut.TellMessage<object>(null, null, null, ExpectedChildrenCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }
        
        [Fact]
        public void TestProbeResolver_TellMessageNoSender_TellsChild()
        {
            //arrange
            TestProbeResolver sut = CreateTestProbeResolver();

            //act
            sut.TellMessage(Recipient, Message, ExpectedChildrenCount);

            //assert
            ChildTellerMock.Verify(
                teller => teller.TellMessage(ChildWaiterMock.Object, this, Recipient, Message, ExpectedChildrenCount, null),
                Times.Once);
        }

        [Fact]
        public void TestProbeResolver_TellMessageSender_TellsChild()
        {
            //arrange
            TestProbeResolver sut = CreateTestProbeResolver();

            //act
            sut.TellMessage(Recipient, Message, Sender, ExpectedChildrenCount);

            //assert
            ChildTellerMock.Verify(
                teller => teller.TellMessage(ChildWaiterMock.Object, this, Recipient, Message, ExpectedChildrenCount, Sender),
                Times.Once);
        }
    }
}