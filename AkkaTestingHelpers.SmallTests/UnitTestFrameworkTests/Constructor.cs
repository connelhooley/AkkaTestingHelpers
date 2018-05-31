using System;
using Akka.Actor;
using Akka.TestKit;
using FluentAssertions;
using Moq;
using Xunit;

// ReSharper disable ObjectCreationAsStatement

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.UnitTestFrameworkTests
{
    public class Constructor : TestBase
    {
        #region Null tests
        [Fact]
        public void UnitTestFramework_ConstructorWithNullSutCreator_ThrowsArgumentNullException()
        {
            //act
            Action act = () => new UnitTestFramework<DummyActor>(
                null,
                ChildTeller,
                ChildWaiter,
                DependencyResolverAdder,
                TestProbeDependencyResolverAdder,
                TestProbeCreator,
                ResolvedTestProbeStore,
                TestProbeActorCreator,
                TestProbeHandlersMapper,
                SutSupervisorStrategyGetter,
                Handlers,
                this,
                Props,
                ExpectedChildCount);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void UnitTestFramework_ConstructorWithNullChildTeller_ThrowsArgumentNullException()
        {
            //act
            Action act = () => new UnitTestFramework<DummyActor>(
                SutCreator,
                null,
                ChildWaiter,
                DependencyResolverAdder,
                TestProbeDependencyResolverAdder,
                TestProbeCreator,
                ResolvedTestProbeStore,
                TestProbeActorCreator,
                TestProbeHandlersMapper,
                SutSupervisorStrategyGetter,
                Handlers,
                this,
                Props,
                ExpectedChildCount);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void UnitTestFramework_ConstructorWithNullChildWaiter_ThrowsArgumentNullException()
        {
            //act
            Action act = () => new UnitTestFramework<DummyActor>(
                SutCreator,
                ChildTeller,
                null,
                DependencyResolverAdder,
                TestProbeDependencyResolverAdder,
                TestProbeCreator,
                ResolvedTestProbeStore,
                TestProbeActorCreator,
                TestProbeHandlersMapper,
                SutSupervisorStrategyGetter,
                Handlers,
                this,
                Props,
                ExpectedChildCount);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void UnitTestFramework_ConstructorWithNullDependencyResolverAdder_ThrowsArgumentNullException()
        {
            //act
            Action act = () => new UnitTestFramework<DummyActor>(
                SutCreator,
                ChildTeller,
                ChildWaiter,
                null,
                TestProbeDependencyResolverAdder,
                TestProbeCreator,
                ResolvedTestProbeStore,
                TestProbeActorCreator,
                TestProbeHandlersMapper,
                SutSupervisorStrategyGetter,
                Handlers,
                this,
                Props,
                ExpectedChildCount);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void UnitTestFramework_ConstructorWithNullTestProbeDependencyResolverAdder_ThrowsArgumentNullException()
        {
            //act
            Action act = () => new UnitTestFramework<DummyActor>(
                SutCreator,
                ChildTeller,
                ChildWaiter,
                DependencyResolverAdder,
                null,
                TestProbeCreator,
                ResolvedTestProbeStore,
                TestProbeActorCreator,
                TestProbeHandlersMapper,
                SutSupervisorStrategyGetter,
                Handlers,
                this,
                Props,
                ExpectedChildCount);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void UnitTestFramework_ConstructorWithNullTestProbeCreator_ThrowsArgumentNullException()
        {
            //act
            Action act = () => new UnitTestFramework<DummyActor>(
                SutCreator,
                ChildTeller,
                ChildWaiter,
                DependencyResolverAdder,
                TestProbeDependencyResolverAdder,
                null,
                ResolvedTestProbeStore,
                TestProbeActorCreator,
                TestProbeHandlersMapper,
                SutSupervisorStrategyGetter,
                Handlers,
                this,
                Props,
                ExpectedChildCount);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void UnitTestFramework_ConstructorWithNullResolvedTestProbeStore_ThrowsArgumentNullException()
        {
            //act
            Action act = () => new UnitTestFramework<DummyActor>(
                SutCreator,
                ChildTeller,
                ChildWaiter,
                DependencyResolverAdder,
                TestProbeDependencyResolverAdder,
                TestProbeCreator,
                null,
                TestProbeActorCreator,
                TestProbeHandlersMapper,
                SutSupervisorStrategyGetter,
                Handlers,
                this,
                Props,
                ExpectedChildCount);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void UnitTestFramework_ConstructorWithNullTestProbeActorCreator_ThrowsArgumentNullException()
        {
            //act
            Action act = () => new UnitTestFramework<DummyActor>(
                SutCreator,
                ChildTeller,
                ChildWaiter,
                DependencyResolverAdder,
                TestProbeDependencyResolverAdder,
                TestProbeCreator,
                ResolvedTestProbeStore,
                null,
                TestProbeHandlersMapper,
                SutSupervisorStrategyGetter,
                Handlers,
                this,
                Props,
                ExpectedChildCount);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void UnitTestFramework_ConstructorWithNullTestProbeHandlersMapper_ThrowsArgumentNullException()
        {
            //act
            Action act = () => new UnitTestFramework<DummyActor>(
                SutCreator,
                ChildTeller,
                ChildWaiter,
                DependencyResolverAdder,
                TestProbeDependencyResolverAdder,
                TestProbeCreator,
                ResolvedTestProbeStore,
                TestProbeActorCreator,
                null,
                SutSupervisorStrategyGetter,
                Handlers,
                this,
                Props,
                ExpectedChildCount);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void UnitTestFramework_ConstructorWithNullSutSupervisorStrategyGetter_ThrowsArgumentNullException()
        {
            //act
            Action act = () => new UnitTestFramework<DummyActor>(
                SutCreator,
                ChildTeller,
                ChildWaiter,
                DependencyResolverAdder,
                TestProbeDependencyResolverAdder,
                TestProbeCreator,
                ResolvedTestProbeStore,
                TestProbeActorCreator,
                TestProbeHandlersMapper,
                null,
                Handlers,
                this,
                Props,
                ExpectedChildCount);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void UnitTestFramework_ConstructorWithNullHandlers_ThrowsArgumentNullException()
        {
            //act
            Action act = () => new UnitTestFramework<DummyActor>(
                SutCreator,
                ChildTeller,
                ChildWaiter,
                DependencyResolverAdder,
                TestProbeDependencyResolverAdder,
                TestProbeCreator,
                ResolvedTestProbeStore,
                TestProbeActorCreator,
                TestProbeHandlersMapper,
                SutSupervisorStrategyGetter,
                null,
                this,
                Props,
                ExpectedChildCount);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void UnitTestFramework_ConstructorWithNullTestKit_ThrowsArgumentNullException()
        {
            //act
            Action act = () => new UnitTestFramework<DummyActor>(
                SutCreator,
                ChildTeller,
                ChildWaiter,
                DependencyResolverAdder,
                TestProbeDependencyResolverAdder,
                TestProbeCreator,
                ResolvedTestProbeStore,
                TestProbeActorCreator,
                TestProbeHandlersMapper,
                SutSupervisorStrategyGetter,
                Handlers,
                null,
                Props,
                ExpectedChildCount);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void UnitTestFramework_ConstructorWithNullProps_ThrowsArgumentNullException()
        {
            //act
            Action act = () => new UnitTestFramework<DummyActor>(
                SutCreator,
                ChildTeller,
                ChildWaiter,
                DependencyResolverAdder,
                TestProbeDependencyResolverAdder,
                TestProbeCreator,
                ResolvedTestProbeStore,
                TestProbeActorCreator,
                TestProbeHandlersMapper,
                SutSupervisorStrategyGetter,
                Handlers,
                this,
                null,
                ExpectedChildCount);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void UnitTestFramework_ConstructorWithAllNulls_ThrowsArgumentNullException()
        {
            //act
            Action act = () => new UnitTestFramework<DummyActor>(
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                0);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }
        #endregion

        [Fact]
        public void UnitTestFramework_Constructor_AddsTestProbeDependencyResolverAdder()
        {
            //act
            CreateUnitTestFramework();

            //assert
            TestProbeDependencyResolverAdderMock.Verify(
                adder => adder.Add(
                    DependencyResolverAdder,
                    TestProbeActorCreator,
                    TestProbeCreator,
                    ResolvedTestProbeStore,
                    ChildWaiter,
                    this,
                    MappedHandlers),
                Times.Once);
        }

        [Fact]
        public void UnitTestFramework_Constructor_DoesNotCallDependencyResolverAdder()
        {
            //act
            CreateUnitTestFramework();

            //assert
            DependencyResolverAdderMock.Verify(
                adder => adder.Add(
                    It.IsAny<TestKitBase>(),
                    It.IsAny<Func<Type, ActorBase>>()),
                Times.Never);
        }

        [Fact]
        public void UnitTestFramework_ConstructorWithPropsThatHaveA_DoesNotCallDependencyResolverAdder()
        {
            //act
            Action act = () => CreateUnitTestFramework(PropsWithSupervisorStrategy);

            //assert
            act
                .Should().Throw<InvalidOperationException>()
                .WithMessage("Do not use Prop objects with supervisor stratergies to create your SUT actor as you cannot garentee your actor will be created with this stratergy in production.");
        }
    }
}