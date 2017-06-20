using System;
using System.Collections.Generic;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.Xunit2;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;
using Moq;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.SutCreatorTests
{
    public class TestBase : TestKit
    {
        internal Mock<IChildWaiter> ChildWaiterMock;
        internal IChildWaiter ChildWaiter;
        internal Props Props;
        internal int ExpectedChildCount;
        internal IActorRef Supervisor;
        internal List<string> CallOrder;

        public TestBase() : base(AkkaConfig.Config)
        {
            CallOrder = new List<string>();
            ChildWaiterMock = new Mock<IChildWaiter>();
            ChildWaiterMock
                .Setup(waiter => waiter.Start(It.IsAny<TestKitBase>(), It.IsAny<int>()))
                .Callback(() => CallOrder.Add(nameof(IChildWaiter.Start)));
            ChildWaiterMock
                .Setup(waiter => waiter.Wait())
                .Callback(() => CallOrder.Add(nameof(IChildWaiter.Wait)));
            ChildWaiter = ChildWaiterMock.Object;
            Action updateChildCreated = () => CallOrder.Add("callback");
            Props = Props.Create(() => new DummyActor(updateChildCreated));
            Supervisor = CreateTestProbe();
            ExpectedChildCount = TestUtils.Create<int>();
        }

        internal SutCreator CreateSutCreator() => new SutCreator();

        public class DummyActor: ReceiveActor
        {
            public IActorRef Supervisor { get; }

            public DummyActor(Action callback)
            {
                Supervisor = Context.Parent;
                callback?.Invoke();
            }
        }
    }
}