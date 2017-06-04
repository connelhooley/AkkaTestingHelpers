using System;
using System.Collections.Generic;
using Akka.Actor;
using Akka.TestKit.TestActors;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.TestProbeResolverTests
{
    internal class ResolveChild : TestBase
    {
        [Test]
        public void TestProbeResolver_Resolve_ReturnsActorFromTestProbeActor()
        {
            //arrange
            CreateTestProbeResolver();

            //act
            ActorBase result = ResolveActor(typeof(BlackHoleActor));

            //assert
            result.Should().BeSameAs(Actor);
        }
        
        [Test]
        public void TestProbeResolver_ResolveChild_ResolvesChildInWaiter()
        {
            //arrange
            CreateTestProbeResolver();

            //act
            ResolveActor(typeof(BlackHoleActor));

            //assert
            ChildWaiterMock.Verify(
                waiter => waiter.ResolvedChild(),
                Times.Once);
        }

        [Test]
        public void TestProbeResolver_ResolveChild_ResolvesChildInStoreBeforeResolvingChildInWaiter()
        {
            //arrange
            CreateTestProbeResolver();

            //act
            ResolveActor(typeof(BlackHoleActor));

            //assert
            CallOrder.Should().ContainInOrder(nameof(IResolvedTestProbeStore.ResolveProbe), nameof(IChildWaiter.ResolvedChild));
        }

        [Test]
        public void TestProbeResolver_ResolveChildWithoutHandlers_HandlersAreNotSet()
        {
            //arrange
            Settings = TestProbeResolverSettings
                .Empty
                .RegisterHandler<EchoActor, string>(s => s);
            CreateTestProbeResolver();

            //act
            ResolveActor(typeof(BlackHoleActor));

            //assert
            TestProbeActorMock.Verify(
                actor => actor.SetHandlers(It.IsAny<IReadOnlyDictionary<Type, Func<object, object>>>()),
                Times.Never);
        }

        [Test]
        public void TestProbeResolver_ResolveChildWithHandlers_CorrectHandlersAreSet()
        {
            //todo
        }
    }
}