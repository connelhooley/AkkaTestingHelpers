using System;
using System.Collections.Generic;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.NUnit3;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;
using Moq;
using NUnit.Framework;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.SutCreatorTests
{
    internal class TestBase : TestKit
    {
        protected Mock<IChildWaiter> ChildWaiterMock;
        protected IChildWaiter ChildWaiter;
        protected Props Props;
        protected int ExpectedChildCount;
        protected IActorRef Supervisor;
        protected List<string> CallOrder;

        [SetUp]
        public void SetUp()
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
        
        [TearDown]
        public void TearDown()
        {
            ChildWaiterMock = null;
            ChildWaiter = null;
            CallOrder = null;
            Props = null;
            ExpectedChildCount = default(int);
            Supervisor = null;
        }

        protected SutCreator CreateSutCreator() => new SutCreator();

        protected class DummyActor: ReceiveActor
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