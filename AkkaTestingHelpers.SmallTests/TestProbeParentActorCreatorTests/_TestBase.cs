using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.Xunit2;
using ConnelHooley.AkkaTestingHelpers.Helpers.Abstract;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;
using ConnelHooley.TestHelpers;
using Moq;

// ReSharper disable VirtualMemberCallInConstructor

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.TestProbeParentActorCreatorTests
{
    public class TestBase : TestKit
    {
        internal Mock<ITestProbeCreator> TestProbeCreatorMock;
        internal Mock<IWaiter> ExceptionWaiterMock;

        internal TestProbe TestProbe;

        internal ITestProbeCreator TestProbeCreator;
        internal IWaiter ExceptionWaiter;
        internal Func<Exception, Directive> Decider;
        internal IReadOnlyDictionary<Type, Func<object, object>> Handlers;

        public TestBase() : base(AkkaConfig.Config)
        {
            // Create mocks
            TestProbeCreatorMock = new Mock<ITestProbeCreator>();
            ExceptionWaiterMock = new Mock<IWaiter>();

            // Create objects passed into sut methods
            TestProbeCreator = TestProbeCreatorMock.Object;
            ExceptionWaiter = ExceptionWaiterMock.Object;
            Decider = exception => TestHelper.GenerateEnum<Directive>();
            Handlers = ImmutableDictionary<Type, Func<object, object>>.Empty;

            // Create values returned by mocks
            TestProbe = CreateTestProbe();

            // Set up mocks
            TestProbeCreatorMock
                .SetupSequence(creator => creator.Create(this))
                .Returns(TestProbe)
                .Returns(CreateTestProbe());
        }
        
        internal TestProbeParentActorCreator CreateTestProbeParentActorCreator() => 
            new TestProbeParentActorCreator();

        protected class DummyActor : ReceiveActor
        {
            public DummyActor() => ReceiveAny(o => Context.Parent.Tell(o));
        }
    }
}