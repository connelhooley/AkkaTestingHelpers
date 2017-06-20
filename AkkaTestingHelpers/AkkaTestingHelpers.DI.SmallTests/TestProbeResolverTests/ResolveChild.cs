using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Akka.Actor;
using Akka.TestKit.TestActors;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract;
using FluentAssertions;
using Moq;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.TestProbeResolverTests
{
    public class ResolveChild : TestBase
    {
        [Fact]
        public void TestProbeResolver_Resolve_ReturnsActorFromTestProbeActor()
        {
            //arrange
            CreateTestProbeResolver();

            //act
            ActorBase result = ResolveActor(typeof(BlackHoleActor));

            //assert
            result.Should().BeSameAs(Actor);
        }
        
        [Fact]
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

        [Fact]
        public void TestProbeResolver_ResolveChild_ResolvesChildInStoreBeforeResolvingChildInWaiter()
        {
            //arrange
            CreateTestProbeResolver();

            //act
            ResolveActor(typeof(BlackHoleActor));

            //assert
            CallOrder.Should().ContainInOrder(nameof(IResolvedTestProbeStore.ResolveProbe), nameof(IChildWaiter.ResolvedChild));
        }

        [Fact]
        public void TestProbeResolver_ResolveChildWithoutHandlers_NoHandlersAreSet()
        {
            //arrange
            Type actor1Type = typeof(EchoActor);
            ImmutableDictionary<Type, Func<object, object>> actor1Handlers =
                ImmutableDictionary<Type, Func<object, object>>
                .Empty
                .Add(GenerateType(), o => TestUtils.Create<object>());
            Type actor2Type = typeof(BlackHoleActor);
            MappedHandlers = ImmutableDictionary<Type, ImmutableDictionary<Type, Func<object, object>>>
                .Empty
                .Add(actor1Type, actor1Handlers);
            CreateTestProbeResolver();

            //act
            ResolveActor(actor2Type);

            //assert
            TestProbeActorMock.Verify(
                actor => actor.SetHandlers(It.IsAny<IReadOnlyDictionary<Type, Func<object, object>>>()),
                Times.Never);
        }

        [Fact]
        public void TestProbeResolver_ResolveChildWithHandlers_CorrectHandlersAreSet()
        {
            //arrange
            Type actor1Type = typeof(EchoActor);
            ImmutableDictionary<Type, Func<object, object>> actor1Handlers =
                ImmutableDictionary<Type, Func<object, object>>
                .Empty
                .Add(GenerateType(), o => TestUtils.Create<object>());
            Type actor2Type = typeof(BlackHoleActor);
            ImmutableDictionary<Type, Func<object, object>> actor2Handlers =
                ImmutableDictionary<Type, Func<object, object>>
                .Empty
                .Add(GenerateType(), o => TestUtils.Create<object>());
            MappedHandlers = ImmutableDictionary<Type, ImmutableDictionary<Type, Func<object, object>>>
                .Empty
                .Add(actor1Type, actor1Handlers)
                .Add(actor2Type, actor2Handlers);
            CreateTestProbeResolver();
            
            //act
            ResolveActor(actor2Type);

            //assert
            TestProbeActorMock.Verify(
                actor => actor.SetHandlers(actor2Handlers),
                Times.Once);
        }
    }
}