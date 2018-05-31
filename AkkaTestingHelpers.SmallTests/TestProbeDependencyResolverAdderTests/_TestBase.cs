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

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.TestProbeDependencyResolverAdderTests
{
    public class TestBase : TestKit
    {
        internal readonly Mock<IDependencyResolverAdder> DependencyResolverAdderMock;
        internal readonly Mock<ITestProbeChildActorCreator> TestProbeChildActorCreatorMock;
        internal readonly Mock<ITestProbeCreator> TestProbeCreatorMock;
        internal readonly Mock<IResolvedTestProbeStore> ResolvedTestProbeStoreMock;
        internal readonly Mock<IChildWaiter> ChildWaiterMock;
        internal readonly Mock<ITestProbeChildActor> TestProbeChildActorWithHandlersMock;
        internal readonly Mock<ITestProbeChildActor> TestProbeChildActorWithoutHandlersMock;

        internal readonly IDependencyResolverAdder DependencyResolverAdder;
        internal readonly ITestProbeChildActorCreator TestProbeChildActorCreator;
        internal readonly ITestProbeCreator TestProbeCreator;
        internal readonly IResolvedTestProbeStore ResolvedTestProbeStore;
        internal readonly IChildWaiter ChildWaiter;
        
        internal readonly ImmutableDictionary<Type, ImmutableDictionary<Type, Func<object, object>>> Handlers;
        internal readonly ImmutableDictionary<Type, Func<object, object>> ActorHandlers;
        internal readonly Type ActorWithHandlersType;
        internal readonly ActorBase ResolvedActorWithHandlers;
        internal readonly ActorPath ResolvedActorPathWithHandlers;
        internal readonly TestProbe ResolvedTestProbeWithHandlers;
        internal readonly SupervisorStrategy ResolvedSupervisorStrategyWithHandlers;
        internal readonly Type ActorWithoutHandlersType;
        internal readonly ActorBase ResolvedActorWithoutHandlers;
        internal readonly ActorPath ResolvedActorPathWithoutHandlers;
        internal readonly TestProbe ResolvedTestProbeWithoutHandlers;
        internal readonly SupervisorStrategy ResolvedSupervisorStrategyWithoutHandlers;

        internal readonly List<string> CallOrder;
        internal Func<Type, ActorBase> ActorFactory;
        
        public TestBase() : base(AkkaConfig.Config)
        {
            Func<Type> generateType = TestHelper.GetRandomTypeGenerator();

            // Create mocks
            DependencyResolverAdderMock = new Mock<IDependencyResolverAdder>();
            TestProbeChildActorCreatorMock = new Mock<ITestProbeChildActorCreator>();
            TestProbeCreatorMock = new Mock<ITestProbeCreator>();
            ResolvedTestProbeStoreMock = new Mock<IResolvedTestProbeStore>();
            ChildWaiterMock = new Mock<IChildWaiter>();
            TestProbeChildActorWithHandlersMock = new Mock<ITestProbeChildActor>();
            TestProbeChildActorWithoutHandlersMock = new Mock<ITestProbeChildActor>();

            // Create objects used by mocks
            CallOrder = new List<string>();

            // Create objects passed into ActorFactory
            ActorWithHandlersType = generateType();
            ActorWithoutHandlersType = generateType();

            // Create objects passed into sut methods
            DependencyResolverAdder = DependencyResolverAdderMock.Object;
            TestProbeChildActorCreator = TestProbeChildActorCreatorMock.Object;
            TestProbeCreator = TestProbeCreatorMock.Object;
            ResolvedTestProbeStore = ResolvedTestProbeStoreMock.Object;
            ChildWaiter = ChildWaiterMock.Object;
            ActorHandlers = ImmutableDictionary<Type, Func<object, object>>
                .Empty
                .Add(generateType(), mess => TestHelper.Generate<object>());
            Handlers = ImmutableDictionary<Type, ImmutableDictionary<Type, Func<object, object>>>
                .Empty
                .Add(ActorWithHandlersType, ActorHandlers);

            // Create values returned by mocks
            ResolvedActorWithHandlers = new Mock<ActorBase>().Object;
            ResolvedActorPathWithHandlers = TestHelper.Generate<ActorPath>();
            ResolvedTestProbeWithHandlers = CreateTestProbe();
            ResolvedSupervisorStrategyWithHandlers = new AllForOneStrategy(
                TestHelper.GenerateNumber(),
                TestHelper.GenerateNumber(),
                exception => TestHelper.Generate<Directive>());

            ResolvedActorWithoutHandlers = new Mock<ActorBase>().Object;
            ResolvedActorPathWithoutHandlers = TestHelper.Generate<ActorPath>();
            ResolvedTestProbeWithoutHandlers = CreateTestProbe();
            ResolvedSupervisorStrategyWithoutHandlers = new OneForOneStrategy(
                TestHelper.GenerateNumber(),
                TestHelper.GenerateNumber(),
                exception => TestHelper.Generate<Directive>());

            // Set up mocks
            TestProbeChildActorWithHandlersMock
                .Setup(actor => actor.Actor)
                .Returns(() => ResolvedActorWithHandlers);

            TestProbeChildActorWithHandlersMock
                .Setup(actor => actor.ActorPath)
                .Returns(() => ResolvedActorPathWithHandlers);
            
            TestProbeChildActorWithHandlersMock
                .Setup(actor => actor.TestProbe)
                .Returns(() => ResolvedTestProbeWithHandlers);

            TestProbeChildActorWithHandlersMock
                .Setup(actor => actor.PropsSupervisorStrategy)
                .Returns(() => ResolvedSupervisorStrategyWithHandlers);

            TestProbeChildActorWithoutHandlersMock
                .Setup(actor => actor.Actor)
                .Returns(() => ResolvedActorWithoutHandlers);

            TestProbeChildActorWithoutHandlersMock
                .Setup(actor => actor.ActorPath)
                .Returns(() => ResolvedActorPathWithoutHandlers);

            TestProbeChildActorWithoutHandlersMock
                .Setup(actor => actor.TestProbe)
                .Returns(() => ResolvedTestProbeWithoutHandlers);

            TestProbeChildActorWithoutHandlersMock
                .Setup(actor => actor.PropsSupervisorStrategy)
                .Returns(() => ResolvedSupervisorStrategyWithoutHandlers);

            TestProbeChildActorCreatorMock
                .Setup(creator => creator.Create(TestProbeCreator, this, ActorHandlers))
                .Returns(() => TestProbeChildActorWithHandlersMock.Object);

            TestProbeChildActorCreatorMock
                .Setup(creator => creator.Create(TestProbeCreator, this, null))
                .Returns(() => TestProbeChildActorWithoutHandlersMock.Object);

            ResolvedTestProbeStoreMock
                .Setup(store => store.ResolveProbe(It.IsAny<ActorPath>(), It.IsAny<Type>(), It.IsAny<TestProbe>(), It.IsAny<SupervisorStrategy>()))
                .Callback(() => CallOrder.Add(nameof(IResolvedTestProbeStore.ResolveProbe)));
            
            ChildWaiterMock
                .Setup(waiter => waiter.ResolvedChild())
                .Callback(() => CallOrder.Add(nameof(IChildWaiter.ResolvedChild)));

            DependencyResolverAdderMock
                .Setup(adder => adder.Add(this, It.IsAny<Func<Type, ActorBase>>()))
                .Callback((TestKitBase testKit, Func<Type, ActorBase> actorFactory) => ActorFactory = actorFactory);
        }
        
        internal TestProbeDependencyResolverAdder CreateTestProbeDependencyResolverAdder() => 
            new TestProbeDependencyResolverAdder();
    }
}