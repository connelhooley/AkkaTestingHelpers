using System;
using System.Collections.Immutable;
using Akka.TestKit;
using Akka.TestKit.TestActors;
using FluentAssertions;
using Xunit;
using EitherSetting = Akka.Util.Either<
    System.Func<Akka.Actor.ActorBase>,
    ConnelHooley.AkkaTestingHelpers.DI.IRegisterableActorFake>;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.ConcreteResolverTests
{
    public class CreateSut : TestBase
    {
        [Fact]
        public void ConcreteResolver_CreateSutWithNullProps_ThrowsArgumentNullException()
        {
            //arrange   
            ConcreteResolver sut = CreateConcreteResolver(ImmutableDictionary<Type, EitherSetting>.Empty);

            //act
            Action act = () => sut.CreateSut<BlackHoleActor>(null, ExpectedChildrenCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void ConcreteResolver_CreateSut_ReturnsCreatedActor()
        {
            //arrange   
            ConcreteResolver sut = CreateConcreteResolver(ImmutableDictionary<Type, EitherSetting>.Empty);

            //act
            TestActorRef<BlackHoleActor> result = sut.CreateSut<BlackHoleActor>(Props, ExpectedChildrenCount);

            //assert
            result.Should().BeSameAs(CreatedActor);
        }
        
        [Fact]
        public void ConcreteResolver_CreateSutWithNoProps_ReturnsCreatedActor()
        {
            //arrange   
            ConcreteResolver sut = CreateConcreteResolver(ImmutableDictionary<Type, EitherSetting>.Empty);
            
            //act
            TestActorRef<BlackHoleActor> result = sut.CreateSut<BlackHoleActor>(ExpectedChildrenCount);

            //assert
            result.Should().BeSameAs(CreatedActorNoProps);
        }
    }
}