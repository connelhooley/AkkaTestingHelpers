using System.Collections.Generic;
using Akka.Actor;
using Akka.TestKit.NUnit3;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;
using Moq;
using NUnit.Framework;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.ChildTellerTests
{
    internal class TestBase : TestKit
    {
        protected Mock<IChildWaiter> ChildWaiterMock;
        protected Mock<IActorRef> RecipientMock;

        protected List<string> CallOrder;

        protected IChildWaiter ChildWaiter;
        protected int ExpectedChildrenCount;
        protected object Message;
        protected IActorRef Recipient;
        protected IActorRef Sender;

        public TestBase(): base(AkkaConfig.Config) { }

        [SetUp]
        public void Setup()
        {
            // Create mocks
            ChildWaiterMock = new Mock<IChildWaiter>();
            RecipientMock = new Mock<IActorRef>();

            // Create objects used by mocks
            CallOrder = new List<string>();

            // Create objects passed into sut
            ChildWaiter = ChildWaiterMock.Object;
            ExpectedChildrenCount = TestUtils.Create<int>();
            Message = TestUtils.Create<object>();
            Recipient = RecipientMock.Object;
            Sender = CreateTestProbe();

            // Set up mocks
            ChildWaiterMock
                .Setup(waiter => waiter.Start(this, ExpectedChildrenCount))
                .Callback(() => CallOrder.Add(nameof(IChildWaiter.Start)));
            ChildWaiterMock
                .Setup(waiter => waiter.Wait())
                .Callback(() => CallOrder.Add(nameof(IChildWaiter.Wait)));
            ChildWaiterMock
                .Setup(waiter => waiter.ResolvedChild())
                .Callback(() => CallOrder.Add(nameof(IChildWaiter.ResolvedChild)));

            RecipientMock
                .Setup(waiter => waiter.Tell(Message, TestActor))
                .Callback(() => CallOrder.Add(nameof(IActorRef.Tell)));
            RecipientMock
                .Setup(waiter => waiter.Tell(Message, Sender))
                .Callback(() => CallOrder.Add(nameof(IActorRef.Tell) + "Sender"));
        }

        [TearDown]
        public void TearDown()
        {
            ChildWaiterMock = null;
            RecipientMock = null;

            CallOrder = null;

            ChildWaiter = null;
            ExpectedChildrenCount = default(int);
            Message = null;
            Recipient = null;
            Sender = null;
        }

        protected ChildTeller CreateChildTeller() => new ChildTeller();
    }
}