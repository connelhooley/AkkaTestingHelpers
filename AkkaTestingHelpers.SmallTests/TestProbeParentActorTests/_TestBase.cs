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

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.TestProbeParentActorTests
{
    public class TestBase : TestKit
    {
        internal Mock<ITestProbeCreator> TestProbeCreatorMock;
        internal Mock<IWaiter> ExceptionWaiterMock;
        internal ITestProbeCreator TestProbeCreator;
        internal IWaiter ExceptionWaiter;
        internal object ParentMessage1;
        internal Type ParentType1;
        internal object ParentReply1;
        internal object ParentMessage2;
        internal Type ParentType2;
        internal object ParentReply2;
        internal Func<Exception, Directive> Decider;
        internal Exception StopChildException;
        internal Exception RestartChildException;

        internal Dictionary<Type, Func<object, object>> Handlers;

        internal global::Akka.TestKit.TestProbe TestProbe;

        public TestBase() : base(AkkaConfig.Config)
        {
            //Create mocks
            TestProbeCreatorMock = new Mock<ITestProbeCreator>();
            ExceptionWaiterMock = new Mock<IWaiter>();

            // Create objects passed into sut constructor
            TestProbeCreator = TestProbeCreatorMock.Object;
            ExceptionWaiter = ExceptionWaiterMock.Object;
            ParentMessage1 = new ExampleMessage1();
            ParentType1 = ParentMessage1.GetType();
            ParentReply1 = TestHelper.Generate<object>();
            ParentMessage2 = new ExampleMessage2();
            ParentType2 = ParentMessage2.GetType();
            ParentReply2 = TestHelper.Generate<object>();
            Handlers = new Dictionary<Type, Func<object, object>>
            {
                { ParentType1, o => ParentReply1 },
                { ParentType2, o => ParentReply2 }
            };
            StopChildException = new ArithmeticException(TestHelper.GenerateString());
            RestartChildException = new DivideByZeroException(TestHelper.GenerateString());
            Decider = (e) =>
            {
                if(e.GetType() == StopChildException.GetType())
                {
                    return Directive.Stop;
                }
                if (e.GetType() == RestartChildException.GetType())
                {
                    return Directive.Restart;
                }
                return Directive.Escalate;
            };

            //Create objects returned by mocks
            TestProbe = CreateTestProbe();

            // Set up mocks
            TestProbeCreatorMock
                .SetupSequence(creator => creator.Create(this))
                .Returns(TestProbe)
                .Returns(CreateTestProbe());
        }

        internal TestActorRef<TestProbeParentActor> CreateTestProbeParentActor() =>
            ActorOfAsTestActorRef<TestProbeParentActor>(Props.Create(() =>
                new TestProbeParentActor(TestProbeCreator, ExceptionWaiter, this, Decider, Handlers)));

        private class ExampleMessage1 { }
        private class ExampleMessage2 { }

        internal class DummyChildActor : ReceiveActor
        {
            public static Guid ReplyBeforeRestart = Guid.NewGuid();
            public static Guid ReplyAfterRestart = Guid.NewGuid();
            private Guid _reply;

            public DummyChildActor()
            {
                _reply = ReplyBeforeRestart;
                Receive<Exception>(m => throw m);
                ReceiveAny(_ => Context.Sender.Tell(_reply));
            }

            protected override void PostRestart(Exception reason)
            {
                _reply = ReplyAfterRestart;
                base.PostRestart(reason);
            }
        }
    }
}