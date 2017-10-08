using System;
using System.Collections.Immutable;
using Akka.Actor;
using Akka.TestKit.TestActors;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract;
using FluentAssertions;
using Moq;
using Xunit;
using EitherFactory = Akka.Util.Either<
    System.Func<Akka.Actor.ActorBase>,
    ConnelHooley.AkkaTestingHelpers.DI.IRegisterableActorFake>;
using ConcreteSetting = Akka.Util.Left<
    System.Func<Akka.Actor.ActorBase>,
    ConnelHooley.AkkaTestingHelpers.DI.IRegisterableActorFake>;
using FakeSetting = Akka.Util.Right<
    System.Func<Akka.Actor.ActorBase>,
    ConnelHooley.AkkaTestingHelpers.DI.IRegisterableActorFake>;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.ConcreteResolverTests
{
    public class ResolveChild : TestBase
    {
        [Fact]
        public void ConcreteResolver_ResolveChildInSettings_ReturnsActor()
        {
            //arrange
            CreateConcreteResolver(ImmutableDictionary<Type, EitherFactory>
                .Empty
                .Add(typeof(BlackHoleActor), new ConcreteSetting(() => ConcreteActorInstance)));

            //act
            ActorBase result = ResolveActor(typeof(BlackHoleActor));

            //assert
            result.Should().BeSameAs(ConcreteActorInstance);
        }

        [Fact]
        public void ConcreteResolver_ResolveFakeChildInSettings_ReturnsActor()
        {
            //arrange
            CreateConcreteResolver(ImmutableDictionary<Type, EitherFactory>
                .Empty
                .Add(typeof(BlackHoleActor), new FakeSetting(RegisterableFakeActorMock.Object)));

            //act
            ActorBase result = ResolveActor(typeof(BlackHoleActor));

            //assert
            result.Should().BeSameAs(ResolvedFakeActorInstance);
        }
        
        [Fact]
        public void ConcreteResolver_ResolveFakeChildInSettings_RegistersFakeActor()
        {
            //arrange
            CreateConcreteResolver(ImmutableDictionary<Type, EitherFactory>
                .Empty
                .Add(typeof(BlackHoleActor), new FakeSetting(RegisterableFakeActorMock.Object)));

            //act
            ResolveActor(typeof(BlackHoleActor));

            //assert
            RegisterableFakeActorMock.Verify(
                actor => actor.RegisterActor(TestProbeActorMock.Object), 
                Times.Once);
        }

        [Fact]
        public void ConcreteResolver_ResolveChildNotInSettings_ThrowsInvalidOperationException()
        {
            //arrange
            CreateConcreteResolver(ImmutableDictionary<Type, EitherFactory>
                .Empty
                .Add(typeof(BlackHoleActor), new ConcreteSetting(() => ConcreteActorInstance)));

            //act
            Action act = () => ResolveActor(typeof(EchoActor));

            //assert
            act.ShouldThrow<InvalidOperationException>();
        }
        
        [Fact]
        public void ConcreteResolver_ResolveFakeChildNotInSettings_ThrowsInvalidOperationException()
        {
            //arrange
            CreateConcreteResolver(ImmutableDictionary<Type, EitherFactory>
                .Empty
                .Add(typeof(BlackHoleActor), new FakeSetting(RegisterableFakeActorMock.Object)));

            //act
            Action act = () => ResolveActor(typeof(EchoActor));

            //assert
            act.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void ConcreteResolver_ResolveChildInSettings_ResolvesChildInWaiter()
        {
            //arrange
            CreateConcreteResolver(ImmutableDictionary<Type, EitherFactory>
                .Empty
                .Add(typeof(BlackHoleActor), new ConcreteSetting(() => ConcreteActorInstance)));

            //act
            ResolveActor(typeof(BlackHoleActor));

            //assert
            ChildWaiterMock.Verify(
                waiter => waiter.ResolvedChild(), 
                Times.Once);
        }

        [Fact]
        public void ConcreteResolver_ResolveChildInSettings_ResolvesChildInWaiterAfterCallingFactory()
        {
            //arrange
            CreateConcreteResolver(ImmutableDictionary<Type, EitherFactory>
                .Empty
                .Add(typeof(BlackHoleActor), new ConcreteSetting(() =>
                {
                    CallOrder.Add("Resolver");
                    return ConcreteActorInstance;
                })));

            //act
            ResolveActor(typeof(BlackHoleActor));

            //assert
            CallOrder.Should().ContainInOrder("Resolver", nameof(IChildWaiter.ResolvedChild));
        }

        [Fact]
        public void ConcreteResolver_ResolveFakeChildInSettings_ResolvesChildInWaiter()
        {
            //arrange
            CreateConcreteResolver(ImmutableDictionary<Type, EitherFactory>
                .Empty
                .Add(typeof(BlackHoleActor), new FakeSetting(RegisterableFakeActorMock.Object)));

            //act
            ResolveActor(typeof(BlackHoleActor));

            //assert
            ChildWaiterMock.Verify(
                waiter => waiter.ResolvedChild(),
                Times.Once);
        }

        [Fact]
        public void ConcreteResolver_ResolveFakeChildInSettings_ResolvesChildInWaiterAfterRegisteringFakeActor()
        {
            //arrange
            CreateConcreteResolver(ImmutableDictionary<Type, EitherFactory>
                .Empty
                .Add(typeof(BlackHoleActor), new FakeSetting(RegisterableFakeActorMock.Object)));

            //act
            ResolveActor(typeof(BlackHoleActor));
            
            //assert
            CallOrder.Should().ContainInOrder(nameof(IRegisterableActorFake.RegisterActor), nameof(IChildWaiter.ResolvedChild));
        }
    }
}