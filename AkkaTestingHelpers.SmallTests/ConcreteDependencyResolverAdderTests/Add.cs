using System;
using Akka.Actor;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;
using FluentAssertions;
using Xunit;

// ReSharper disable EmptyGeneralCatchClause

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.ConcreteDependencyResolverAdderTests
{
    public class Add : TestBase
    {
        #region Null tests
        [Fact]
        public void ConcreteDependencyResolverAdder_AddWithNullTestKit_ThrowsArgumentNullException()
        {
            //arrange
            ConcreteDependencyResolverAdder sut = CreateConcreteDependencyResolverAdder();

            //act
            Action act = () =>sut.Add(
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
                this,
                Factories);

            //assert
            ActorBase result = ActorFactory(RegisteredActorType);
            result.Should().BeSameAs(LastResolvedActor);
        }
        
        [Fact]
        public void ConcreteDependencyResolverAdder_Add_AddedFactoryIsCalledOnEverCall()
        {
            //arrange
            ConcreteDependencyResolverAdder sut = CreateConcreteDependencyResolverAdder();

            //act
            sut.Add(
                this,
                Factories);

            //assert
            ActorBase result1 = ActorFactory(RegisteredActorType);
            ActorBase result2 = ActorFactory(RegisteredActorType);
            result1.Should().NotBeSameAs(result2);
        }

        [Fact]
        public void ConcreteDependencyResolverAdder_Add_AddedFactoryReturnsActorWhenCalledWithUnregisteredActor()
        {
            //arrange
            ConcreteDependencyResolverAdder sut = CreateConcreteDependencyResolverAdder();

            //act
            sut.Add(
                this,
                Factories);

            //assert
            Action act = () => ActorFactory(UnregisteredActorType);
            act
                .ShouldThrow<InvalidOperationException>()
                .WithMessage($"Please register the type '{UnregisteredActorType.Name}' in the settings");
        }
    }
}