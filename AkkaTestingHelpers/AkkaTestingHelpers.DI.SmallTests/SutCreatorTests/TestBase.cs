using System;
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
        protected Props SutProps;
        protected int ExpectedChildCount;
        protected IActorRef Supervisor;
        protected DateTime WhenChildWaiterStart;
        protected DateTime WhenChildCreated;
        protected DateTime WhenChildWaiterWait;

        [SetUp]
        public void SetUp()
        {
            ChildWaiterMock = new Mock<IChildWaiter>();
            ChildWaiterMock
                .Setup(waiter => waiter.Start(It.IsAny<TestKitBase>(), It.IsAny<int>()))
                .Callback(() => WhenChildWaiterStart = DateTime.UtcNow);
            ChildWaiterMock
                .Setup(waiter => waiter.Wait())
                .Callback(() => WhenChildWaiterWait = DateTime.UtcNow);
            ChildWaiter = ChildWaiterMock.Object;
            Action updateChildCreated = () => WhenChildCreated = DateTime.UtcNow;
            SutProps = Props.Create(() => new DummyActor(updateChildCreated));
            Supervisor = CreateTestProbe();
            ExpectedChildCount = TestUtils.Create<int>();
        }
        
        [TearDown]
        public void TearDown()
        {
            ChildWaiterMock = null;
            ChildWaiter = null;
            SutProps = null;
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