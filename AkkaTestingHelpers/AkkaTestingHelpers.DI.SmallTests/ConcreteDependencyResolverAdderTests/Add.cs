using System;
using Akka.Actor;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;
using FluentAssertions;
using Moq;
using Xunit;
// ReSharper disable EmptyGeneralCatchClause

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.ConcreteDependencyResolverAdderTests
{
    public class Add : TestBase
    {
        #region Null tests
        [Fact]
        public void ConcreteDependencyResolverAdder_AddWithNullDependencyResolverAdder_ThrowsArgumentNullException()
        {
            //arrange
            ConcreteDependencyResolverAdder sut = CreateConcreteDependencyResolverAdder();

            //act
            Action act = () =>sut.Add(
                null,
                ChildWaiter,
                this,
                Factories);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }
        
        [Fact]
        public void ConcreteDependencyResolverAdder_AddWithNullChildWaiter_ThrowsArgumentNullException()
        {
            //arrange
            ConcreteDependencyResolverAdder sut = CreateConcreteDependencyResolverAdder();

            //act
            Action act = () =>sut.Add(
                DependencyResolverAdder,
                null,
                this,
                Factories);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void ConcreteDependencyResolverAdder_AddWithNullTestKit_ThrowsArgumentNullException()
        {
            //arrange
            ConcreteDependencyResolverAdder sut = CreateConcreteDependencyResolverAdder();

            //act
            Action act = () =>sut.Add(
                DependencyResolverAdder,
                ChildWaiter,
                null,
                Factories);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void ConcreteDependencyResolverAdder_AddWithNullFactories_ThrowsArgumentNullException()
        {
            //arrange
            ConcreteDependencyResolverAdder sut = CreateConcreteDependencyResolverAdder();

            //act
            Action act = () =>sut.Add(
                DependencyResolverAdder,
                ChildWaiter,
                this,
                null);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void ConcreteDependencyResolverAdder_AddWithAllNulls_ThrowsArgumentNullException()
        {
            //arrange
            ConcreteDependencyResolverAdder sut = CreateConcreteDependencyResolverAdder();

            //act
            Action act = () => sut.Add(
                null,
                null,
                null,
                null);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }
        #endregion

        [Fact]
        public void ConcreteDependencyResolverAdder_Add_AddedFactoryReturnsActorWhenCalledWithRegisteredActor()
        {
            //arrange
            ConcreteDependencyResolverAdder sut = CreateConcreteDependencyResolverAdder();

            //act
            sut.Add(
                DependencyResolverAdder,
                ChildWaiter,
                this,
                Factories);

            //assert
            ActorBase result = ActorFactory(RegisteredActorType);
            result.Should().BeSameAs(ResolvedActor);
        }

        [Fact]
        public void ConcreteDependencyResolverAdder_Add_AddedFactoryReturnsActorWhenCalledWithUnregisteredActor()
        {
            //arrange
            ConcreteDependencyResolverAdder sut = CreateConcreteDependencyResolverAdder();

            //act
            sut.Add(
                DependencyResolverAdder,
                ChildWaiter,
                this,
                Factories);

            //assert
            Action act = () => ActorFactory(UnregisteredActorType);
            act
                .ShouldThrow<InvalidOperationException>()
                .WithMessage($"Please register the type '{UnregisteredActorType.Name}' in the settings");
        }
        
        [Fact]
        public void ConcreteDependencyResolverAdder_Add_AddedFactoryResolvesChildWaiterWhenCalledWithRegisteredActor()
        {
            //arrange
            ConcreteDependencyResolverAdder sut = CreateConcreteDependencyResolverAdder();

            //act
            sut.Add(
                DependencyResolverAdder,
                ChildWaiter,
                this,
                Factories);

            //assert
            ActorFactory(RegisteredActorType);
            ChildWaiterMock.Verify(
                waiter => waiter.ResolvedChild(),
                Times.Once);
        }

        [Fact]
        public void ConcreteDependencyResolverAdder_Add_AddedFactoryDoesNotResolveChildWaiterWhenCalledWithUnregisteredActor()
        {
            //arrange
            ConcreteDependencyResolverAdder sut = CreateConcreteDependencyResolverAdder();

            //act
            sut.Add(
                DependencyResolverAdder,
                ChildWaiter,
                this,
                Factories);

            //assert
            try { ActorFactory(UnregisteredActorType); } catch { }
            ChildWaiterMock.Verify(
                waiter => waiter.ResolvedChild(),
                Times.Never);
        }
    }
}