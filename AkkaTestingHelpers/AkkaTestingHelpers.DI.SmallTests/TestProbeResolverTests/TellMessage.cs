using System;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.TestProbeResolverTests
{
    internal class TellMessage : TestBase
    {
        [Test]
        public void TestProbeResolver_TellMessageNoSenderWithNullRecipient_ThrowsArgumentNullException()
        {
            //arrange
            TestProbeResolver sut = CreateTestProbeResolver(TestProbeResolverSettings.Empty);

            //act
            Action act = () => sut.TellMessage(null, Message, ExpectedChildrenCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void TestProbeResolver_TellMessageNoSenderWithNullMessage_ThrowsArgumentNullException()
        {
            //arrange
            TestProbeResolver sut = CreateTestProbeResolver(TestProbeResolverSettings.Empty);

            //act
            Action act = () => sut.TellMessage<object>(Recipient, null, ExpectedChildrenCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void TestProbeResolver_TellMessageNoSenderWithNullRecipientAndMessage_ThrowsArgumentNullException()
        {
            //arrange
            TestProbeResolver sut = CreateTestProbeResolver(TestProbeResolverSettings.Empty);

            //act
            Action act = () => sut.TellMessage<object>(null, null, ExpectedChildrenCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void TestProbeResolver_TellMessageFromSenderWithNullRecipient_ThrowsArgumentNullException()
        {
            //arrange
            TestProbeResolver sut = CreateTestProbeResolver(TestProbeResolverSettings.Empty);

            //act
            Action act = () => sut.TellMessage(null, Message, Sender, ExpectedChildrenCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void TestProbeResolver_TellMessageFromSenderWithNullMessage_ThrowsArgumentNullException()
        {
            //arrange
            TestProbeResolver sut = CreateTestProbeResolver(TestProbeResolverSettings.Empty);

            //act
            Action act = () => sut.TellMessage<object>(Recipient, null, Sender, ExpectedChildrenCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void TestProbeResolver_TellMessageFromSenderWithNullSender_ThrowsArgumentNullException()
        {
            //arrange
            TestProbeResolver sut = CreateTestProbeResolver(TestProbeResolverSettings.Empty);

            //act
            Action act = () => sut.TellMessage(Recipient, Message, null, ExpectedChildrenCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void TestProbeResolver_TellMessageNoSenderWithNullRecipientAndMessageAndSender_ThrowsArgumentNullException()
        {
            //arrange
            TestProbeResolver sut = CreateTestProbeResolver(TestProbeResolverSettings.Empty);

            //act
            Action act = () => sut.TellMessage<object>(null, null, null, ExpectedChildrenCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void TestProbeResolver_TellMessageSenderWithNullSender_ThrowsArgumentNullException()
        {
            //arrange
            TestProbeResolver sut = CreateTestProbeResolver(TestProbeResolverSettings.Empty);

            //act
            Action act = () => sut.TellMessage(Recipient, Message, null, ExpectedChildrenCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void TestProbeResolver_TellMessageWithNullRecipientAndMessageAndSender_ThrowsArgumentNullException()
        {
            //arrange
            TestProbeResolver sut = CreateTestProbeResolver(TestProbeResolverSettings.Empty);

            //act
            Action act = () => sut.TellMessage<object>(null, null, null, ExpectedChildrenCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }
        
        [Test]
        public void TestProbeResolver_TellMessageNoSender_TellsChild()
        {
            //arrange
            TestProbeResolver sut = CreateTestProbeResolver(TestProbeResolverSettings.Empty);

            //act
            sut.TellMessage(Recipient, Message, ExpectedChildrenCount);

            //assert
            ChildTellerMock.Verify(
                teller => teller.TellMessage(ChildWaiterMock.Object, this, Recipient, Message, ExpectedChildrenCount, null),
                Times.Once);
        }

        [Test]
        public void TestProbeResolver_TellMessageSender_TellsChild()
        {
            //arrange
            TestProbeResolver sut = CreateTestProbeResolver(TestProbeResolverSettings.Empty);

            //act
            sut.TellMessage(Recipient, Message, Sender, ExpectedChildrenCount);

            //assert
            ChildTellerMock.Verify(
                teller => teller.TellMessage(ChildWaiterMock.Object, this, Recipient, Message, ExpectedChildrenCount, Sender),
                Times.Once);
        }
    }
}