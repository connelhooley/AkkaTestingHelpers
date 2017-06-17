using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.NUnit3;
using Akka.TestKit.TestActors;
using ConnelHooley.AkkaTestingHelpers.DI.Actors.Abstract;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract;
using Moq;
using NUnit.Framework;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.TestProbeResolverTests
{
    internal class TestBase: TestKit
    {
        protected Func<Type> GenerateType;

        protected Mock<IDependencyResolverAdder> DependencyResolverAdderMock;
        protected Mock<ISutCreator> SutCreatorMock;
        protected Mock<IChildTeller> ChildTellerMock;
        protected Mock<IChildWaiter> ChildWaiterMock;
        protected Mock<ITestProbeCreator> TestProbeCreatorMock;
        protected Mock<IResolvedTestProbeStore> ResolvedTestProbeStoreMock;
        protected Mock<ITestProbeActorCreator> TestProbeActorCreatorMock;
        protected Mock<ITestProbeHandlersMapper> TestProbeHandlersMapperMock;
        protected Mock<ITestProbeActor> TestProbeActorMock;
        
        protected Func<Type, ActorBase> ResolveActor;
        protected List<string> CallOrder;
        protected ImmutableDictionary<Type, ImmutableDictionary<Type, Func<object, object>>> MappedHandlers;

        protected ImmutableDictionary<(Type, Type), Func<object, object>> Handlers;
        protected Props Props;
        protected int ExpectedChildrenCount;
        protected object Message;
        protected IActorRef Recipient;
        protected IActorRef Sender;
        protected string ChildName;
        protected TestProbe Supervisor;
        protected TestActorRef<BlackHoleActor> CreatedActor;
        protected TestActorRef<BlackHoleActor> CreatedActorNoProps;
        protected Type ResolvedType;
        protected TestProbe ResolvedTestProbe;
        protected ITestProbeActor TestProbeActor;
        protected ActorBase Actor;
        protected ActorPath ActorPath;
        protected TestProbe ActorTestProbe;

        public TestBase() : base(AkkaConfig.Config) { }

        [SetUp]
        public void Setup()
        {
            GenerateType = TestUtils.RandomTypeGenerator();

            // Create mocks
            DependencyResolverAdderMock = new Mock<IDependencyResolverAdder>();
            SutCreatorMock = new Mock<ISutCreator>();
            ChildTellerMock = new Mock<IChildTeller>();
            ChildWaiterMock = new Mock<IChildWaiter>();
            TestProbeCreatorMock = new Mock<ITestProbeCreator>();
            ResolvedTestProbeStoreMock = new Mock<IResolvedTestProbeStore>();
            TestProbeActorCreatorMock = new Mock<ITestProbeActorCreator>();
            TestProbeHandlersMapperMock = new Mock<ITestProbeHandlersMapper>();
            TestProbeActorMock = new Mock<ITestProbeActor>();

            // Create objects used by mocks
            CallOrder = new List<string>();
            Supervisor = CreateTestProbe();
            MappedHandlers = ImmutableDictionary<Type, ImmutableDictionary<Type, Func<object, object>>>.Empty;

            // Create objects passed into sut
            Handlers = ImmutableDictionary<(Type, Type), Func<object, object>>.Empty;
            Props = Props.Create<BlackHoleActor>();
            ExpectedChildrenCount = TestUtils.Create<int>();
            Message = TestUtils.Create<object>();
            ChildName = TestUtils.Create<string>();
            Recipient = new Mock<IActorRef>().Object;
            Sender = new Mock<IActorRef>().Object;
            ResolvedType = GenerateType();
            ResolvedTestProbe = CreateTestProbe();
            Actor = new BlackHoleActor();
            ActorPath = TestUtils.Create<ActorPath>();
            ActorTestProbe = CreateTestProbe();

            // Create objects returned by mocks
            CreatedActor = ActorOfAsTestActorRef<BlackHoleActor>(Supervisor);
            CreatedActorNoProps = ActorOfAsTestActorRef<BlackHoleActor>(Supervisor);
            TestProbeActor = TestProbeActorMock.Object;

            // Set up mocks
            DependencyResolverAdderMock
                .Setup(adder => adder.Add(this, It.IsNotNull<Func<Type, ActorBase>>()))
                .Callback((TestKitBase testKit, Func<Type, ActorBase> resolveActor) =>
                {
                    ResolveActor = resolveActor;
                });
            
            SutCreatorMock
                .Setup(creator => creator.Create<BlackHoleActor>(ChildWaiterMock.Object, this, Props, ExpectedChildrenCount, Supervisor))
                .Returns(() => CreatedActor);
            SutCreatorMock
                .Setup(creator => creator.Create<BlackHoleActor>(ChildWaiterMock.Object, this, It.Is<Props>(props => !ReferenceEquals(props, Props) && props.Equals(Props.Create<BlackHoleActor>())), ExpectedChildrenCount, Supervisor))
                .Returns(() => CreatedActorNoProps);
            
            ChildWaiterMock
                .Setup(waiter => waiter.ResolvedChild())
                .Callback(() => CallOrder.Add(nameof(IChildWaiter.ResolvedChild)));

            TestProbeCreatorMock
                .SetupSequence(creator => creator.Create(this))
                .Returns(Supervisor)
                .Throws(new ArithmeticException("Do not call probe creator twice"));

            ResolvedTestProbeStoreMock
                .Setup(store => store.ResolveProbe(It.IsAny<ActorPath>(), It.IsAny<Type>(), It.IsAny<TestProbe>()))
                .Callback(() => CallOrder.Add(nameof(IResolvedTestProbeStore.ResolveProbe)));
            ResolvedTestProbeStoreMock
                .Setup(store => store.FindResolvedType(TestActor, ChildName))
                .Returns(() => ResolvedType);
            ResolvedTestProbeStoreMock
                .Setup(store => store.FindResolvedTestProbe(TestActor, ChildName))
                .Returns(() => ResolvedTestProbe);

            TestProbeHandlersMapperMock
                .Setup(mapper => mapper.Map(Handlers))
                .Returns((ImmutableDictionary<(Type, Type), Func<object, object>> handlers) => MappedHandlers);

            TestProbeActorCreatorMock
                .Setup(creator => creator.Create(this))
                .Returns(() => TestProbeActor);

            TestProbeActorMock
                .SetupGet(actor => actor.TestProbe)
                .Returns(() => ActorTestProbe);
            TestProbeActorMock
                .SetupGet(actor => actor.ActorPath)
                .Returns(() => ActorPath);
            TestProbeActorMock
                .SetupGet(actor => actor.Actor)
                .Returns(() => Actor);
        }

        [TearDown]
        public void TearDown()
        {
            GenerateType = null;
            DependencyResolverAdderMock = null;
            SutCreatorMock = null;
            ChildTellerMock = null;
            ChildWaiterMock = null;
            TestProbeCreatorMock = null;
            ResolvedTestProbeStoreMock = null;
            TestProbeActorCreatorMock = null;
            TestProbeHandlersMapperMock = null;
            CallOrder = null;
            Supervisor = null;
            Props = null;
            ExpectedChildrenCount = default(int);
            Message = null;
            ChildName = null;
            Recipient = null;
            Sender = null;
            CreatedActor = null;
            CreatedActorNoProps = null;
            Actor = null;
            ActorPath = null;
            ActorTestProbe = null;
        }

        public TestProbeResolver CreateTestProbeResolver() => 
            new TestProbeResolver(
                DependencyResolverAdderMock.Object,
                SutCreatorMock.Object,
                ChildTellerMock.Object,
                ChildWaiterMock.Object,
                ResolvedTestProbeStoreMock.Object,
                TestProbeCreatorMock.Object,
                TestProbeActorCreatorMock.Object,
                TestProbeHandlersMapperMock.Object,
                this,
                Handlers);
    }
}