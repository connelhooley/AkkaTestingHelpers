using System;
using Akka.Actor;
using Akka.TestKit;
using FluentAssertions;
using Moq;
using Xunit;
// ReSharper disable ObjectCreationAsStatement

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.UnitTestFrameworkTests
{
    public class Constructor : TestBase
    {
        #region Null tests
        [Fact]
        public void TestProbeResolver_ConstructorWithNullSutCreator_ThrowsArgumentNullException()
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
                Handlers,
                this,
                Props,
                ExpectedChildCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeResolver_ConstructorWithNullChildTeller_ThrowsArgumentNullException()
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
                Handlers,
                this,
                Props,
                ExpectedChildCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeResolver_ConstructorWithNullChildWaiter_ThrowsArgumentNullException()
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
                Handlers,
                this,
                Props,
                ExpectedChildCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeResolver_ConstructorWithNullDependencyResolverAdder_ThrowsArgumentNullException()
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
                Handlers,
                this,
                Props,
                ExpectedChildCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeResolver_ConstructorWithNullTestProbeDependencyResolverAdder_ThrowsArgumentNullException()
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
                Handlers,
                this,
                Props,
                ExpectedChildCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeResolver_ConstructorWithNullTestProbeCreator_ThrowsArgumentNullException()
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
                Handlers,
                this,
                Props,
                ExpectedChildCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeResolver_ConstructorWithNullResolvedTestProbeStore_ThrowsArgumentNullException()
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
                Handlers,
                this,
                Props,
                ExpectedChildCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeResolver_ConstructorWithNullTestProbeActorCreator_ThrowsArgumentNullException()
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
                Handlers,
                this,
                Props,
                ExpectedChildCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeResolver_ConstructorWithNullTestProbeHandlersMapper_ThrowsArgumentNullException()
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
                Handlers,
                this,
                Props,
                ExpectedChildCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeResolver_ConstructorWithNullHandlers_ThrowsArgumentNullException()
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
                this,
                Props,
                ExpectedChildCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeResolver_ConstructorWithNullTestKit_ThrowsArgumentNullException()
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
                Handlers,
                null,
                Props,
                ExpectedChildCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeResolver_ConstructorWithNullProps_ThrowsArgumentNullException()
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
                Handlers,
                this,
                null,
                ExpectedChildCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeResolver_ConstructorWithAllNulls_ThrowsArgumentNullException()
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
                0);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }
        #endregion

        [Fact]
        public void TestProbeResolver_Constructor_AddsTestProbeDependencyResolverAdder()
        {
            //act
            CreateTestProbeResolver();

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
        public void TestProbeResolver_Constructor_DoesNotCallDependencyResolverAdder()
        {
            //act
            CreateTestProbeResolver();

            //assert
            DependencyResolverAdderMock.Verify(
                adder => adder.Add(
                    It.IsAny<TestKitBase>(),
                    It.IsAny<Func<Type, ActorBase>>()),
                Times.Never);
        }
    }
}