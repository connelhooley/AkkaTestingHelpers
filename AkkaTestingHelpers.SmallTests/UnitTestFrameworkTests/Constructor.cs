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
                TellWaiter,
                ChildWaiter,
                ExceptionWaiter,
                DependencyResolverAdder,
                TestProbeDependencyResolverAdder,
                TestProbeCreator,
                ResolvedTestProbeStore,
                TestProbeChildActorCreator,
                TestProbeHandlersMapper,
                SutSupervisorStrategyGetter,
                ParentHandlers,
                ChildHandlers,
                this,
                Props,
                Decider,
                ExpectedChildCount);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void UnitTestFramework_ConstructorWithNullTellWaiter_ThrowsArgumentNullException()
        {
            //act
            Action act = () => new UnitTestFramework<DummyActor>(
                SutCreator,
                null,
                ChildWaiter,
                ExceptionWaiter,
                DependencyResolverAdder,
                TestProbeDependencyResolverAdder,
                TestProbeCreator,
                ResolvedTestProbeStore,
                TestProbeChildActorCreator,
                TestProbeHandlersMapper,
                SutSupervisorStrategyGetter,
                ParentHandlers,
                ChildHandlers,
                this,
                Props,
                Decider,
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
                TellWaiter,
                null,
                ExceptionWaiter,
                DependencyResolverAdder,
                TestProbeDependencyResolverAdder,
                TestProbeCreator,
                ResolvedTestProbeStore,
                TestProbeChildActorCreator,
                TestProbeHandlersMapper,
                SutSupervisorStrategyGetter,
                ParentHandlers,
                ChildHandlers,
                this,
                Props,
                Decider,
                ExpectedChildCount);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void UnitTestFramework_ConstructorWithNullExceptionWaiter_ThrowsArgumentNullException()
        {
            //act
            Action act = () => new UnitTestFramework<DummyActor>(
                SutCreator,
                TellWaiter,
                ChildWaiter,
                null,
                DependencyResolverAdder,
                TestProbeDependencyResolverAdder,
                TestProbeCreator,
                ResolvedTestProbeStore,
                TestProbeChildActorCreator,
                TestProbeHandlersMapper,
                SutSupervisorStrategyGetter,
                ParentHandlers,
                ChildHandlers,
                this,
                Props,
                Decider,
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
                TellWaiter,
                ChildWaiter,
                ExceptionWaiter,
                null,
                TestProbeDependencyResolverAdder,
                TestProbeCreator,
                ResolvedTestProbeStore,
                TestProbeChildActorCreator,
                TestProbeHandlersMapper,
                SutSupervisorStrategyGetter,
                ParentHandlers,
                ChildHandlers,
                this,
                Props,
                Decider,
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
                TellWaiter,
                ChildWaiter,
                ExceptionWaiter,
                DependencyResolverAdder,
                null,
                TestProbeCreator,
                ResolvedTestProbeStore,
                TestProbeChildActorCreator,
                TestProbeHandlersMapper,
                SutSupervisorStrategyGetter,
                ParentHandlers,
                ChildHandlers,
                this,
                Props,
                Decider,
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
                TellWaiter,
                ChildWaiter,
                ExceptionWaiter,
                DependencyResolverAdder,
                TestProbeDependencyResolverAdder,
                null,
                ResolvedTestProbeStore,
                TestProbeChildActorCreator,
                TestProbeHandlersMapper,
                SutSupervisorStrategyGetter,
                ParentHandlers,
                ChildHandlers,
                this,
                Props,
                Decider,
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
                TellWaiter,
                ChildWaiter,
                ExceptionWaiter,
                DependencyResolverAdder,
                TestProbeDependencyResolverAdder,
                TestProbeCreator,
                null,
                TestProbeChildActorCreator,
                TestProbeHandlersMapper,
                SutSupervisorStrategyGetter,
                ParentHandlers,
                ChildHandlers,
                this,
                Props,
                Decider,
                ExpectedChildCount);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void UnitTestFramework_ConstructorWithNullTestProbeChildActorCreator_ThrowsArgumentNullException()
        {
            //act
            Action act = () => new UnitTestFramework<DummyActor>(
                SutCreator,
                TellWaiter,
                ChildWaiter,
                ExceptionWaiter,
                DependencyResolverAdder,
                TestProbeDependencyResolverAdder,
                TestProbeCreator,
                ResolvedTestProbeStore,
                null,
                TestProbeHandlersMapper,
                SutSupervisorStrategyGetter,
                ParentHandlers,
                ChildHandlers,
                this,
                Props,
                Decider,
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
                TellWaiter,
                ChildWaiter,
                ExceptionWaiter,
                DependencyResolverAdder,
                TestProbeDependencyResolverAdder,
                TestProbeCreator,
                ResolvedTestProbeStore,
                TestProbeChildActorCreator,
                null,
                SutSupervisorStrategyGetter,
                ParentHandlers,
                ChildHandlers,
                this,
                Props,
                Decider,
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
                TellWaiter,
                ChildWaiter,
                ExceptionWaiter,
                DependencyResolverAdder,
                TestProbeDependencyResolverAdder,
                TestProbeCreator,
                ResolvedTestProbeStore,
                TestProbeChildActorCreator,
                TestProbeHandlersMapper,
                null,
                ParentHandlers,
                ChildHandlers,
                this,
                Props,
                Decider,
                ExpectedChildCount);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }


        [Fact]
        public void UnitTestFramework_ConstructorWithNullParentHandlers_ThrowsArgumentNullException()
        {
            //act
            Action act = () => new UnitTestFramework<DummyActor>(
                SutCreator,
                TellWaiter,
                ChildWaiter,
                ExceptionWaiter,
                DependencyResolverAdder,
                TestProbeDependencyResolverAdder,
                TestProbeCreator,
                ResolvedTestProbeStore,
                TestProbeChildActorCreator,
                TestProbeHandlersMapper,
                SutSupervisorStrategyGetter,
                null,
                ChildHandlers,
                this,
                Props,
                Decider,
                ExpectedChildCount);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void UnitTestFramework_ConstructorWithNullChildHandlers_ThrowsArgumentNullException()
        {
            //act
            Action act = () => new UnitTestFramework<DummyActor>(
                SutCreator,
                TellWaiter,
                ChildWaiter,
                ExceptionWaiter,
                DependencyResolverAdder,
                TestProbeDependencyResolverAdder,
                TestProbeCreator,
                ResolvedTestProbeStore,
                TestProbeChildActorCreator,
                TestProbeHandlersMapper,
                SutSupervisorStrategyGetter,
                ParentHandlers,
                null,
                this,
                Props,
                Decider,
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
                TellWaiter,
                ChildWaiter,
                ExceptionWaiter,
                DependencyResolverAdder,
                TestProbeDependencyResolverAdder,
                TestProbeCreator,
                ResolvedTestProbeStore,
                TestProbeChildActorCreator,
                TestProbeHandlersMapper,
                SutSupervisorStrategyGetter,
                ParentHandlers,
                ChildHandlers,
                null,
                Props,
                Decider,
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
                TellWaiter,
                ChildWaiter,
                ExceptionWaiter,
                DependencyResolverAdder,
                TestProbeDependencyResolverAdder,
                TestProbeCreator,
                ResolvedTestProbeStore,
                TestProbeChildActorCreator,
                TestProbeHandlersMapper,
                SutSupervisorStrategyGetter,
                ParentHandlers,
                ChildHandlers,
                this,
                null,
                Decider,
                ExpectedChildCount);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void UnitTestFramework_ConstructorWithNullDecider_ThrowsArgumentNullException()
        {
            //act
            Action act = () => new UnitTestFramework<DummyActor>(
                SutCreator,
                TellWaiter,
                ChildWaiter,
                ExceptionWaiter,
                DependencyResolverAdder,
                TestProbeDependencyResolverAdder,
                TestProbeCreator,
                ResolvedTestProbeStore,
                TestProbeChildActorCreator,
                TestProbeHandlersMapper,
                SutSupervisorStrategyGetter,
                ParentHandlers,
                ChildHandlers,
                this,
                Props,
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
                    TestProbeChildActorCreator,
                    TestProbeCreator,
                    ResolvedTestProbeStore,
                    ChildWaiter,
                    this,
                    MappedChildHandlers),
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