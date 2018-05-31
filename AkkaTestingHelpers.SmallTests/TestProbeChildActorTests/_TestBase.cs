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

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.TestProbeChildActorTests
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

        internal global::Akka.TestKit.TestProbe TestProbe;

        public TestBase() : base(AkkaConfig.Config)
        {
            //Create mocks
            TestProbeCreatorMock = new Mock<ITestProbeCreator>();

            // Create objects passed into sut constructor
            TestProbeCreator = TestProbeCreatorMock.Object;
            Message1 = new ExampleMessage1();
            Type1 = typeof(ExampleMessage1);
            Reply1 = TestHelper.Generate<object>();
            Message2 = new ExampleMessage2();
            Type2 = typeof(ExampleMessage2);
            Reply2 = TestHelper.Generate<object>();
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
        
        internal TestActorRef<TestProbeChildActor> CreateTestProbeChildActorWithoutSupervisorStrategy() => 
            ActorOfAsTestActorRef<TestProbeChildActor>(Props.Create(() => new TestProbeChildActor(TestProbeCreator, this, Handlers)));

        internal TestActorRef<TestProbeChildActor> CreateTestProbeChildActorWithSupervisorStrategy(SupervisorStrategy supervisorStrategy) =>
            ActorOfAsTestActorRef<TestProbeChildActor>(Props
                .Create(() => new TestProbeChildActor(TestProbeCreator, this, Handlers))
                .WithSupervisorStrategy(supervisorStrategy));
        
        private class ExampleMessage1 { }
        private class ExampleMessage2 { }
    }
}