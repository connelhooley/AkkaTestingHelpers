using System;
using System.Collections.Immutable;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.Xunit2;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract;
using Moq;
// ReSharper disable VirtualMemberCallInConstructor

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.UnitTestFrameworkTests
{
    public class TestBase: TestKit
    {
        internal Mock<ISutCreator> SutCreatorMock;
        internal Mock<IChildTeller> ChildTellerMock;
        internal Mock<IChildWaiter> ChildWaiterMock;
        internal Mock<IDependencyResolverAdder> DependencyResolverAdderMock;
        internal Mock<ITestProbeDependencyResolverAdder> TestProbeDependencyResolverAdderMock;
        internal Mock<IResolvedTestProbeStore> ResolvedTestProbeStoreMock;
        internal Mock<ITestProbeCreator> TestProbeCreatorMock;
        internal Mock<ITestProbeActorCreator> TestProbeActorCreatorMock;
        internal Mock<ITestProbeHandlersMapper> TestProbeHandlersMapperMock;
        internal Mock<ITestProbeActor> TestProbeActorMock;

        internal ISutCreator SutCreator;
        internal IChildTeller ChildTeller;
        internal IChildWaiter ChildWaiter;
        internal IDependencyResolverAdder DependencyResolverAdder;
        internal ITestProbeDependencyResolverAdder TestProbeDependencyResolverAdder;
        internal ITestProbeCreator TestProbeCreator;
        internal IResolvedTestProbeStore ResolvedTestProbeStore;
        internal ITestProbeActorCreator TestProbeActorCreator;
        internal ITestProbeHandlersMapper TestProbeHandlersMapper;
        
        internal ImmutableDictionary<(Type, Type), Func<object, object>> Handlers;
        internal ImmutableDictionary<Type, ImmutableDictionary<Type, Func<object, object>>> MappedHandlers;

        internal Props Props;
        internal int ExpectedChildCount;
        internal object Message;
        internal IActorRef Sender;
        internal string ChildName;
        internal TestProbe Supervisor;
        internal Type ResolvedType;
        internal TestProbe ResolvedTestProbe;
        protected TestActorRef<DummyActor> SutActor;

        public TestBase() : base(AkkaConfig.Config)
        {
            Func<Type> generateType = TestUtils.RandomTypeGenerator();

            // Create mocks
            SutCreatorMock = new Mock<ISutCreator>();
            ChildTellerMock = new Mock<IChildTeller>();
            ChildWaiterMock = new Mock<IChildWaiter>();
            DependencyResolverAdderMock = new Mock<IDependencyResolverAdder>();
            TestProbeDependencyResolverAdderMock = new Mock<ITestProbeDependencyResolverAdder>();
            ResolvedTestProbeStoreMock = new Mock<IResolvedTestProbeStore>();
            TestProbeActorCreatorMock = new Mock<ITestProbeActorCreator>();
            TestProbeCreatorMock = new Mock<ITestProbeCreator>();
            TestProbeHandlersMapperMock = new Mock<ITestProbeHandlersMapper>();
            TestProbeActorMock = new Mock<ITestProbeActor>();

            // Create objects passed into sut constructor
            SutCreator = SutCreatorMock.Object;
            ChildTeller = ChildTellerMock.Object;
            ChildWaiter = ChildWaiterMock.Object;
            DependencyResolverAdder = DependencyResolverAdderMock.Object;
            TestProbeDependencyResolverAdder = TestProbeDependencyResolverAdderMock.Object;
            TestProbeCreator = TestProbeCreatorMock.Object;
            ResolvedTestProbeStore = ResolvedTestProbeStoreMock.Object;
            TestProbeActorCreator = TestProbeActorCreatorMock.Object;
            TestProbeHandlersMapper = TestProbeHandlersMapperMock.Object;
            Handlers = ImmutableDictionary<(Type, Type), Func<object, object>>
                .Empty
                .Add((generateType(), generateType()), message => TestUtils.Create<object>());
            Props = Props.Create<DummyActor>();
            ExpectedChildCount = TestUtils.Create<int>();
        
            // Create objects passed into sut methods
            Message = TestUtils.Create<object>();
            ChildName = TestUtils.Create<string>();
            Sender = new Mock<IActorRef>().Object;

            // Create objects returned by mocks
            MappedHandlers = ImmutableDictionary<Type, ImmutableDictionary<Type, Func<object, object>>>
                .Empty
                .Add(generateType(), ImmutableDictionary<Type, Func<object, object>>
                    .Empty
                    .Add(generateType(), mess => TestUtils.Create<object>()));
            Supervisor = CreateTestProbe();
            SutActor = ActorOfAsTestActorRef<DummyActor>();
            ResolvedType = generateType();
            ResolvedTestProbe = CreateTestProbe();

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

            TestProbeHandlersMapperMock
                .Setup(mapper => mapper.Map(Handlers))
                .Returns(() => MappedHandlers);
        }
        
        protected UnitTestFramework<DummyActor> CreateTestProbeResolver() => 
            new UnitTestFramework<DummyActor>(
                SutCreator,
                ChildTeller,
                ChildWaiter,
                DependencyResolverAdder,
                TestProbeDependencyResolverAdder,
                TestProbeCreator,
                ResolvedTestProbeStore,
                TestProbeActorCreator,
                TestProbeHandlersMapper,
                Handlers,
                this,
                Props,
                ExpectedChildCount);

        protected class DummyActor : ReceiveActor { }
    }
}