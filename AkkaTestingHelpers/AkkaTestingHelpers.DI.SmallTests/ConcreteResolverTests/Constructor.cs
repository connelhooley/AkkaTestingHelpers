using System;
using Akka.Actor;
using Akka.TestKit;
using FluentAssertions;
using Moq;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.ConcreteResolverTests
{
    public class Constructor : TestBase
    {
        #region Null tests
        [Fact]
        public void ConcreteResolver_ConstructorWithNullSutCreator_ThrowsArgumentNullException()
        {
            //act
            Action act = () => new ConcreteResolver(
                null,
                ChildTeller,
                ChildWaiter,
                DependencyResolverAdder,
                ConcreteDependencyResolverAdder,
                this,
                Factories);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void ConcreteResolver_ConstructorWithNullChildTeller_ThrowsArgumentNullException()
        {
            //act
            Action act = () => new ConcreteResolver(
                SutCreator,
                null,
                ChildWaiter,
                DependencyResolverAdder,
                ConcreteDependencyResolverAdder,
                this,
                Factories);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void ConcreteResolver_ConstructorWithNullConcreteResolver_ThrowsArgumentNullException()
        {
            //act
            Action act = () => new ConcreteResolver(
                SutCreator,
                ChildTeller,
                null,
                DependencyResolverAdder,
                ConcreteDependencyResolverAdder,
                this,
                Factories);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void ConcreteResolver_ConstructorWithNullDependencyResolverAdder_ThrowsArgumentNullException()
        {
            //act
            Action act = () => new ConcreteResolver(
                SutCreator,
                ChildTeller,
                ChildWaiter,
                null,
                ConcreteDependencyResolverAdder,
                this,
                Factories);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void ConcreteResolver_ConstructorWithNullConcreteDependencyResolverAdder_ThrowsArgumentNullException()
        {
            //act
            Action act = () => new ConcreteResolver(
                SutCreator,
                ChildTeller,
                ChildWaiter,
                DependencyResolverAdder,
                null,
                this,
                Factories);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void ConcreteResolver_ConstructorWithNullTestKit_ThrowsArgumentNullException()
        {
            //act
            Action act = () => new ConcreteResolver(
                SutCreator,
                ChildTeller,
                ChildWaiter,
                DependencyResolverAdder,
                ConcreteDependencyResolverAdder,
                null,
                Factories);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void ConcreteResolver_ConstructorWithNullFactories_ThrowsArgumentNullException()
        {
            //act
            Action act = () => new ConcreteResolver(
                SutCreator,
                ChildTeller,
                ChildWaiter,
                DependencyResolverAdder,
                ConcreteDependencyResolverAdder,
                this,
                null);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void ConcreteResolver_ConstructorWithAllNulls_ThrowsArgumentNullException()
        {
            //act
            Action act = () => new ConcreteResolver(
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
        public void ConcreteResolver_Constructor_AddsTestProbeDependencyResolverAdder()
        {
            //act
            CreateConcreteResolver();

            //assert
            ConcreteDependencyResolverAdderMock.Verify(
                adder => adder.Add(
                    DependencyResolverAdder,
                    ChildWaiter,
                    this,
                    Factories),
                Times.Once);
        }

        [Fact]
        public void ConcreteResolver_Constructor_DoesNotCallDependencyResolverAdder()
        {
            //act
            CreateConcreteResolver();

            //assert
            DependencyResolverAdderMock.Verify(
                adder => adder.Add(
                    It.IsAny<TestKitBase>(),
                    It.IsAny<Func<Type, ActorBase>>()),
                Times.Never);
        }
    }
}