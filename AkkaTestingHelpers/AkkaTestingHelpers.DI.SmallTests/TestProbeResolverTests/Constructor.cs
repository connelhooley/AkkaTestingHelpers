using System;
using Akka.Actor;
using Akka.TestKit;
using FluentAssertions;
using Moq;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.TestProbeResolverTests
{
    public class Constructor : TestBase
    {
        #region Null tests
        [Fact]
        public void TestProbeResolver_ConstructorWithNullSutCreator_ThrowsArgumentNullException()
        {
            //act
            Action act = () => new TestProbeResolver(
                null,
                ChildTeller,
                ChildWaiter,
                DependencyResolverAdder,
                TestProbeDependencyResolverAdder,
                TestProbeCreator,
                ResolvedTestProbeStore,
                TestProbeActorCreator,
                TestProbeHandlersMapper,
                this,
                Handlers);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeResolver_ConstructorWithNullChildTeller_ThrowsArgumentNullException()
        {
            //act
            Action act = () => new TestProbeResolver(
                SutCreator,
                null,
                ChildWaiter,
                DependencyResolverAdder,
                TestProbeDependencyResolverAdder,
                TestProbeCreator,
                ResolvedTestProbeStore,
                TestProbeActorCreator,
                TestProbeHandlersMapper,
                this,
                Handlers);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeResolver_ConstructorWithNullChildWaiter_ThrowsArgumentNullException()
        {
            //act
            Action act = () => new TestProbeResolver(
                SutCreator,
                ChildTeller,
                null,
                DependencyResolverAdder,
                TestProbeDependencyResolverAdder,
                TestProbeCreator,
                ResolvedTestProbeStore,
                TestProbeActorCreator,
                TestProbeHandlersMapper,
                this,
                Handlers);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeResolver_ConstructorWithNullDependencyResolverAdder_ThrowsArgumentNullException()
        {
            //act
            Action act = () => new TestProbeResolver(
                SutCreator,
                ChildTeller,
                ChildWaiter,
                null,
                TestProbeDependencyResolverAdder,
                TestProbeCreator,
                ResolvedTestProbeStore,
                TestProbeActorCreator,
                TestProbeHandlersMapper,
                this,
                Handlers);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeResolver_ConstructorWithNullTestProbeDependencyResolverAdder_ThrowsArgumentNullException()
        {
            //act
            Action act = () => new TestProbeResolver(
                SutCreator,
                ChildTeller,
                ChildWaiter,
                DependencyResolverAdder,
                null,
                TestProbeCreator,
                ResolvedTestProbeStore,
                TestProbeActorCreator,
                TestProbeHandlersMapper,
                this,
                Handlers);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeResolver_ConstructorWithNullTestProbeCreator_ThrowsArgumentNullException()
        {
            //act
            Action act = () => new TestProbeResolver(
                SutCreator,
                ChildTeller,
                ChildWaiter,
                DependencyResolverAdder,
                TestProbeDependencyResolverAdder,
                null,
                ResolvedTestProbeStore,
                TestProbeActorCreator,
                TestProbeHandlersMapper,
                this,
                Handlers);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeResolver_ConstructorWithNullResolvedTestProbeStore_ThrowsArgumentNullException()
        {
            //act
            Action act = () => new TestProbeResolver(
                SutCreator,
                ChildTeller,
                ChildWaiter,
                DependencyResolverAdder,
                TestProbeDependencyResolverAdder,
                TestProbeCreator,
                null,
                TestProbeActorCreator,
                TestProbeHandlersMapper,
                this,
                Handlers);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeResolver_ConstructorWithNullTestProbeActorCreator_ThrowsArgumentNullException()
        {
            //act
            Action act = () => new TestProbeResolver(
                SutCreator,
                ChildTeller,
                ChildWaiter,
                DependencyResolverAdder,
                TestProbeDependencyResolverAdder,
                TestProbeCreator,
                ResolvedTestProbeStore,
                null,
                TestProbeHandlersMapper,
                this,
                Handlers);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeResolver_ConstructorWithNullTestProbeHandlersMapper_ThrowsArgumentNullException()
        {
            //act
            Action act = () => new TestProbeResolver(
                SutCreator,
                ChildTeller,
                ChildWaiter,
                DependencyResolverAdder,
                TestProbeDependencyResolverAdder,
                TestProbeCreator,
                ResolvedTestProbeStore,
                TestProbeActorCreator,
                null,
                this,
                Handlers);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeResolver_ConstructorWithNullTestKit_ThrowsArgumentNullException()
        {
            //act
            Action act = () => new TestProbeResolver(
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
                Handlers);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeResolver_ConstructorWithNullHandlers_ThrowsArgumentNullException()
        {
            //act
            Action act = () => new TestProbeResolver(
                SutCreator,
                ChildTeller,
                ChildWaiter,
                DependencyResolverAdder,
                TestProbeDependencyResolverAdder,
                TestProbeCreator,
                ResolvedTestProbeStore,
                TestProbeActorCreator,
                TestProbeHandlersMapper,
                this,
                null);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeResolver_ConstructorWithAllNulls_ThrowsArgumentNullException()
        {
            //act
            Action act = () => new TestProbeResolver(
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
                null);

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