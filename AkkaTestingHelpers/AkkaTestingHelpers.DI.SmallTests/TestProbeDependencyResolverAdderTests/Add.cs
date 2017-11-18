using System;
using Akka.Actor;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;
using FluentAssertions;
using Moq;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.TestProbeDependencyResolverAdderTests
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
                TestProbeActorCreator,
                TestProbeCreator,
                ResolvedTestProbeStore,
                ChildWaiter,
                this,
                Handlers);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeDependencyResolverAdder_AddWithNullTestProbeActorCreator_ThrowsArgumentNullException()
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
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeDependencyResolverAdder_AddWithNullTestProbeCreator_ThrowsArgumentNullException()
        {
            //arrange
            TestProbeDependencyResolverAdder sut = CreateTestProbeDependencyResolverAdder();

            //act
            Action act = () => sut.Add(
                DependencyResolverAdder,
                TestProbeActorCreator,
                null,
                ResolvedTestProbeStore,
                ChildWaiter,
                this,
                Handlers);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeDependencyResolverAdder_AddWithNullResolvedTestProbeStore_ThrowsArgumentNullException()
        {
            //arrange
            TestProbeDependencyResolverAdder sut = CreateTestProbeDependencyResolverAdder();

            //act
            Action act = () =>sut.Add(
                DependencyResolverAdder,
                TestProbeActorCreator, 
                TestProbeCreator,
                null,
                ChildWaiter,
                this,
                Handlers);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeDependencyResolverAdder_AddWithNullChildWaiter_ThrowsArgumentNullException()
        {
            //arrange
            TestProbeDependencyResolverAdder sut = CreateTestProbeDependencyResolverAdder();

            //act
            Action act = () =>sut.Add(
                DependencyResolverAdder,
                TestProbeActorCreator,
                TestProbeCreator,
                ResolvedTestProbeStore,
                null,
                this,
                Handlers);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeDependencyResolverAdder_AddWithNullTestKit_ThrowsArgumentNullException()
        {
            //arrange
            TestProbeDependencyResolverAdder sut = CreateTestProbeDependencyResolverAdder();

            //act
            Action act = () =>sut.Add(
                DependencyResolverAdder,
                TestProbeActorCreator,
                TestProbeCreator,
                ResolvedTestProbeStore,
                ChildWaiter,
                null,
                Handlers);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeDependencyResolverAdder_AddWithNullHandlers_ThrowsArgumentNullException()
        {
            //arrange
            TestProbeDependencyResolverAdder sut = CreateTestProbeDependencyResolverAdder();

            //act
            Action act = () =>sut.Add(
                DependencyResolverAdder,
                TestProbeActorCreator,
                TestProbeCreator,
                ResolvedTestProbeStore,
                ChildWaiter,
                this,
                null);

            //assert
            act.ShouldThrow<ArgumentNullException>();
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
            act.ShouldThrow<ArgumentNullException>();
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
                TestProbeActorCreator,
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
                TestProbeActorCreator,
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
        public void TestProbeDependencyResolverAdder_Add_AddedFactoryStoresTestProbeActorWhenCalledWithHandlers()
        {
            //arrange
            TestProbeDependencyResolverAdder sut = CreateTestProbeDependencyResolverAdder();

            //act
            sut.Add(
                DependencyResolverAdder,
                TestProbeActorCreator,
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
        public void TestProbeDependencyResolverAdder_Add_AddedFactoryStoresTestProbeActorWhenCalledWithoutHandlers()
        {
            //arrange
            TestProbeDependencyResolverAdder sut = CreateTestProbeDependencyResolverAdder();

            //act
            sut.Add(
                DependencyResolverAdder,
                TestProbeActorCreator,
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
                TestProbeActorCreator,
                TestProbeCreator,
                ResolvedTestProbeStore,
                ChildWaiter,
                this,
                Handlers);

            //assert
            ActorFactory(ActorWithHandlersType);
            ChildWaiterMock.Verify(
                waiter => waiter.ResolvedChild(),
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
                TestProbeActorCreator,
                TestProbeCreator,
                ResolvedTestProbeStore,
                ChildWaiter,
                this,
                Handlers);

            //assert
            ActorFactory(ActorWithoutHandlersType);
            ChildWaiterMock.Verify(
                waiter => waiter.ResolvedChild(),
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
                TestProbeActorCreator,
                TestProbeCreator,
                ResolvedTestProbeStore,
                ChildWaiter,
                this,
                Handlers);

            //assert
            ActorFactory(ActorWithHandlersType);
            CallOrder.Should().Equal(
                nameof(IResolvedTestProbeStore.ResolveProbe),
                nameof(IChildWaiter.ResolvedChild));
        }

        [Fact]
        public void TestProbeDependencyResolverAdder_Add_AddedFactoryStoresChildInStoreBeforeResolvingInWaiterWhenCalledWithoutHandlers()
        {
            //arrange
            TestProbeDependencyResolverAdder sut = CreateTestProbeDependencyResolverAdder();

            //act
            sut.Add(
                DependencyResolverAdder,
                TestProbeActorCreator,
                TestProbeCreator,
                ResolvedTestProbeStore,
                ChildWaiter,
                this,
                Handlers);

            //assert
            ActorFactory(ActorWithoutHandlersType);
            CallOrder.Should().Equal(
                nameof(IResolvedTestProbeStore.ResolveProbe),
                nameof(IChildWaiter.ResolvedChild));
        }
    }
}