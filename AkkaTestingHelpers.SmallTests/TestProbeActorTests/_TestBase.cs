using System;
using System.Collections.Generic;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.Xunit2;
using ConnelHooley.AkkaTestingHelpers.Helpers.Abstract;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;
using Moq;

// ReSharper disable VirtualMemberCallInConstructor

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.TestProbeActorTests
{
    public class TestBase : TestKit
    {
        internal Mock<ITestProbeCreator> TestProbeCreatorMock;
        internal ITestProbeCreator TestProbeCreator;
        internal object Message1;
        internal Type Type1;
        internal object Reply1;
        internal object Message2;
        internal Type Type2;
        internal object Reply2;
        internal Dictionary<Type, Func<object, object>> Handlers;

        internal Akka.TestKit.TestProbe TestProbe;

        public TestBase() : base(AkkaConfig.Config)
        {
            //Create mocks
            TestProbeCreatorMock = new Mock<ITestProbeCreator>();

            // Create objects passed into sut constructor
            TestProbeCreator = TestProbeCreatorMock.Object;
            Message1 = new ExampleMessage1();
            Type1 = typeof(ExampleMessage1);
            Reply1 = TestUtils.Create<object>();
            Message2 = new ExampleMessage2();
            Type2 = typeof(ExampleMessage2);
            Reply2 = TestUtils.Create<object>();
            Handlers = new Dictionary<Type, Func<object, object>>
            {
                { Type1, o => Reply1 },
                { Type2, o => Reply2 }
            };

            //Create objects returned by mocks
            TestProbe = CreateTestProbe();

            // Set up mocks
            TestProbeCreatorMock
                .SetupSequence(creator => creator.Create(this))
                .Returns(TestProbe)
                .Returns(CreateTestProbe());
        }
        
        internal TestActorRef<TestProbeActor> CreateTestProbeActorWithoutSupervisorStrategy() => 
            ActorOfAsTestActorRef<TestProbeActor>(Props.Create(() => new TestProbeActor(TestProbeCreator, this, Handlers)));

        internal TestActorRef<TestProbeActor> CreateTestProbeActorWithSupervisorStrategy(SupervisorStrategy supervisorStrategy) =>
            ActorOfAsTestActorRef<TestProbeActor>(Props
                .Create(() => new TestProbeActor(TestProbeCreator, this, Handlers))
                .WithSupervisorStrategy(supervisorStrategy));
        
        private class ExampleMessage1 { }
        private class ExampleMessage2 { }
    }
}