using System;
using System.Collections.Immutable;
using Akka.Actor;
using Akka.TestKit;
using FluentAssertions;
using Moq;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.ConcreteResolverSettingsTests
{
    public class RegisterResolver : TestBase
    {
        #region Null tests
        [Fact]
        public void ConcreteResolverSettings_RegisterResolverWithNullTestKit_ThrowsArgumentNullException()
        {
            //arrange
            ConcreteResolverSettings sut = ConcreteResolverSettings.Empty;

            //act
            Action act = () => sut.RegisterResolver(null);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }
        #endregion
        
        [Fact]
        public void ConcreteResolverSettings_RegisterResolver_ConstructsOnlyOneConcreteDependencyResolverAdderCreator()
        {
            //arrange
            ConcreteResolverSettings sut = ConcreteResolverSettings.Empty;

            //act
            sut.RegisterResolver(this);

            //assert
            ConcreteDependencyResolverAdderCreatorConstructorCount.Should().Be(1);
        }

        [Fact]
        public void ConcreteResolverSettings_RegisterResolver_OnlysAddsOneConcreteDependancyResolver()
        {
            //arrange
            ConcreteResolverSettings sut = ConcreteResolverSettings.Empty;

            //act
            sut.RegisterResolver(this);

            //assert
            ConcreteDependencyResolverAdderMock.Verify(
                adder => adder.Add(It.IsAny<TestKitBase>(), It.IsAny<ImmutableDictionary<Type, Func<ActorBase>>>()),
                Times.Once);
        }

        [Fact]
        public void ConcreteResolverSettings_RegisterResolver_AddsConcreteDependancyResolverWithCorrectTestKit()
        {
            //arrange
            ConcreteResolverSettings sut = ConcreteResolverSettings.Empty;

            //act
            sut.RegisterResolver(this);

            //assert
            ConcreteDependencyResolverAdderMock.Verify(
                adder => adder.Add(this, It.IsAny<ImmutableDictionary<Type, Func<ActorBase>>>()));
        }

        [Fact]
        public void ConcreteResolverSettings_RegisterResolverWithNoFactories_AddsConcreteDependancyResolverWithEmptyFactories()
        {
            //arrange
            ConcreteResolverSettings sut = ConcreteResolverSettings.Empty;

            //act
            sut.RegisterResolver(this);

            //assert
            HandlersPassedIntoMock.Should().BeEmpty();
        }

        [Fact]
        public void ConcreteResolverSettings_RegisterResolverWithFuncFactories_AddsConcreteDependancyResolverWithCorrectFactories()
        {
            //arrange
            DummyActor1 actor1 = new DummyActor1();
            DummyActor2 actor2 = new DummyActor2();
            ConcreteResolverSettings sut = ConcreteResolverSettings
                .Empty
                .RegisterActor(() => actor1)
                .RegisterActor(() => actor2);

            //act
            sut.RegisterResolver(this);

            //assert
            HandlersPassedIntoMock.ShouldAllBeEquivalentTo(
                ImmutableDictionary<Type, Func<ActorBase>>
                    .Empty
                    .Add(typeof(DummyActor1), () => actor1)
                    .Add(typeof(DummyActor2), () => actor2),
                options => options
                    .Using<Func<ActorBase>>(context => context.Subject.Invoke().Should().BeSameAs(context.Expectation.Invoke()))
                    .WhenTypeIs<Func<ActorBase>>());
        }

        [Fact]
        public void ConcreteResolverSettings_RegisterResolverWithDuplicateFuncFactories_AddsConcreteDependancyResolverWithCorrectFactories()
        {
            //arrange
            DummyActor1 actor1 = new DummyActor1();
            DummyActor1 duplicateActor1 = new DummyActor1();
            DummyActor2 actor2 = new DummyActor2();
            ConcreteResolverSettings sut = ConcreteResolverSettings
                .Empty
                .RegisterActor(() => actor1)
                .RegisterActor(() => actor2)
                .RegisterActor(() => duplicateActor1);

            //act
            sut.RegisterResolver(this);

            //assert
            HandlersPassedIntoMock.ShouldAllBeEquivalentTo(
                ImmutableDictionary<Type, Func<ActorBase>>
                    .Empty
                    .Add(typeof(DummyActor1), () => duplicateActor1)
                    .Add(typeof(DummyActor2), () => actor2),
                options => options
                    .Using<Func<ActorBase>>(context => context.Subject.Invoke().Should().BeSameAs(context.Expectation.Invoke()))
                    .WhenTypeIs<Func<ActorBase>>());
        }

        [Fact]
        public void ConcreteResolverSettings_RegisterResolverWithDuplicateFuncFactoriesInDifferentInstances_AddsConcreteDependancyResolverWithCorrectFactories()
        {
            //arrange
            DummyActor1 actor1 = new DummyActor1();
            DummyActor2 actor2 = new DummyActor2();
            ConcreteResolverSettings sut = ConcreteResolverSettings
                .Empty
                .RegisterActor(() => actor1);
            ConcreteResolverSettings differentInstance = sut
                .RegisterActor(() => actor2);

            //act
            sut.RegisterResolver(this);

            //assert
            HandlersPassedIntoMock.ShouldAllBeEquivalentTo(
                ImmutableDictionary<Type, Func<ActorBase>>
                    .Empty
                    .Add(typeof(DummyActor1), () => actor1),
                options => options
                    .Using<Func<ActorBase>>(context => context.Subject.Invoke().Should().BeSameAs(context.Expectation.Invoke()))
                    .WhenTypeIs<Func<ActorBase>>());
        }

        [Fact]
        public void ConcreteResolverSettings_RegisterResolverWithGenericFactories_AddsConcreteDependancyResolverWithCorrectFactories()
        {
            //arrange
            ConcreteResolverSettings sut = ConcreteResolverSettings
                .Empty
                .RegisterActor<DummyActor1>()
                .RegisterActor<DummyActor2>();

            //act
            sut.RegisterResolver(this);

            //assert
            HandlersPassedIntoMock.ShouldAllBeEquivalentTo(
                ImmutableDictionary<Type, Func<ActorBase>>
                    .Empty
                    .Add(typeof(DummyActor1), () => new DummyActor1())
                    .Add(typeof(DummyActor2), () => new DummyActor2()),
                options => options
                    .Using<Func<ActorBase>>(context => context.Subject.Invoke().GetType().Should().Be(context.Expectation.Invoke().GetType()))
                    .WhenTypeIs<Func<ActorBase>>());
        }

        [Fact]
        public void ConcreteResolverSettings_RegisterResolverWithDuplicateGenericFactories_AddsConcreteDependancyResolverWithCorrectFactories()
        {
            //arrange
            ConcreteResolverSettings sut = ConcreteResolverSettings
                .Empty
                .RegisterActor<DummyActor1>()
                .RegisterActor<DummyActor2>()
                .RegisterActor<DummyActor1>();

            //act
            sut.RegisterResolver(this);

            //assert
            HandlersPassedIntoMock.ShouldAllBeEquivalentTo(
                ImmutableDictionary<Type, Func<ActorBase>>
                    .Empty
                    .Add(typeof(DummyActor2), () => new DummyActor2())
                    .Add(typeof(DummyActor1), () => new DummyActor1()),
                options => options
                    .Using<Func<ActorBase>>(context => context.Subject.Invoke().GetType().Should().Be(context.Expectation.Invoke().GetType()))
                    .WhenTypeIs<Func<ActorBase>>());
        }

        [Fact]
        public void ConcreteResolverSettings_RegisterResolverWithFactoriesInDifferentInstances_AddsConcreteDependancyResolverWithCorrectFactories()
        {
            //arrange
            ConcreteResolverSettings sut = ConcreteResolverSettings
                .Empty
                .RegisterActor<DummyActor1>();
            ConcreteResolverSettings differentInstance = sut
                .RegisterActor<DummyActor2>();

            //act
            sut.RegisterResolver(this);

            //assert
            HandlersPassedIntoMock.ShouldAllBeEquivalentTo(
                ImmutableDictionary<Type, Func<ActorBase>>
                    .Empty
                    .Add(typeof(DummyActor1), () => new DummyActor1()),
                options => options
                    .Using<Func<ActorBase>>(context => context.Subject.Invoke().GetType().Should().Be(context.Expectation.Invoke().GetType()))
                    .WhenTypeIs<Func<ActorBase>>());
        }
    }
}