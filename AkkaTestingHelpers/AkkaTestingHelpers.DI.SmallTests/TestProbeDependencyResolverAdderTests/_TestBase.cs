using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.Xunit2;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;
using Moq;
// ReSharper disable VirtualMemberCallInConstructor

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.TestProbeDependencyResolverAdderTests
{
    public class TestBase : TestKit
    {
        internal readonly Mock<IDependencyResolverAdder> DependencyResolverAdderMock;
        internal readonly Mock<ITestProbeActorCreator> TestProbeActorCreatorMock;
        internal readonly Mock<ITestProbeCreator> TestProbeCreatorMock;
        internal readonly Mock<IResolvedTestProbeStore> ResolvedTestProbeStoreMock;
        internal readonly Mock<IChildWaiter> ChildWaiterMock;
        internal readonly Mock<ITestProbeActor> TestProbeActorWithHandlersMock;
        internal readonly Mock<ITestProbeActor> TestProbeActorWithoutHandlersMock;

        internal readonly IDependencyResolverAdder DependencyResolverAdder;
        internal readonly ITestProbeActorCreator TestProbeActorCreator;
        internal readonly ITestProbeCreator TestProbeCreator;
        internal readonly IResolvedTestProbeStore ResolvedTestProbeStore;
        internal readonly IChildWaiter ChildWaiter;
        
        internal readonly ImmutableDictionary<Type, ImmutableDictionary<Type, Func<object, object>>> Handlers;
        internal readonly ImmutableDictionary<Type, Func<object, object>> ActorHandlers;
        internal readonly Type ActorWithHandlersType;
        internal readonly ActorBase ResolvedActorWithHandlers;
        internal readonly ActorPath ResolvedActorPathWithHandlers;
        internal readonly TestProbe ResolvedTestProbeWithHandlers;
        internal readonly Type ActorWithoutHandlersType;
        internal readonly ActorBase ResolvedActorWithoutHandlers;
        internal readonly ActorPath ResolvedActorPathWithoutHandlers;
        internal readonly TestProbe ResolvedTestProbeWithoutHandlers;

        internal readonly List<string> CallOrder;
        internal Func<Type, ActorBase> ActorFactory;
        
        public TestBase() : base(AkkaConfig.Config)
        {
            Func<Type> generateType = TestUtils.RandomTypeGenerator();

            // Create mocks
            DependencyResolverAdderMock = new Mock<IDependencyResolverAdder>();
            TestProbeActorCreatorMock = new Mock<ITestProbeActorCreator>();
            TestProbeCreatorMock = new Mock<ITestProbeCreator>();
            ResolvedTestProbeStoreMock = new Mock<IResolvedTestProbeStore>();
            ChildWaiterMock = new Mock<IChildWaiter>();
            TestProbeActorWithHandlersMock = new Mock<ITestProbeActor>();
            TestProbeActorWithoutHandlersMock = new Mock<ITestProbeActor>();

            // Create objects used by mocks
            CallOrder = new List<string>();

            // Create objects passed into ActorFactory
            ActorWithHandlersType = generateType();
            ActorWithoutHandlersType = generateType();

            // Create objects passed into sut methods
            DependencyResolverAdder = DependencyResolverAdderMock.Object;
            TestProbeActorCreator = TestProbeActorCreatorMock.Object;
            TestProbeCreator = TestProbeCreatorMock.Object;
            ResolvedTestProbeStore = ResolvedTestProbeStoreMock.Object;
            ChildWaiter = ChildWaiterMock.Object;
            ActorHandlers = ImmutableDictionary<Type, Func<object, object>>
                .Empty
                .Add(generateType(), mess => TestUtils.Create<object>());
            Handlers = ImmutableDictionary<Type, ImmutableDictionary<Type, Func<object, object>>>
                .Empty
                .Add(ActorWithHandlersType, ActorHandlers);

            // Create values returned by mocks
            ResolvedActorWithHandlers = new Mock<ActorBase>().Object;
            ResolvedActorPathWithHandlers = ActorPath.Parse($"akka://{TestUtils.Create<string>()}");
            ResolvedTestProbeWithHandlers = CreateTestProbe();
            
            ResolvedActorWithoutHandlers = new Mock<ActorBase>().Object;
            ResolvedActorPathWithoutHandlers = ActorPath.Parse($"akka://{TestUtils.Create<string>()}");
            ResolvedTestProbeWithoutHandlers = CreateTestProbe();
            
            // Set up mocks
            TestProbeActorWithHandlersMock
                .Setup(actor => actor.Actor)
                .Returns(() => ResolvedActorWithHandlers);

            TestProbeActorWithHandlersMock
                .Setup(actor => actor.ActorPath)
                .Returns(() => ResolvedActorPathWithHandlers);
            
            TestProbeActorWithHandlersMock
                .Setup(actor => actor.TestProbe)
                .Returns(() => ResolvedTestProbeWithHandlers);

            TestProbeActorWithoutHandlersMock
                .Setup(actor => actor.Actor)
                .Returns(() => ResolvedActorWithoutHandlers);

            TestProbeActorWithoutHandlersMock
                .Setup(actor => actor.ActorPath)
                .Returns(() => ResolvedActorPathWithoutHandlers);

            TestProbeActorWithoutHandlersMock
                .Setup(actor => actor.TestProbe)
                .Returns(() => ResolvedTestProbeWithoutHandlers);

            TestProbeActorCreatorMock
                .Setup(creator => creator.Create(TestProbeCreator, this, ActorHandlers))
                .Returns(() => TestProbeActorWithHandlersMock.Object);

            TestProbeActorCreatorMock
                .Setup(creator => creator.Create(TestProbeCreator, this, null))
                .Returns(() => TestProbeActorWithoutHandlersMock.Object);

            ResolvedTestProbeStoreMock
                .Setup(store => store.ResolveProbe(It.IsAny<ActorPath>(), It.IsAny<Type>(), It.IsAny<TestProbe>()))
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