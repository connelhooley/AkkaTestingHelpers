using Akka.Actor;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.TestProbeResolverTests
{
    internal class TellMessage : TestBase
    {
        [Test]
        public void TestProbeResolver_TellMessage_StartsWaitingForChildren()
        {
            //arrange
            TestProbeResolver sut = CreateTestProbeResolver(TestProbeResolverSettings.Empty);

            //act
            sut.TellMessage(Recipient, Message, ExpectedChildrenCount);

            //assert
            ChildWaiterMock.Verify(
                waiter => waiter.Start(this, ExpectedChildrenCount),
                Times.Once);
        }

        [Test]
        public void TestProbeResolver_TellMessage_WaitsForChildren()
        {
            //arrange
            TestProbeResolver sut = CreateTestProbeResolver(TestProbeResolverSettings.Empty);

            //act
            sut.TellMessage(Recipient, Message, ExpectedChildrenCount);

            //assert
            ChildWaiterMock.Verify(
                waiter => waiter.Wait(),
                Times.Once);
        }

        [Test]
        public void TestProbeResolver_TellMessage_TellsRecipient()
        {
            //arrange
            TestProbeResolver sut = CreateTestProbeResolver(TestProbeResolverSettings.Empty);

            //act
            sut.TellMessage(Recipient, Message, ExpectedChildrenCount);

            //assert
            RecipientMock.Verify(actorRef => actorRef.Tell(Message, TestActor));
        }

        [Test]
        public void TestProbeResolver_TellMessage_StartsWaitingForChildrenBeforeTellingRecipient()
        {
            //arrange
            TestProbeResolver sut = CreateTestProbeResolver(TestProbeResolverSettings.Empty);

            //act
            sut.TellMessage(Recipient, Message, ExpectedChildrenCount);

            //assert
            CallOrder.Should().ContainInOrder(nameof(IChildWaiter.Start), nameof(IActorRef.Tell));
        }

        [Test]
        public void TestProbeResolver_TellMessage_WaitsForChildrenAfterTellingRecipient()
        {
            //arrange
            TestProbeResolver sut = CreateTestProbeResolver(TestProbeResolverSettings.Empty);

            //act
            sut.TellMessage(Recipient, Message, ExpectedChildrenCount);

            //assert
            CallOrder.Should().ContainInOrder(nameof(IActorRef.Tell), nameof(IChildWaiter.Wait));
        }
        
        [Test]
        public void TestProbeResolver_TellMessageFromSender_StartsWaitingForChildren()
        {
            //arrange
            TestProbeResolver sut = CreateTestProbeResolver(TestProbeResolverSettings.Empty);

            //act
            sut.TellMessage(Recipient, Message, CreatedActor, ExpectedChildrenCount);

            //assert
            ChildWaiterMock.Verify(
                waiter => waiter.Start(this, ExpectedChildrenCount),
                Times.Once);
        }

        [Test]
        public void TestProbeResolver_TellMessageFromSender_WaitsForChildren()
        {
            //arrange
            TestProbeResolver sut = CreateTestProbeResolver(TestProbeResolverSettings.Empty);

            //act
            sut.TellMessage(Recipient, Message, CreatedActor, ExpectedChildrenCount);

            //assert
            ChildWaiterMock.Verify(
                waiter => waiter.Wait(),
                Times.Once);
        }

        [Test]
        public void TestProbeResolver_TellMessageFromSender_TellsRecipient()
        {
            //arrange
            TestProbeResolver sut = CreateTestProbeResolver(TestProbeResolverSettings.Empty);

            //act
            sut.TellMessage(Recipient, Message, CreatedActor, ExpectedChildrenCount);

            //assert
            RecipientMock.Verify(actorRef => actorRef.Tell(Message, CreatedActor));
        }

        [Test]
        public void TestProbeResolver_TellMessageFromSender_StartsWaitingForChildrenBeforeTellingRecipient()
        {
            //arrange
            TestProbeResolver sut = CreateTestProbeResolver(TestProbeResolverSettings.Empty);

            //act
            sut.TellMessage(Recipient, Message, CreatedActor, ExpectedChildrenCount);

            //assert
            CallOrder.Should().ContainInOrder(nameof(IChildWaiter.Start), nameof(IActorRef.Tell) + "Sender");
        }

        [Test]
        public void TestProbeResolver_TellMessageFromSender_WaitsForChildrenAfterTellingRecipient()
        {
            //arrange
            TestProbeResolver sut = CreateTestProbeResolver(TestProbeResolverSettings.Empty);

            //act
            sut.TellMessage(Recipient, Message, CreatedActor, ExpectedChildrenCount);

            //assert
            CallOrder.Should().ContainInOrder(nameof(IActorRef.Tell) + "Sender", nameof(IChildWaiter.Wait));
        }
    }
}