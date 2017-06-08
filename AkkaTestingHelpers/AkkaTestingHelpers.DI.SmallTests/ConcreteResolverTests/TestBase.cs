using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
        protected Mock<IChildTeller> ChildTellerMock;
        protected Mock<IChildWaiter> ChildWaiterMock;
        
        protected Func<Type, ActorBase> ResolveActor;
        protected List<string> CallOrder;
        
        protected Props Props;
        protected int ExpectedChildrenCount;
        protected object Message;
        protected IActorRef Recipient;
        protected IActorRef Sender;
        protected string ChildName;
        protected TestActorRef<BlackHoleActor> CreatedActor;
        protected TestActorRef<BlackHoleActor> CreatedActorNoProps;

        [SetUp]
        public void Setup()
        {
            // Create mocks
            DependencyResolverAdderMock = new Mock<IDependencyResolverAdder>();
            SutCreatorMock = new Mock<ISutCreator>();
            ChildTellerMock = new Mock<IChildTeller>();
            ChildWaiterMock = new Mock<IChildWaiter>();

            // Create objects used by mocks
            CallOrder = new List<string>();
            
            // Create objects passed into sut
            Props = Props.Create<BlackHoleActor>();
            ExpectedChildrenCount = TestUtils.Create<int>();
            Message = TestUtils.Create<object>();
            ChildName = TestUtils.Create<string>();
            Recipient = new Mock<IActorRef>().Object;
            Sender = new Mock<IActorRef>().Object;

            // Create objects returned by mocks
            CreatedActor = ActorOfAsTestActorRef<BlackHoleActor>();
            CreatedActorNoProps = ActorOfAsTestActorRef<BlackHoleActor>();

            // Set up mocks
            DependencyResolverAdderMock
                .Setup(adder => adder.Add(this, It.IsNotNull<Func<Type, ActorBase>>()))
                .Callback((TestKitBase testKit, Func<Type, ActorBase> resolveActor) => 
                    ResolveActor = resolveActor);
            
            SutCreatorMock
                .Setup(creator => creator.Create<BlackHoleActor>(ChildWaiterMock.Object, this, Props, ExpectedChildrenCount, null))
                .Returns(() => CreatedActor);
            SutCreatorMock
                .Setup(creator => creator.Create<BlackHoleActor>(ChildWaiterMock.Object, this, It.Is<Props>(props => !ReferenceEquals(props, Props) && props.Equals(Props.Create<BlackHoleActor>())), ExpectedChildrenCount, null))
                .Returns(() => CreatedActorNoProps);

            ChildWaiterMock
                .Setup(waiter => waiter.ResolvedChild())
                .Callback(() => CallOrder.Add(nameof(IChildWaiter.ResolvedChild)));
        }

        [TearDown]
        public void TearDown()
        {
            DependencyResolverAdderMock = null;
            SutCreatorMock = null;
            ChildTellerMock = null;
            ChildWaiterMock = null;
            CallOrder = null;
            Props = null;
            ExpectedChildrenCount = default(int);
            Message = null;
            Recipient = null;
            Sender = null;
            CreatedActor = null;
            CreatedActorNoProps = null;
        }

        public ConcreteResolver CreateConcreteResolver(ImmutableDictionary<Type, Func<ActorBase>> factories) => 
            new ConcreteResolver(
                DependencyResolverAdderMock.Object,
                SutCreatorMock.Object,
                ChildTellerMock.Object,
                ChildWaiterMock.Object,
                this,
                factories);
    }
}