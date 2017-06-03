using System;
using System.Collections.Generic;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.NUnit3;
using Akka.TestKit.TestActors;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract;
using Moq;
using NUnit.Framework;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.TestProbeResolverTests
{
    internal class TestBase: TestKit
    {
        protected Mock<IDependencyResolverAdder> DependencyResolverAdderMock;
        protected Mock<ISutCreator> SutCreatorMock;
        protected Mock<IChildTeller> ChildTellerMock;
        protected Mock<IChildWaiter> ChildWaiterMock;
        protected Mock<ITestProbeCreator> TestProbeCreatorMock;
        protected Mock<IResolvedTestProbeStore> ResolvedTestProbeStoreMock;
        protected Mock<ITestProbeActorFactory> TestProbeActorFactoryMock;
        protected Mock<ITestProbeHandlersMapper> TestProbeHandlersMapperMock;
        
        protected Func<Type, ActorBase> ResolveActor;
        protected List<string> CallOrder;

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

        [SetUp]
        public void Setup()
        {
            // Create mocks
            DependencyResolverAdderMock = new Mock<IDependencyResolverAdder>();
            SutCreatorMock = new Mock<ISutCreator>();
            ChildTellerMock = new Mock<IChildTeller>();
            ChildWaiterMock = new Mock<IChildWaiter>();
            TestProbeCreatorMock = new Mock<ITestProbeCreator>();
            ResolvedTestProbeStoreMock = new Mock<IResolvedTestProbeStore>();
            TestProbeActorFactoryMock = new Mock<ITestProbeActorFactory>();
            TestProbeHandlersMapperMock = new Mock<ITestProbeHandlersMapper>();

            // Create objects used by mocks
            CallOrder = new List<string>();
            Supervisor = CreateTestProbe();
            
            // Create objects passed into sut
            Props = Props.Create<BlackHoleActor>();
            ExpectedChildrenCount = TestUtils.Create<int>();
            Message = TestUtils.Create<object>();
            ChildName = TestUtils.Create<string>();
            Recipient = new Mock<IActorRef>().Object;
            Sender = new Mock<IActorRef>().Object;
            ResolvedType = TestUtils.RandomTypeGenerator()();
            ResolvedTestProbe = CreateTestProbe();

            // Create objects returned by mocks
            CreatedActor = ActorOfAsTestActorRef<BlackHoleActor>(Supervisor);
            CreatedActorNoProps = ActorOfAsTestActorRef<BlackHoleActor>(Supervisor);

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
                .Setup(waiter => waiter.Start(this, ExpectedChildrenCount))
                .Callback(() => CallOrder.Add(nameof(IChildWaiter.Start)));
            ChildWaiterMock
                .Setup(waiter => waiter.Wait())
                .Callback(() => CallOrder.Add(nameof(IChildWaiter.Wait)));
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
        }

        [TearDown]
        public void TearDown()
        {
            DependencyResolverAdderMock = null;
            SutCreatorMock = null;
            ChildTellerMock = null;
            ChildWaiterMock = null;
            TestProbeCreatorMock = null;
            ResolvedTestProbeStoreMock = null;
            TestProbeActorFactoryMock = null;
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
        }

        public TestProbeResolver CreateTestProbeResolver(TestProbeResolverSettings settings) => 
            new TestProbeResolver(
                DependencyResolverAdderMock.Object,
                SutCreatorMock.Object,
                ChildTellerMock.Object,
                ChildWaiterMock.Object,
                ResolvedTestProbeStoreMock.Object,
                TestProbeCreatorMock.Object,
                TestProbeActorFactoryMock.Object,
                TestProbeHandlersMapperMock.Object,
                this,
                settings);
    }
}