﻿using System;
using Akka.Actor;
using ConnelHooley.AkkaTestingHelpers.Helpers.Abstract;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;
using FluentAssertions;
using Moq;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.TestProbeDependencyResolverAdderTests
{
    public class Add : TestBase
    {
        #region Null tests
        [Fact]
        public void TestProbeDependencyResolverAdder_AddWithNullDependencyResolverAdder_ThrowsArgumentNullException()
        {
            //arrange
            TestProbeDependencyResolverAdder sut = CreateTestProbeDependencyResolverAdder();

            //act
            Action act = () =>sut.Add(
                null,
                TestProbeChildActorCreator,
                TestProbeCreator,
                ResolvedTestProbeStore,
                ChildWaiter,
                this,
                Handlers);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeDependencyResolverAdder_AddWithNullTestProbeChildActorCreator_ThrowsArgumentNullException()
        {
            //arrange
            TestProbeDependencyResolverAdder sut = CreateTestProbeDependencyResolverAdder();

            //act
            Action act = () =>sut.Add(
                DependencyResolverAdder,
                null,
                TestProbeCreator,
                ResolvedTestProbeStore,
                ChildWaiter,
                this,
                Handlers);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeDependencyResolverAdder_AddWithNullTestProbeCreator_ThrowsArgumentNullException()
        {
            //arrange
            TestProbeDependencyResolverAdder sut = CreateTestProbeDependencyResolverAdder();

            //act
            Action act = () => sut.Add(
                DependencyResolverAdder,
                TestProbeChildActorCreator,
                null,
                ResolvedTestProbeStore,
                ChildWaiter,
                this,
                Handlers);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeDependencyResolverAdder_AddWithNullResolvedTestProbeStore_ThrowsArgumentNullException()
        {
            //arrange
            TestProbeDependencyResolverAdder sut = CreateTestProbeDependencyResolverAdder();

            //act
            Action act = () =>sut.Add(
                DependencyResolverAdder,
                TestProbeChildActorCreator, 
                TestProbeCreator,
                null,
                ChildWaiter,
                this,
                Handlers);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeDependencyResolverAdder_AddWithNullChildWaiter_ThrowsArgumentNullException()
        {
            //arrange
            TestProbeDependencyResolverAdder sut = CreateTestProbeDependencyResolverAdder();

            //act
            Action act = () =>sut.Add(
                DependencyResolverAdder,
                TestProbeChildActorCreator,
                TestProbeCreator,
                ResolvedTestProbeStore,
                null,
                this,
                Handlers);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeDependencyResolverAdder_AddWithNullTestKit_ThrowsArgumentNullException()
        {
            //arrange
            TestProbeDependencyResolverAdder sut = CreateTestProbeDependencyResolverAdder();

            //act
            Action act = () =>sut.Add(
                DependencyResolverAdder,
                TestProbeChildActorCreator,
                TestProbeCreator,
                ResolvedTestProbeStore,
                ChildWaiter,
                null,
                Handlers);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeDependencyResolverAdder_AddWithNullHandlers_ThrowsArgumentNullException()
        {
            //arrange
            TestProbeDependencyResolverAdder sut = CreateTestProbeDependencyResolverAdder();

            //act
            Action act = () =>sut.Add(
                DependencyResolverAdder,
                TestProbeChildActorCreator,
                TestProbeCreator,
                ResolvedTestProbeStore,
                ChildWaiter,
                this,
                null);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeDependencyResolverAdder_AddWithAllNulls_ThrowsArgumentNullException()
        {
            //arrange
            TestProbeDependencyResolverAdder sut = CreateTestProbeDependencyResolverAdder();

            //act
            Action act = () => sut.Add(
                null,
                null,
                null,
                null,
                null,
                null,
                null);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }
        #endregion

        [Fact]
        public void TestProbeDependencyResolverAdder_Add_AddedFactoryReturnsActorWhenCalledWithHandlers()
        {
            //arrange
            TestProbeDependencyResolverAdder sut = CreateTestProbeDependencyResolverAdder();

            //act
            sut.Add(
                DependencyResolverAdder,
                TestProbeChildActorCreator,
                TestProbeCreator,
                ResolvedTestProbeStore,
                ChildWaiter,
                this,
                Handlers);

            //assert
            ActorBase result = ActorFactory(ActorWithHandlersType);
            result.Should().BeSameAs(ResolvedActorWithHandlers);
        }

        [Fact]
        public void TestProbeDependencyResolverAdder_Add_AddedFactoryReturnsActorWhenCalledWithoutHandlers()
        {
            //arrange
            TestProbeDependencyResolverAdder sut = CreateTestProbeDependencyResolverAdder();

            //act
            sut.Add(
                DependencyResolverAdder,
                TestProbeChildActorCreator,
                TestProbeCreator,
                ResolvedTestProbeStore,
                ChildWaiter,
                this,
                Handlers);

            //assert
            ActorBase result = ActorFactory(ActorWithoutHandlersType);
            result.Should().BeSameAs(ResolvedActorWithoutHandlers);
        }
        
        [Fact]
        public void TestProbeDependencyResolverAdder_Add_AddedFactoryStoresTestProbeChildActorWhenCalledWithHandlers()
        {
            //arrange
            TestProbeDependencyResolverAdder sut = CreateTestProbeDependencyResolverAdder();

            //act
            sut.Add(
                DependencyResolverAdder,
                TestProbeChildActorCreator,
                TestProbeCreator,
                ResolvedTestProbeStore,
                ChildWaiter,
                this,
                Handlers);

            //assert
            ActorFactory(ActorWithHandlersType);
            ResolvedTestProbeStoreMock.Verify(
                store => store.ResolveProbe(
                    ResolvedActorPathWithHandlers,
                    ActorWithHandlersType,
                    ResolvedTestProbeWithHandlers,
                    ResolvedSupervisorStrategyWithHandlers));
        }
        
        [Fact]
        public void TestProbeDependencyResolverAdder_Add_AddedFactoryStoresTestProbeChildActorWhenCalledWithoutHandlers()
        {
            //arrange
            TestProbeDependencyResolverAdder sut = CreateTestProbeDependencyResolverAdder();

            //act
            sut.Add(
                DependencyResolverAdder,
                TestProbeChildActorCreator,
                TestProbeCreator,
                ResolvedTestProbeStore,
                ChildWaiter,
                this,
                Handlers);

            //assert
            ActorFactory(ActorWithoutHandlersType);
            ResolvedTestProbeStoreMock.Verify(
                store => store.ResolveProbe(
                    ResolvedActorPathWithoutHandlers,
                    ActorWithoutHandlersType,
                    ResolvedTestProbeWithoutHandlers,
                    ResolvedSupervisorStrategyWithoutHandlers));
        }
        
        [Fact]
        public void TestProbeDependencyResolverAdder_Add_AddedFactoryResolvesChildWaiterWhenCalledWithHandlers()
        {
            //arrange
            TestProbeDependencyResolverAdder sut = CreateTestProbeDependencyResolverAdder();

            //act
            sut.Add(
                DependencyResolverAdder,
                TestProbeChildActorCreator,
                TestProbeCreator,
                ResolvedTestProbeStore,
                ChildWaiter,
                this,
                Handlers);

            //assert
            ActorFactory(ActorWithHandlersType);
            ChildWaiterMock.Verify(
                waiter => waiter.ResolveEvent(),
                Times.Once);
        }

        [Fact]
        public void TestProbeDependencyResolverAdder_Add_AddedFactoryResolvesChildWaiterWhenCalledWithoutHandlers()
        {
            //arrange
            TestProbeDependencyResolverAdder sut = CreateTestProbeDependencyResolverAdder();

            //act
            sut.Add(
                DependencyResolverAdder,
                TestProbeChildActorCreator,
                TestProbeCreator,
                ResolvedTestProbeStore,
                ChildWaiter,
                this,
                Handlers);

            //assert
            ActorFactory(ActorWithoutHandlersType);
            ChildWaiterMock.Verify(
                waiter => waiter.ResolveEvent(),
                Times.Once);
        }

        [Fact]
        public void TestProbeDependencyResolverAdder_Add_AddedFactoryStoresChildInStoreBeforeResolvingInWaiterWhenCalledWithHandlers()
        {
            //arrange
            TestProbeDependencyResolverAdder sut = CreateTestProbeDependencyResolverAdder();

            //act
            sut.Add(
                DependencyResolverAdder,
                TestProbeChildActorCreator,
                TestProbeCreator,
                ResolvedTestProbeStore,
                ChildWaiter,
                this,
                Handlers);

            //assert
            ActorFactory(ActorWithHandlersType);
            CallOrder.Should().Equal(
                nameof(IResolvedTestProbeStore.ResolveProbe),
                nameof(IWaiter.ResolveEvent));
        }

        [Fact]
        public void TestProbeDependencyResolverAdder_Add_AddedFactoryStoresChildInStoreBeforeResolvingInWaiterWhenCalledWithoutHandlers()
        {
            //arrange
            TestProbeDependencyResolverAdder sut = CreateTestProbeDependencyResolverAdder();

            //act
            sut.Add(
                DependencyResolverAdder,
                TestProbeChildActorCreator,
                TestProbeCreator,
                ResolvedTestProbeStore,
                ChildWaiter,
                this,
                Handlers);

            //assert
            ActorFactory(ActorWithoutHandlersType);
            CallOrder.Should().Equal(
                nameof(IResolvedTestProbeStore.ResolveProbe),
                nameof(IWaiter.ResolveEvent));
        }
    }
}