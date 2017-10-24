using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Akka.Actor;
using FluentAssertions;
using FluentAssertions.Equivalency;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.ConcreteResolverSettingsTests
{
    public class CreateResolver : TestBase
    {
        #region Null tests
        [Fact]
        public void ConcreteResolverSettings_CreateResolverWithNullTestKit_ThrowsArgumentNullException()
        {
            //arrange
            ConcreteResolverSettings sut = ConcreteResolverSettings.Empty;

            //act
            Action act = () => sut.CreateResolver(null);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }
        #endregion

        [Fact]
        public void ConcreteResolverSettings_CreateResolver_ReturnsConcreteResolver()
        {
            //arrange
            ConcreteResolverSettings sut = ConcreteResolverSettings.Empty;

            //act
            ConcreteResolver result = sut.CreateResolver(this);

            //assert
            result.Should().BeSameAs(ConstructedConcreteResolver);
        }

        [Fact]
        public void ConcreteResolverSettings_CreateResolver_ConstructsOnlyOneConcreteResolver()
        {
            //arrange
            ConcreteResolverSettings sut = ConcreteResolverSettings.Empty;

            //act
            sut.CreateResolver(this);

            //assert
            ConcreteResolverConstructorCount.Should().Be(1);
        }

        [Fact]
        public void ConcreteResolverSettings_CreateResolver_ConstructsConcreteResolverWithSutCreatorClass()
        {
            //arrange
            ConcreteResolverSettings sut = ConcreteResolverSettings.Empty;

            //act
            sut.CreateResolver(this);

            //assert
            SutCreatorPassedIntoShim.Should().BeSameAs(ConstructedSutCreator);
        }

        [Fact]
        public void ConcreteResolverSettings_CreateResolver_OnlyConstructsOneSutCreator()
        {
            //arrange
            ConcreteResolverSettings sut = ConcreteResolverSettings.Empty;

            //act
            sut.CreateResolver(this);

            //assert
            SutCreatorConstructorCount.Should().Be(1);
        }

        [Fact]
        public void ConcreteResolverSettings_CreateResolver_ConstructsConcreteResolverWithChildTellerClass()
        {
            //arrange
            ConcreteResolverSettings sut = ConcreteResolverSettings.Empty;

            //act
            sut.CreateResolver(this);

            //assert
            ChildTellerPassedIntoShim.Should().BeSameAs(ConstructedChildTeller);
        }

        [Fact]
        public void ConcreteResolverSettings_CreateResolver_OnlyConstructsOneChildTeller()
        {
            //arrange
            ConcreteResolverSettings sut = ConcreteResolverSettings.Empty;

            //act
            sut.CreateResolver(this);

            //assert
            ChildTellerConstructorCount.Should().Be(1);
        }

        [Fact]
        public void ConcreteResolverSettings_CreateResolver_ConstructsConcreteResolverWithChildWaiterClass()
        {
            //arrange
            ConcreteResolverSettings sut = ConcreteResolverSettings.Empty;

            //act
            sut.CreateResolver(this);

            //assert
            ChildWaiterPassedIntoShim.Should().BeSameAs(ConstructedChildWaiter);
        }

        [Fact]
        public void ConcreteResolverSettings_CreateResolver_OnlyConstructsOneChildWaiter()
        {
            //arrange
            ConcreteResolverSettings sut = ConcreteResolverSettings.Empty;

            //act
            sut.CreateResolver(this);

            //assert
            ChildWaiterConstructorCount.Should().Be(1);
        }

        [Fact]
        public void ConcreteResolverSettings_CreateResolver_ConstructsConcreteResolverWithDependencyResolverAdderClass()
        {
            //arrange
            ConcreteResolverSettings sut = ConcreteResolverSettings.Empty;

            //act
            sut.CreateResolver(this);

            //assert
            DependencyResolverAdderPassedIntoShim.Should().BeSameAs(ConstructedDependencyResolverAdder);
        }

        [Fact]
        public void ConcreteResolverSettings_CreateResolver_OnlyConstructsOneDependencyResolverAdder()
        {
            //arrange
            ConcreteResolverSettings sut = ConcreteResolverSettings.Empty;

            //act
            sut.CreateResolver(this);

            //assert
            DependencyResolverAdderConstructorCount.Should().Be(1);
        }

        [Fact]
        public void ConcreteResolverSettings_CreateResolver_ConstructsConcreteResolverWithConcreteDependencyResolverAdder()
        {
            //arrange
            ConcreteResolverSettings sut = ConcreteResolverSettings.Empty;

            //act
            sut.CreateResolver(this);

            //assert
            ConcreteDependencyResolverAdderPassedIntoShim.Should().BeSameAs(ConstructedConcreteDependencyResolverAdder);
        }

        [Fact]
        public void ConcreteResolverSettings_CreateResolver_OnlyConstructsOneConcreteDependencyResolverAdder()
        {
            //arrange
            ConcreteResolverSettings sut = ConcreteResolverSettings.Empty;

            //act
            sut.CreateResolver(this);

            //assert
            ConcreteDependencyResolverAdderConstructorCount.Should().Be(1);
        }

        [Fact]
        public void ConcreteResolverSettings_CreateResolver_ConstructsConcreteResolverWithCorrectTestKit()
        {
            //arrange
            ConcreteResolverSettings sut = ConcreteResolverSettings.Empty;

            //act
            sut.CreateResolver(this);

            //assert
            TestKitPassedIntoShim.Should().BeSameAs(this);
        }

        [Fact]
        public void ConcreteResolverSettings_CreateResolverWithNoFactories_ConstructsConcreteResolverWithEmptyFactories()
        {
            //arrange
            ConcreteResolverSettings sut = ConcreteResolverSettings.Empty;

            //act
            sut.CreateResolver(this);

            //assert
            FactoriesPassedIntoShim.Should().BeEmpty();
        }

        [Fact]
        public void ConcreteResolverSettings_CreateResolverWithFuncFactories_ConstructsConcreteResolverWithCorrectFactories()
        {
            //arrange
            DummyActor1 actor1 = new DummyActor1();
            DummyActor2 actor2 = new DummyActor2();
            ConcreteResolverSettings sut = ConcreteResolverSettings
                .Empty
                .Register(() => actor1)
                .Register(() => actor2);

            //act
            sut.CreateResolver(this);
            
            //assert
            FactoriesPassedIntoShim.ShouldAllBeEquivalentTo(
                ImmutableDictionary<Type, Func<ActorBase>>
                    .Empty
                    .Add(typeof(DummyActor1), () => actor1)
                    .Add(typeof(DummyActor2), () => actor2),
                options => options
                    .Using<Func<ActorBase>>(context => context.Subject.Invoke().Should().BeSameAs(context.Expectation.Invoke()))
                    .WhenTypeIs<Func<ActorBase>>());
        }

        [Fact]
        public void ConcreteResolverSettings_CreateResolverWithDuplicateFuncFactories_ConstructsConcreteResolverWithCorrectFactories()
        {
            //arrange
            DummyActor1 actor1 = new DummyActor1();
            DummyActor1 duplicateActor1 = new DummyActor1();
            DummyActor2 actor2 = new DummyActor2();
            ConcreteResolverSettings sut = ConcreteResolverSettings
                .Empty
                .Register(() => actor1)
                .Register(() => actor2)
                .Register(() => duplicateActor1);

            //act
            sut.CreateResolver(this);

            //assert
            FactoriesPassedIntoShim.ShouldAllBeEquivalentTo(
                ImmutableDictionary<Type, Func<ActorBase>>
                    .Empty
                    .Add(typeof(DummyActor1), () => duplicateActor1)
                    .Add(typeof(DummyActor2), () => actor2),
                options => options
                    .Using<Func<ActorBase>>(context => context.Subject.Invoke().Should().BeSameAs(context.Expectation.Invoke()))
                    .WhenTypeIs<Func<ActorBase>>());
        }

        [Fact]
        public void ConcreteResolverSettings_CreateResolverWithDuplicateFuncFactoriesInDifferentInstances_ConstructsConcreteResolverWithCorrectFactories()
        {
            //arrange
            DummyActor1 actor1 = new DummyActor1();
            DummyActor2 actor2 = new DummyActor2();
            ConcreteResolverSettings sut = ConcreteResolverSettings
                .Empty
                .Register(() => actor1);
            ConcreteResolverSettings differentInstance = sut
                .Register(() => actor2);

            //act
            sut.CreateResolver(this);

            //assert
            FactoriesPassedIntoShim.ShouldAllBeEquivalentTo(
                ImmutableDictionary<Type, Func<ActorBase>>
                    .Empty
                    .Add(typeof(DummyActor1), () => actor1),
                options => options
                    .Using<Func<ActorBase>>(context => context.Subject.Invoke().Should().BeSameAs(context.Expectation.Invoke()))
                    .WhenTypeIs<Func<ActorBase>>());
        }

        [Fact]
        public void ConcreteResolverSettings_CreateResolverWithGenericFactories_ConstructsConcreteResolverWithCorrectFactories()
        {
            //arrange
            ConcreteResolverSettings sut = ConcreteResolverSettings
                .Empty
                .Register<DummyActor1>()
                .Register<DummyActor2>();

            //act
            sut.CreateResolver(this);

            //assert
            FactoriesPassedIntoShim.ShouldAllBeEquivalentTo(
                ImmutableDictionary<Type, Func<ActorBase>>
                    .Empty
                    .Add(typeof(DummyActor1), () => new DummyActor1())
                    .Add(typeof(DummyActor2), () => new DummyActor2()),
                options => options
                    .Using<Func<ActorBase>>(context => context.Subject.Invoke().GetType().Should().Be(context.Expectation.Invoke().GetType()))
                    .WhenTypeIs<Func<ActorBase>>());
        }

        [Fact]
        public void ConcreteResolverSettings_CreateResolverWithDuplicateGenericFactories_ConstructsConcreteResolverWithCorrectFactories()
        {
            //arrange
            ConcreteResolverSettings sut = ConcreteResolverSettings
                .Empty
                .Register<DummyActor1>()
                .Register<DummyActor2>()
                .Register<DummyActor1>();

            //act
            sut.CreateResolver(this);

            //assert
            FactoriesPassedIntoShim.ShouldAllBeEquivalentTo(
                ImmutableDictionary<Type, Func<ActorBase>>
                    .Empty
                    .Add(typeof(DummyActor2), () => new DummyActor2())
                    .Add(typeof(DummyActor1), () => new DummyActor1()),
                options => options
                    .Using<Func<ActorBase>>(context => context.Subject.Invoke().GetType().Should().Be(context.Expectation.Invoke().GetType()))
                    .WhenTypeIs<Func<ActorBase>>());
        }

        [Fact]
        public void ConcreteResolverSettings_CreateResolverWithFactoriesInDifferentInstances_ConstructsConcreteResolverWithCorrectFactories()
        {
            //arrange
            ConcreteResolverSettings sut = ConcreteResolverSettings
                .Empty
                .Register<DummyActor1>();
            ConcreteResolverSettings differentInstance = sut
                .Register<DummyActor2>();

            //act
            sut.CreateResolver(this);

            //assert
            FactoriesPassedIntoShim.ShouldAllBeEquivalentTo(
                ImmutableDictionary<Type, Func<ActorBase>>
                    .Empty
                    .Add(typeof(DummyActor1), () => new DummyActor1()),
                options => options
                    .Using<Func<ActorBase>>(context => context.Subject.Invoke().GetType().Should().Be(context.Expectation.Invoke().GetType()))
                    .WhenTypeIs<Func<ActorBase>>());
        }
    }
}