using System;
using System.Collections.Generic;
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
        internal Mock<ITellWaiter> TellWaiterMock;
        internal Mock<IWaiter> ChildWaiterMock;
        internal Mock<IWaiter> ExceptionWaiterMock;
        internal Mock<IDependencyResolverAdder> DependencyResolverAdderMock;
        internal Mock<ITestProbeDependencyResolverAdder> TestProbeDependencyResolverAdderMock;
        internal Mock<IResolvedTestProbeStore> ResolvedTestProbeStoreMock;
        internal Mock<ITestProbeCreator> TestProbeCreatorMock;
        internal Mock<ITestProbeChildActorCreator> TestProbeChildActorCreatorMock;
        internal Mock<ITestProbeChildHandlersMapper> TestProbeHandlersMapperMock;
        internal Mock<ITestProbeChildActor> TestProbeChildActorMock;
        internal Mock<ISutSupervisorStrategyGetter> SutSupervisorStrategyGetterMock;
        internal Mock<ITestProbeParentActorCreator> TestProbeParentActorCreatorMock;
        internal Mock<ITestProbeParentActor> TestProbeParentActorMock;
        internal Mock<IDelayer> DelayerMock;

        internal ISutCreator SutCreator;
        internal ITellWaiter TellWaiter;
        internal IWaiter ChildWaiter;
        internal IWaiter ExceptionWaiter;
        internal IDependencyResolverAdder DependencyResolverAdder;
        internal ITestProbeDependencyResolverAdder TestProbeDependencyResolverAdder;
        internal ITestProbeCreator TestProbeCreator;
        internal IResolvedTestProbeStore ResolvedTestProbeStore;
        internal ITestProbeChildActorCreator TestProbeChildActorCreator;
        internal ITestProbeChildHandlersMapper TestProbeHandlersMapper;
        internal ISutSupervisorStrategyGetter SutSupervisorStrategyGetter;
        internal ITestProbeParentActorCreator TestProbeParentActorCreator;
        internal IDelayer Delayer;
        
        internal ImmutableDictionary<Type, Func<object, object>> ParentHandlers;
        internal ImmutableDictionary<(Type, Type), Func<object, object>> ChildHandlers;
        internal ImmutableDictionary<Type, ImmutableDictionary<Type, Func<object, object>>> MappedChildHandlers;
        internal Func<Exception, Directive> Decider;

        internal Props Props;
        internal Props PropsWithSupervisorStrategy;
        internal int ExpectedChildCount;
        internal int ExpectedExceptionCount;
        internal object Message;
        internal IActorRef Sender;
        internal string ChildName;
        internal string ChildNameWithoutSupervisor;
        internal Type ResolvedType;
        internal TestProbe ResolvedTestProbe;
        internal TimeSpan DelayDuration;
        internal SupervisorStrategy ResolvedSupervisorStrategy;
        internal SupervisorStrategy SutSupervisorStrategy;
        internal ITestProbeParentActor TestProbeParentActor;
        internal IActorRef TestProbeParentActorRef;
        internal TestProbe TestProbeParentActorTestProbe;
        internal IEnumerable<Exception> TestProbeParentActorUnhandledExceptions;
        protected TestActorRef<DummyActor> SutActor;

        public TestBase() : base(AkkaConfig.Config)
        {
            Func<Type> generateType = TestHelper.GetRandomTypeGenerator();

            // Create mocks
            SutCreatorMock = new Mock<ISutCreator>();
            TellWaiterMock = new Mock<ITellWaiter>();
            ChildWaiterMock = new Mock<IWaiter>();
            ExceptionWaiterMock = new Mock<IWaiter>();
            DependencyResolverAdderMock = new Mock<IDependencyResolverAdder>();
            TestProbeDependencyResolverAdderMock = new Mock<ITestProbeDependencyResolverAdder>();
            ResolvedTestProbeStoreMock = new Mock<IResolvedTestProbeStore>();
            TestProbeChildActorCreatorMock = new Mock<ITestProbeChildActorCreator>();
            TestProbeCreatorMock = new Mock<ITestProbeCreator>();
            TestProbeHandlersMapperMock = new Mock<ITestProbeChildHandlersMapper>();
            TestProbeChildActorMock = new Mock<ITestProbeChildActor>();
            SutSupervisorStrategyGetterMock = new Mock<ISutSupervisorStrategyGetter>();
            TestProbeParentActorCreatorMock = new Mock<ITestProbeParentActorCreator>();
            TestProbeParentActorMock = new Mock<ITestProbeParentActor>();
            DelayerMock = new Mock<IDelayer>();

            // Create objects passed into sut constructor
            SutCreator = SutCreatorMock.Object;
            TellWaiter = TellWaiterMock.Object;
            ChildWaiter = ChildWaiterMock.Object;
            ExceptionWaiter = ChildWaiterMock.Object;
            DependencyResolverAdder = DependencyResolverAdderMock.Object;
            TestProbeDependencyResolverAdder = TestProbeDependencyResolverAdderMock.Object;
            TestProbeCreator = TestProbeCreatorMock.Object;
            ResolvedTestProbeStore = ResolvedTestProbeStoreMock.Object;
            TestProbeChildActorCreator = TestProbeChildActorCreatorMock.Object;
            TestProbeHandlersMapper = TestProbeHandlersMapperMock.Object;
            SutSupervisorStrategyGetter = SutSupervisorStrategyGetterMock.Object;
            TestProbeParentActorCreator = TestProbeParentActorCreatorMock.Object;
            Delayer = DelayerMock.Object;
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
            ExpectedExceptionCount = TestHelper.GenerateNumber();

            // Create objects passed into sut methods
            Message = TestHelper.Generate<object>();
            ChildName = TestHelper.GenerateString();
            ChildNameWithoutSupervisor = TestHelper.GenerateString();
            Sender = new Mock<IActorRef>().Object;
            DelayDuration = TestHelper.Generate<TimeSpan>();

            // Create objects returned by mocks
            MappedChildHandlers = ImmutableDictionary<Type, ImmutableDictionary<Type, Func<object, object>>>
                .Empty
                .Add(generateType(), ImmutableDictionary<Type, Func<object, object>>
                    .Empty
                    .Add(generateType(), mess => TestHelper.Generate<object>()));
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
            TestProbeParentActor = TestProbeParentActorMock.Object;
            TestProbeParentActorTestProbe = CreateTestProbe();
            TestProbeParentActorRef = TestProbeParentActorTestProbe.Ref;
            TestProbeParentActorUnhandledExceptions = TestHelper.GenerateMany<Exception>(() => TestHelper.GenerateException());
            
            // Set up mocks
            TestProbeParentActorMock
                .SetupGet(actor => actor.Ref)
                .Returns(TestProbeParentActorRef);
            TestProbeParentActorMock
                .SetupGet(actor => actor.TestProbe)
                .Returns(TestProbeParentActorTestProbe);
            TestProbeParentActorMock
                .SetupGet(actor => actor.UnhandledExceptions)
                .Returns(TestProbeParentActorUnhandledExceptions);

            TestProbeParentActorCreatorMock
                .SetupSequence(creator => creator.Create(TestProbeCreator, ExceptionWaiter, this, Decider, ParentHandlers))
                .Returns(TestProbeParentActor)
                .Returns(Mock.Of<ITestProbeParentActor>());

            SutCreatorMock
                .Setup(creator => creator.Create<DummyActor>(
                    ChildWaiterMock.Object,
                    this,
                    Props,
                    ExpectedChildCount,
                    TestProbeParentActorRef))
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
                TellWaiter,
                ChildWaiter,
                ExceptionWaiter,
                DependencyResolverAdder,
                TestProbeDependencyResolverAdder,
                TestProbeCreator,
                ResolvedTestProbeStore,
                TestProbeChildActorCreator,
                TestProbeHandlersMapper,
                SutSupervisorStrategyGetter,
                TestProbeParentActorCreator,
                Delayer,
                ParentHandlers,
                ChildHandlers,
                this,
                props ?? Props,
                Decider,
                ExpectedChildCount);

        protected class DummyActor : ReceiveActor { }
    }
}