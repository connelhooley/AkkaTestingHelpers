using System;
using System.Collections.Immutable;
using Akka.Actor;
using Akka.TestKit.TestActors;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.ConcreteResolverTests
{
    internal class ResolveChild : TestBase
    {
        [Test]
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
        
        [Test]
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

        [Test]
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

        [Test]
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