using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.Xunit2;
using Akka.TestKit.TestActors;
using ConnelHooley.AkkaTestingHelpers.DI.Actors.Abstract;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract;
using Moq;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.TestProbeResolverTests
{
    public class TestBase: TestKit
    {
        internal Func<Type> GenerateType;

        internal Mock<IDependencyResolverAdder> DependencyResolverAdderMock;
        internal Mock<ISutCreator> SutCreatorMock;
        internal Mock<IChildTeller> ChildTellerMock;
        internal Mock<IChildWaiter> ChildWaiterMock;
        internal Mock<ITestProbeCreator> TestProbeCreatorMock;
        internal Mock<IResolvedTestProbeStore> ResolvedTestProbeStoreMock;
        internal Mock<ITestProbeActorCreator> TestProbeActorCreatorMock;
        internal Mock<ITestProbeHandlersMapper> TestProbeHandlersMapperMock;
        internal Mock<ITestProbeActor> TestProbeActorMock;
        
        internal Func<Type, ActorBase> ResolveActor;
        internal List<string> CallOrder;
        internal ImmutableDictionary<Type, ImmutableDictionary<Type, Func<object, object>>> MappedHandlers;

        internal ImmutableDictionary<(Type, Type), Func<object, object>> Handlers;
        internal Props Props;
        internal int ExpectedChildrenCount;
        internal object Message;
        internal IActorRef Recipient;
        internal IActorRef Sender;
        internal string ChildName;
        internal TestProbe Supervisor;
        internal TestActorRef<BlackHoleActor> CreatedActor;
        internal TestActorRef<BlackHoleActor> CreatedActorNoProps;
        internal Type ResolvedType;
        internal TestProbe ResolvedTestProbe;
        internal ITestProbeActor TestProbeActor;
        internal ActorBase Actor;
        internal ActorPath ActorPath;
        internal TestProbe ActorTestProbe;

        public TestBase() : base(AkkaConfig.Config)
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
        
        internal TestProbeResolver CreateTestProbeResolver() => 
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