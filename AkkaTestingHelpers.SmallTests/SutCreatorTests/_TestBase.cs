using System;
using System.Collections.Generic;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.Xunit2;
using ConnelHooley.AkkaTestingHelpers.Helpers.Abstract;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;
using ConnelHooley.TestHelpers;
using Moq;

// ReSharper disable VirtualMemberCallInConstructor

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.SutCreatorTests
{
    public class TestBase : TestKit
    {
        internal Mock<IWaiter> ChildWaiterMock;
        internal IWaiter ChildWaiter;
        internal Props Props;
        internal int ExpectedChildCount;
        internal IActorRef Supervisor;

        internal List<string> CallOrder;

        public TestBase() : base(AkkaConfig.Config)
        {
            // Create mocks
            ChildWaiterMock = new Mock<IWaiter>();

            // Create objects used by mocks
            CallOrder = new List<string>();
            
            // Create objects passed into sut methods
            ChildWaiter = ChildWaiterMock.Object;
            Props = Props.Create(() => new DummyActor(() => CallOrder.Add("callback")));
            ExpectedChildCount = TestHelper.GenerateNumber();
            Supervisor = CreateTestProbe();

            // Set up mocks
            ChildWaiterMock
                .Setup(waiter => waiter.Start(It.IsAny<TestKitBase>(), It.IsAny<int>()))
                .Callback(() => CallOrder.Add(nameof(IWaiter.Start)));
            ChildWaiterMock
                .Setup(waiter => waiter.Wait())
                .Callback(() => CallOrder.Add(nameof(IWaiter.Wait)));
        }

        internal SutCreator CreateSutCreator() => new SutCreator();

        protected class DummyActor: ReceiveActor
        {
            public IActorRef Supervisor { get; }

            public DummyActor(Action constructorCallback)
            {
                Supervisor = Context.Parent;
                constructorCallback();
            }
        }
    }
}