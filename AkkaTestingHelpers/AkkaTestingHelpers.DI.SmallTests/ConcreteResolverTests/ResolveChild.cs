using System;
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
            CreateConcreteResolver(ConcreteResolverSettings
                .Empty
                .Register(() => CreatedActor.UnderlyingActor));

            //act
            ActorBase result = ResolveActor(typeof(BlackHoleActor));

            //assert
            result.Should().BeSameAs(CreatedActor.UnderlyingActor);
        }

        [Test]
        public void ConcreteResolver_ResolveChildNotInSettings_ThrowsInvalidOperationException()
        {
            //arrange
            CreateConcreteResolver(ConcreteResolverSettings
                .Empty
                .Register(() => CreatedActor.UnderlyingActor));

            //act
            Action act = () => ResolveActor(typeof(EchoActor));

            //assert
            act.ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void ConcreteResolver_ResolveChildNotInSettings_ResolvesChildInWaiter()
        {
            //arrange
            CreateConcreteResolver(ConcreteResolverSettings
                .Empty
                .Register(() => CreatedActor.UnderlyingActor));

            //act
            ResolveActor(typeof(BlackHoleActor));

            //assert
            ChildWaiterMock.Verify(
                waiter => waiter.ResolvedChild(), 
                Times.Once);
        }

        [Test]
        public void ConcreteResolver_ResolveChildNotInSettings_ResolvesChildInWaiterAfterCallingFactory()
        {
            //arrange
            CreateConcreteResolver(ConcreteResolverSettings
                .Empty
                .Register(() =>
                {
                    CallOrder.Add("callback");
                    return CreatedActor.UnderlyingActor;
                }));

            //act
            ResolveActor(typeof(BlackHoleActor));

            //assert
            CallOrder.Should().ContainInOrder("callback", nameof(IChildWaiter.ResolvedChild));
        }
    }
}