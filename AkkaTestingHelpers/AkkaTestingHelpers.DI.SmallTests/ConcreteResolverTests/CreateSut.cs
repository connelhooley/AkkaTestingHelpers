using System;
using System.Collections.Immutable;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.TestActors;
using FluentAssertions;
using NUnit.Framework;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.ConcreteResolverTests
{
    internal class CreateSut : TestBase
    {
        [Test]
        public void ConcreteResolver_CreateSutWithNullProps_ThrowsArgumentNullException()
        {
            //arrange   
            ConcreteResolver sut = CreateConcreteResolver(ImmutableDictionary<Type, Func<ActorBase>>.Empty);

            //act
            Action act = () => sut.CreateSut<BlackHoleActor>(null, ExpectedChildrenCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void ConcreteResolver_CreateSut_ReturnsCreatedActor()
        {
            //arrange   
            ConcreteResolver sut = CreateConcreteResolver(ImmutableDictionary<Type, Func<ActorBase>>.Empty);

            //act
            TestActorRef<BlackHoleActor> result = sut.CreateSut<BlackHoleActor>(Props, ExpectedChildrenCount);

            //assert
            result.Should().BeSameAs(CreatedActor);
        }
        
        [Test]
        public void ConcreteResolver_CreateSutWithNoProps_ReturnsCreatedActor()
        {
            //arrange   
            ConcreteResolver sut = CreateConcreteResolver(ImmutableDictionary<Type, Func<ActorBase>>.Empty);

            //act
            TestActorRef<BlackHoleActor> result = sut.CreateSut<BlackHoleActor>(ExpectedChildrenCount);

            //assert
            result.Should().BeSameAs(CreatedActorNoProps);
        }
    }
}