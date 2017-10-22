using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.Xunit2;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;
using Moq;
// ReSharper disable VirtualMemberCallInConstructor

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.ConcreteDependencyResolverAdderTests
{
    public class TestBase : TestKit
    {
        internal readonly Mock<IChildWaiter> ChildWaiterMock;
        internal readonly Mock<IDependencyResolverAdder> DependencyResolverAdderMock;

        internal readonly List<string> CallOrder;

        internal readonly IDependencyResolverAdder DependencyResolverAdder;
        internal readonly IChildWaiter ChildWaiter;
        internal readonly Type RegisteredActorType;
        internal readonly ActorBase ResolvedActor;
        internal readonly Type UnregisteredActorType;
        internal readonly ImmutableDictionary<Type, Func<ActorBase>> Factories;

        internal Func<Type, ActorBase> ActorFactory;
        
        public TestBase() : base(AkkaConfig.Config)
        {
            Func<Type> geneterateType = TestUtils.RandomTypeGenerator();

            // Create mocks
            ChildWaiterMock = new Mock<IChildWaiter>();
            DependencyResolverAdderMock = new Mock<IDependencyResolverAdder>();

            // Create objects used by mocks
            CallOrder = new List<string>();
            
            // Create objects passed into ActorFactory
            RegisteredActorType = geneterateType();
            UnregisteredActorType = geneterateType();

            // Create objects passed into sut methods
            DependencyResolverAdder = DependencyResolverAdderMock.Object;
            ChildWaiter = ChildWaiterMock.Object;
            ResolvedActor = new Mock<ActorBase>().Object;
            Factories = ImmutableDictionary<Type, Func<ActorBase>>
                .Empty
                .Add(RegisteredActorType, () => ResolvedActor);

            // Set up mocks
            ChildWaiterMock
                .Setup(waiter => waiter.ResolvedChild())
                .Callback(() => CallOrder.Add(nameof(IChildWaiter.ResolvedChild)));

            DependencyResolverAdderMock
                .Setup(adder => adder.Add(this, It.IsAny<Func<Type, ActorBase>>()))
                .Callback((TestKitBase testKit, Func<Type, ActorBase> actorFactory) => ActorFactory = actorFactory);
        }
        
        internal ConcreteDependencyResolverAdder CreateConcreteDependencyResolverAdder() => 
            new ConcreteDependencyResolverAdder();
    }
}