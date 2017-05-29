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
        protected Mock<IChildWaiter> ChildWaiterMock;
        protected Mock<ITestProbeCreator> TestProbeCreatorMock;
        protected Mock<IResolvedTestProbeStore> ResolvedTestProbeStoreMock;
        protected Mock<IActorRef> RecipientMock;
        
        protected Func<Type, ActorBase> ResolveActor;
        protected List<string> CallOrder;

        protected Props Props;
        protected int ExpectedChildrenCount;
        protected object Message;
        protected IActorRef Recipient;
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
            ChildWaiterMock = new Mock<IChildWaiter>();
            TestProbeCreatorMock = new Mock<ITestProbeCreator>();
            ResolvedTestProbeStoreMock = new Mock<IResolvedTestProbeStore>();
            RecipientMock = new Mock<IActorRef>();

            // Create objects used by mocks
            CallOrder = new List<string>();
            Supervisor = CreateTestProbe();
            
            // Create objects passed into sut
            Props = Props.Create<BlackHoleActor>();
            ExpectedChildrenCount = TestUtils.Create<int>();
            Message = TestUtils.Create<object>();
            Recipient = RecipientMock.Object;
            ChildName = TestUtils.Create<string>();

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

            RecipientMock
                .Setup(waiter => waiter.Tell(Message, TestActor))
                .Callback(() => CallOrder.Add(nameof(IActorRef.Tell)));
            RecipientMock
                .Setup(waiter => waiter.Tell(Message, CreatedActor))
                .Callback(() => CallOrder.Add(nameof(IActorRef.Tell) + "Sender"));

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
            ChildWaiterMock = null;
            TestProbeCreatorMock = null;
            ResolvedTestProbeStoreMock = null;
            RecipientMock = null;
            CallOrder = null;
            Supervisor = null;
            Props = null;
            ExpectedChildrenCount = default(int);
            Message = null;
            Recipient = null;
            ChildName = null;
            CreatedActor = null;
            CreatedActorNoProps = null;
        }

        public TestProbeResolver CreateTestProbeResolver(TestProbeResolverSettings settings) => 
            new TestProbeResolver(
                DependencyResolverAdderMock.Object,
                SutCreatorMock.Object,
                ChildWaiterMock.Object,
                TestProbeCreatorMock.Object,
                this,
                settings);
    }
}