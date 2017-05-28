using System;
using System.Collections.Generic;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.NUnit3;
using Akka.TestKit.TestActors;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract;
using Moq;
using NUnit.Framework;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.ConcreteResolverTests
{
    internal class TestBase: TestKit
    {
        protected Mock<IDependencyResolverAdder> DependencyResolverAdderMock;
        protected Mock<ISutCreator> SutCreatorMock;
        protected Mock<IChildWaiter> ChildWaiterMock;
        protected Mock<IActorRef> RecipientMock;
        
        protected Func<Type, ActorBase> ResolveActor;
        protected List<string> CallOrder;

        protected Props Props;
        protected int ExpectedChildrenCount;
        protected object Message;
        protected IActorRef Recipient;
        protected IActorRef Supervisor;
        protected TestActorRef<BlackHoleActor> CreatedActor;
        protected TestActorRef<BlackHoleActor> CreatedSupervisedActor;
        protected TestActorRef<BlackHoleActor> CreatedActorNoProps;
        protected TestActorRef<BlackHoleActor> CreatedSupervisedActorNoProps;

        [SetUp]
        public void Setup()
        {
            // Create mocks
            DependencyResolverAdderMock = new Mock<IDependencyResolverAdder>();
            SutCreatorMock = new Mock<ISutCreator>();
            ChildWaiterMock = new Mock<IChildWaiter>();
            RecipientMock = new Mock<IActorRef>();

            // Create objects used by mocks
            CallOrder = new List<string>();
            
            // Create objects put into mocks
            Props = Props.Create<BlackHoleActor>();
            ExpectedChildrenCount = TestUtils.Create<int>();
            Supervisor = CreateTestProbe();
            Message = TestUtils.Create<object>();
            Recipient = RecipientMock.Object;

            // Create objects returned by mocks
            CreatedActor = ActorOfAsTestActorRef<BlackHoleActor>();
            CreatedSupervisedActor = ActorOfAsTestActorRef<BlackHoleActor>(Supervisor);
            CreatedActorNoProps = ActorOfAsTestActorRef<BlackHoleActor>();
            CreatedSupervisedActorNoProps = ActorOfAsTestActorRef<BlackHoleActor>(Supervisor);

            // Set up mocks
            DependencyResolverAdderMock
                .Setup(adder => adder.Add(this, It.IsNotNull<Func<Type, ActorBase>>()))
                .Callback((TestKitBase testKit, Func<Type, ActorBase> resolveActor) =>
                {
                    ResolveActor = resolveActor;
                });
            
            SutCreatorMock
                .Setup(creator => creator.Create<BlackHoleActor>(ChildWaiterMock.Object, this, Props, ExpectedChildrenCount, null))
                .Returns(() => CreatedActor);
            SutCreatorMock
                .Setup(creator => creator.Create<BlackHoleActor>(ChildWaiterMock.Object, this, Props, ExpectedChildrenCount, Supervisor))
                .Returns(() => CreatedSupervisedActor);
            SutCreatorMock
                .Setup(creator => creator.Create<BlackHoleActor>(ChildWaiterMock.Object, this, It.Is<Props>(props => !ReferenceEquals(props, Props) && props.Equals(Props.Create<BlackHoleActor>())), ExpectedChildrenCount, null))
                .Returns(() => CreatedActorNoProps);
            SutCreatorMock
                .Setup(creator => creator.Create<BlackHoleActor>(ChildWaiterMock.Object, this, It.Is<Props>(props => !ReferenceEquals(props, Props) && props.Equals(Props.Create<BlackHoleActor>())), ExpectedChildrenCount, Supervisor))
                .Returns(() => CreatedSupervisedActorNoProps);

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
        }

        [TearDown]
        public void TearDown()
        {
            DependencyResolverAdderMock = null;
            SutCreatorMock = null;
            ChildWaiterMock = null;
            CallOrder = null;
            Props = null;
            ExpectedChildrenCount = default(int);
            Message = null;
            Recipient = null;
            Supervisor = null;
            CreatedActor = null;
            CreatedSupervisedActor = null;
            CreatedActorNoProps = null;
            CreatedSupervisedActorNoProps = null;
        }

        public ConcreteResolver CreateConcreteResolver(ConcreteResolverSettings settings) => 
            new ConcreteResolver(
                DependencyResolverAdderMock.Object,
                SutCreatorMock.Object,
                ChildWaiterMock.Object,
                this,
                settings);
    }
}