using System;
using System.Collections.Immutable;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.Xunit2;
using ConnelHooley.AkkaTestingHelpers.Helpers.Abstract;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;
using Moq;

// ReSharper disable VirtualMemberCallInConstructor

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.ConcreteDependencyResolverAdderTests
{
    public class TestBase : TestKit
    {
        internal readonly Mock<IDependencyResolverAdder> DependencyResolverAdderMock;
        
        internal readonly IDependencyResolverAdder DependencyResolverAdder;
        
        internal readonly Type RegisteredActorType;
        internal ActorBase LastResolvedActor;
        internal readonly Type UnregisteredActorType;
        internal readonly ImmutableDictionary<Type, Func<ActorBase>> Factories;

        internal Func<Type, ActorBase> ActorFactory;
        
        public TestBase() : base(AkkaConfig.Config)
        {
            Func<Type> geneterateType = TestUtils.RandomTypeGenerator();

            // Create mocks
            DependencyResolverAdderMock = new Mock<IDependencyResolverAdder>();
            
            // Create objects passed into ActorFactory
            RegisteredActorType = geneterateType();
            UnregisteredActorType = geneterateType();

            // Create objects passed into sut constructor
            DependencyResolverAdder = DependencyResolverAdderMock.Object;
            
            // Create objects passed into sut methods
            LastResolvedActor = new Mock<ActorBase>().Object;
            Factories = ImmutableDictionary<Type, Func<ActorBase>>
                .Empty
                .Add(RegisteredActorType, () =>
                {
                    LastResolvedActor = new Mock<ActorBase>().Object;
                    return LastResolvedActor;
                });

            // Set up mocks
            DependencyResolverAdderMock
                .Setup(adder => adder.Add(this, It.IsAny<Func<Type, ActorBase>>()))
                .Callback((TestKitBase testKit, Func<Type, ActorBase> actorFactory) => ActorFactory = actorFactory);
        }
        
        internal ConcreteDependencyResolverAdder CreateConcreteDependencyResolverAdder() => 
            new ConcreteDependencyResolverAdder(DependencyResolverAdder);
    }
}