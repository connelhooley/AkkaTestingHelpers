using System;
using System.Collections.Immutable;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.Xunit2;
using ConnelHooley.AkkaTestingHelpers.Helpers.Abstract;
using ConnelHooley.TestHelpers;
using Moq;

// ReSharper disable VirtualMemberCallInConstructor

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.UnitTestFrameworkTests
{
    public class TestBase: TestKit
    {
        internal Mock<ISutCreator> SutCreatorMock;
        internal Mock<ITellChildWaiter> ChildTellerMock;
        internal Mock<IChildWaiter> ChildWaiterMock;
        internal Mock<IDependencyResolverAdder> DependencyResolverAdderMock;
        internal Mock<ITestProbeDependencyResolverAdder> TestProbeDependencyResolverAdderMock;
        internal Mock<IResolvedTestProbeStore> ResolvedTestProbeStoreMock;
        internal Mock<ITestProbeCreator> TestProbeCreatorMock;
        internal Mock<ITestProbeChildActorCreator> TestProbeChildActorCreatorMock;
        internal Mock<ITestProbeChildHandlersMapper> TestProbeHandlersMapperMock;
        internal Mock<ITestProbeChildActor> TestProbeChildActorMock;
        internal Mock<ISutSupervisorStrategyGetter> SutSupervisorStrategyGetterMock;

        internal ISutCreator SutCreator;
        internal ITellChildWaiter ChildTeller;
        internal IChildWaiter ChildWaiter;
        internal IDependencyResolverAdder DependencyResolverAdder;
        internal ITestProbeDependencyResolverAdder TestProbeDependencyResolverAdder;
        internal ITestProbeCreator TestProbeCreator;
        internal IResolvedTestProbeStore ResolvedTestProbeStore;
        internal ITestProbeChildActorCreator TestProbeChildActorCreator;
        internal ITestProbeChildHandlersMapper TestProbeHandlersMapper;
        internal ISutSupervisorStrategyGetter SutSupervisorStrategyGetter;
        
        internal ImmutableDictionary<Type, Func<object, object>> ParentHandlers;
        internal ImmutableDictionary<(Type, Type), Func<object, object>> ChildHandlers;
        internal ImmutableDictionary<Type, ImmutableDictionary<Type, Func<object, object>>> MappedChildHandlers;
        internal Func<Exception, Directive> Decider;

        internal Props Props;
        internal Props PropsWithSupervisorStrategy;
        internal int ExpectedChildCount;
        internal object Message;
        internal IActorRef Sender;
        internal string ChildName;
        internal string ChildNameWithoutSupervisor;
        internal TestProbe Supervisor;
        internal Type ResolvedType;
        internal TestProbe ResolvedTestProbe;
        internal SupervisorStrategy ResolvedSupervisorStrategy;
        internal SupervisorStrategy SutSupervisorStrategy;
        protected TestActorRef<DummyActor> SutActor;

        public TestBase() : base(AkkaConfig.Config)
        {
            Func<Type> generateType = TestHelper.GetRandomTypeGenerator();

            // Create mocks
            SutCreatorMock = new Mock<ISutCreator>();
            ChildTellerMock = new Mock<ITellChildWaiter>();
            ChildWaiterMock = new Mock<IChildWaiter>();
            DependencyResolverAdderMock = new Mock<IDependencyResolverAdder>();
            TestProbeDependencyResolverAdderMock = new Mock<ITestProbeDependencyResolverAdder>();
            ResolvedTestProbeStoreMock = new Mock<IResolvedTestProbeStore>();
            TestProbeChildActorCreatorMock = new Mock<ITestProbeChildActorCreator>();
            TestProbeCreatorMock = new Mock<ITestProbeCreator>();
            TestProbeHandlersMapperMock = new Mock<ITestProbeChildHandlersMapper>();
            TestProbeChildActorMock = new Mock<ITestProbeChildActor>();
            SutSupervisorStrategyGetterMock = new Mock<ISutSupervisorStrategyGetter>();

            // Create objects passed into sut constructor
            SutCreator = SutCreatorMock.Object;
            ChildTeller = ChildTellerMock.Object;
            ChildWaiter = ChildWaiterMock.Object;
            DependencyResolverAdder = DependencyResolverAdderMock.Object;
            TestProbeDependencyResolverAdder = TestProbeDependencyResolverAdderMock.Object;
            TestProbeCreator = TestProbeCreatorMock.Object;
            ResolvedTestProbeStore = ResolvedTestProbeStoreMock.Object;
            TestProbeChildActorCreator = TestProbeChildActorCreatorMock.Object;
            TestProbeHandlersMapper = TestProbeHandlersMapperMock.Object;
            SutSupervisorStrategyGetter = SutSupervisorStrategyGetterMock.Object;
            ParentHandlers = ImmutableDictionary<Type, Func<object, object>>
                .Empty
                .Add((generateType()), message => TestHelper.Generate<object>());
            ChildHandlers = ImmutableDictionary<(Type, Type), Func<object, object>>
                .Empty
                .Add((generateType(), generateType()), message => TestHelper.Generate<object>());
            Decider = exception => TestHelper.GenerateEnum<Directive>();
            Props = Props.Create<DummyActor>();
            PropsWithSupervisorStrategy = Props
                .Create<DummyActor>()
                .WithSupervisorStrategy(new AllForOneStrategy(
                    TestHelper.GenerateNumber(),
                    TestHelper.GenerateNumber(),
                    exception => TestHelper.Generate<Directive>()));
            ExpectedChildCount = TestHelper.GenerateNumber();
        
            // Create objects passed into sut methods
            Message = TestHelper.Generate<object>();
            ChildName = TestHelper.GenerateString();
            ChildNameWithoutSupervisor = TestHelper.GenerateString();
            Sender = new Mock<IActorRef>().Object;

            // Create objects returned by mocks
            MappedChildHandlers = ImmutableDictionary<Type, ImmutableDictionary<Type, Func<object, object>>>
                .Empty
                .Add(generateType(), ImmutableDictionary<Type, Func<object, object>>
                    .Empty
                    .Add(generateType(), mess => TestHelper.Generate<object>()));
            Supervisor = CreateTestProbe();
            SutActor = ActorOfAsTestActorRef<DummyActor>();
            ResolvedType = generateType();
            ResolvedTestProbe = CreateTestProbe();
            ResolvedSupervisorStrategy = new AllForOneStrategy(
                TestHelper.GenerateNumber(), 
                TestHelper.GenerateNumber(),
                exception => TestHelper.Generate<Directive>());
            SutSupervisorStrategy = new OneForOneStrategy(
                TestHelper.GenerateNumber(),
                TestHelper.GenerateNumber(),
                exception => TestHelper.Generate<Directive>());

            // Set up mocks
            TestProbeCreatorMock
                .SetupSequence(creator => creator.Create(this))
                .Returns(Supervisor)
                .Returns(CreateTestProbe());
                
            SutCreatorMock
                .Setup(creator => creator.Create<DummyActor>(
                    ChildWaiterMock.Object, 
                    this, 
                    Props, 
                    ExpectedChildCount, 
                    Supervisor))
                .Returns(() => SutActor);
            
            ResolvedTestProbeStoreMock
                .Setup(store => store.FindResolvedType(SutActor, ChildName))
                .Returns(() => ResolvedType);
            ResolvedTestProbeStoreMock
                .Setup(store => store.FindResolvedTestProbe(SutActor, ChildName))
                .Returns(() => ResolvedTestProbe);
            ResolvedTestProbeStoreMock
                .Setup(store => store.FindResolvedSupervisorStrategy(SutActor, ChildName))
                .Returns(() => ResolvedSupervisorStrategy);
            ResolvedTestProbeStoreMock
                .Setup(store => store.FindResolvedSupervisorStrategy(SutActor, ChildNameWithoutSupervisor))
                .Returns(() => null);

            TestProbeHandlersMapperMock
                .Setup(mapper => mapper.Map(ChildHandlers))
                .Returns(() => MappedChildHandlers);

            SutSupervisorStrategyGetterMock
                .Setup(getter => getter.Get(SutActor.UnderlyingActor))
                .Returns(() => SutSupervisorStrategy);
        }
        
        protected UnitTestFramework<DummyActor> CreateUnitTestFramework(Props props = null) => 
            new UnitTestFramework<DummyActor>(
                SutCreator,
                ChildTeller,
                ChildWaiter,
                DependencyResolverAdder,
                TestProbeDependencyResolverAdder,
                TestProbeCreator,
                ResolvedTestProbeStore,
                TestProbeChildActorCreator,
                TestProbeHandlersMapper,
                SutSupervisorStrategyGetter,
                ParentHandlers,
                ChildHandlers,
                this,
                props ?? Props,
                Decider,
                ExpectedChildCount);

        protected class DummyActor : ReceiveActor { }
    }
}