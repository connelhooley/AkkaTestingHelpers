using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.Xunit2;
using Akka.TestKit.TestActors;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract;
using Moq;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.ConcreteResolverTests
{
    public class TestBase: TestKit
    {
        internal Mock<IDependencyResolverAdder> DependencyResolverAdderMock;
        internal Mock<ISutCreator> SutCreatorMock;
        internal Mock<IChildTeller> ChildTellerMock;
        internal Mock<IChildWaiter> ChildWaiterMock;

        internal Func<Type, ActorBase> ResolveActor;
        internal List<string> CallOrder;
        
        internal Props Props;
        internal int ExpectedChildrenCount;
        internal object Message;
        internal IActorRef Recipient;
        internal IActorRef Sender;
        internal string ChildName;
        internal BlackHoleActor ConcreteActorInstance;
        internal BlackHoleActor ResolvedFakeActorInstance;
        internal TestActorRef<BlackHoleActor> CreatedActor;
        internal TestActorRef<BlackHoleActor> CreatedActorNoProps;

        public TestBase() : base(AkkaConfig.Config)
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
            ConcreteActorInstance = ActorOfAsTestActorRef<BlackHoleActor>().UnderlyingActor;

            // Create objects returned by mocks
            CreatedActor = ActorOfAsTestActorRef<BlackHoleActor>();
            CreatedActorNoProps = ActorOfAsTestActorRef<BlackHoleActor>();
            ResolvedFakeActorInstance = ActorOfAsTestActorRef<BlackHoleActor>().UnderlyingActor;

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
        
        internal ConcreteResolver CreateConcreteResolver(ImmutableDictionary<Type, Func<ActorBase>> factories) => 
            new ConcreteResolver(
                DependencyResolverAdderMock.Object,
                SutCreatorMock.Object,
                ChildTellerMock.Object,
                ChildWaiterMock.Object,
                this,
                factories);
    }
}