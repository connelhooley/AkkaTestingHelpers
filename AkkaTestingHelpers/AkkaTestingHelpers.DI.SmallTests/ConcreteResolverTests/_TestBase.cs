using System;
using System.Collections.Immutable;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.Xunit2;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract;
using Moq;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.ConcreteResolverTests
{
    public class TestBase: TestKit
    {
        internal Mock<IDependencyResolverAdder> DependencyResolverAdderMock;
        internal Mock<IConcreteDependencyResolverAdder> ConcreteDependencyResolverAdderMock;
        internal Mock<ISutCreator> SutCreatorMock;
        internal Mock<IChildTeller> ChildTellerMock;
        internal Mock<IChildWaiter> ChildWaiterMock;

        internal IDependencyResolverAdder DependencyResolverAdder;
        internal IConcreteDependencyResolverAdder ConcreteDependencyResolverAdder;
        internal ISutCreator SutCreator;
        internal IChildTeller ChildTeller;
        internal IChildWaiter ChildWaiter;
        internal ImmutableDictionary<Type, Func<ActorBase>> Factories;
        
        internal Props Props;
        internal int ExpectedChildCount;
        internal object Message;
        internal IActorRef Recipient;
        internal IActorRef Sender;
        internal string ChildName;
        protected TestActorRef<DummyActor> CreatedSutWithProps;
        protected TestActorRef<DummyActor> CreatedSutWithoutProps;

        public TestBase() : base(AkkaConfig.Config)
        {
            Func<Type> generateType = TestUtils.RandomTypeGenerator();

            // Create mocks
            DependencyResolverAdderMock = new Mock<IDependencyResolverAdder>();
            ConcreteDependencyResolverAdderMock =  new Mock<IConcreteDependencyResolverAdder>();
            SutCreatorMock = new Mock<ISutCreator>();
            ChildTellerMock = new Mock<IChildTeller>();
            ChildWaiterMock = new Mock<IChildWaiter>();
            
            // Create objects passed into sut constructor
            DependencyResolverAdder = DependencyResolverAdderMock.Object;
            ConcreteDependencyResolverAdder = ConcreteDependencyResolverAdderMock.Object;
            SutCreator = SutCreatorMock.Object;
            ChildTeller = ChildTellerMock.Object;
            ChildWaiter = ChildWaiterMock.Object;
            Factories = ImmutableDictionary<Type, Func<ActorBase>>
                .Empty
                .Add(generateType(), () => new Mock<ActorBase>().Object);

            // Create objects passed into sut methods
            Props = Props.Create<DummyActor>();
            ExpectedChildCount = TestUtils.Create<int>();
            Message = TestUtils.Create<object>();
            ChildName = TestUtils.Create<string>();
            Recipient = new Mock<IActorRef>().Object;
            Sender = new Mock<IActorRef>().Object;

            // Create objects returned by mocks
            CreatedSutWithProps = ActorOfAsTestActorRef<DummyActor>();
            CreatedSutWithoutProps = ActorOfAsTestActorRef<DummyActor>();

            // Set up mocks
            SutCreatorMock
                .Setup(creator => creator.Create<DummyActor>(
                    ChildWaiterMock.Object, 
                    this, 
                    Props, 
                    ExpectedChildCount, 
                    null))
                .Returns(() => CreatedSutWithProps);
            SutCreatorMock
                .Setup(creator => creator.Create<DummyActor>(
                    ChildWaiterMock.Object, 
                    this, It
                    .Is<Props>(props => !ReferenceEquals(props, Props) && props.Equals(Props.Create<DummyActor>())), 
                    ExpectedChildCount, 
                    null))
                .Returns(() => CreatedSutWithoutProps);
        }
        
        internal ConcreteResolver CreateConcreteResolver() => 
            new ConcreteResolver(
                SutCreator,
                ChildTeller,
                ChildWaiter,
                DependencyResolverAdder,
                ConcreteDependencyResolverAdder,
                this,
                Factories);

        protected class DummyActor : ReceiveActor { }
    }
}