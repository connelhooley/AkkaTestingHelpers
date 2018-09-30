using System.Collections.Generic;
using Akka.Actor;
using Akka.TestKit.Xunit2;
using ConnelHooley.AkkaTestingHelpers.Helpers.Abstract;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;
using ConnelHooley.TestHelpers;
using Moq;

// ReSharper disable VirtualMemberCallInConstructor

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.TellWaiterTests
{
    public class TestBase : TestKit
    {
        internal Mock<IWaiter> WaiterMock;
        internal Mock<IActorRef> RecipientMock;

        internal List<string> CallOrder;

        internal IWaiter Waiter;
        internal int ExpectedEventCount;
        internal object Message;
        internal IActorRef Recipient;
        internal IActorRef Sender;

        public TestBase() : base(AkkaConfig.Config)
        {
            // Create mocks
            WaiterMock = new Mock<IWaiter>();
            RecipientMock = new Mock<IActorRef>();

            // Create objects used by mocks
            CallOrder = new List<string>();

            // Create objects passed into sut methods
            Waiter = WaiterMock.Object;
            ExpectedEventCount = TestHelper.GenerateNumber();
            Message = TestHelper.Generate<object>();
            Recipient = RecipientMock.Object;
            Sender = CreateTestProbe(); // Mocking sender breaks Akka so a TestProbe is used

            // Set up mocks
            WaiterMock
                .Setup(waiter => waiter.Start(this, ExpectedEventCount))
                .Callback(() => CallOrder.Add(nameof(IWaiter.Start)));
            WaiterMock
                .Setup(waiter => waiter.Wait())
                .Callback(() => CallOrder.Add(nameof(IWaiter.Wait)));
            WaiterMock
                .Setup(waiter => waiter.ResolveEvent())
                .Callback(() => CallOrder.Add(nameof(IWaiter.ResolveEvent)));

            RecipientMock
                .Setup(waiter => waiter.Tell(Message, TestActor))
                .Callback(() => CallOrder.Add(nameof(IActorRef.Tell)));
            RecipientMock
                .Setup(waiter => waiter.Tell(Message, Sender))
                .Callback(() => CallOrder.Add(nameof(IActorRef.Tell) + "Sender"));
        }
        
        internal TellWaiter CreateTellWaiter() => new TellWaiter();
    }
}