using System;
using System.Collections.Immutable;
using Akka.Actor;
using Akka.TestKit.TestActors;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract;
using FluentAssertions;
using Moq;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.ConcreteResolverTests
{
    internal class ResolveChild : TestBase
    {
        [Fact]
        public void ConcreteResolver_ResolveChildInSettings_ReturnsActor()
        {
            //arrange
            CreateConcreteResolver(ImmutableDictionary<Type, Func<ActorBase>>
                .Empty
                .Add(typeof(BlackHoleActor), () => CreatedActor.UnderlyingActor));

            //act
            ActorBase result = ResolveActor(typeof(BlackHoleActor));

            //assert
            result.Should().BeSameAs(CreatedActor.UnderlyingActor);
        }
        
        [Fact]
        public void ConcreteResolver_ResolveChildNotInSettings_ThrowsInvalidOperationException()
        {
            //arrange
            CreateConcreteResolver(ImmutableDictionary<Type, Func<ActorBase>>
                .Empty
                .Add(typeof(BlackHoleActor), () => CreatedActor.UnderlyingActor));

            //act
            Action act = () => ResolveActor(typeof(EchoActor));

            //assert
            act.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void ConcreteResolver_ResolveChildInSettings_ResolvesChildInWaiter()
        {
            //arrange
            CreateConcreteResolver(ImmutableDictionary<Type, Func<ActorBase>>
                .Empty
                .Add(typeof(BlackHoleActor), () => CreatedActor.UnderlyingActor));

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
            CreateConcreteResolver(ImmutableDictionary<Type, Func<ActorBase>>
                .Empty
                .Add(typeof(BlackHoleActor), () =>
                {
                    CallOrder.Add("Resolver");
                    return CreatedActor.UnderlyingActor;
                }));

            //act
            ResolveActor(typeof(BlackHoleActor));

            //assert
            CallOrder.Should().ContainInOrder("Resolver", nameof(IChildWaiter.ResolvedChild));
        }
    }
}