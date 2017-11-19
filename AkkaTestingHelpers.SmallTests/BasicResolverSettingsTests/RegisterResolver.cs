using System;
using System.Collections.Immutable;
using Akka.Actor;
using Akka.TestKit;
using FluentAssertions;
using Moq;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.BasicResolverSettingsTests
{
    public class RegisterResolver : TestBase
    {
        #region Null tests
        [Fact]
        public void BasicResolverSettings_RegisterResolverWithNullTestKit_ThrowsArgumentNullException()
        {
            //arrange
            BasicResolverSettings sut = BasicResolverSettings.Empty;

            //act
            Action act = () => sut.RegisterResolver(null);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }
        #endregion
        
        [Fact]
        public void BasicResolverSettings_RegisterResolver_ConstructsOnlyOneConcreteDependencyResolverAdderCreator()
        {
            //arrange
            BasicResolverSettings sut = BasicResolverSettings.Empty;

            //act
            sut.RegisterResolver(this);

            //assert
            ConcreteDependencyResolverAdderCreatorConstructorCount.Should().Be(1);
        }

        [Fact]
        public void BasicResolverSettings_RegisterResolver_OnlysAddsOneConcreteDependancyResolver()
        {
            //arrange
            BasicResolverSettings sut = BasicResolverSettings.Empty;

            //act
            sut.RegisterResolver(this);

            //assert
            ConcreteDependencyResolverAdderMock.Verify(
                adder => adder.Add(It.IsAny<TestKitBase>(), It.IsAny<ImmutableDictionary<Type, Func<ActorBase>>>()),
                Times.Once);
        }

        [Fact]
        public void BasicResolverSettings_RegisterResolver_AddsConcreteDependancyResolverWithCorrectTestKit()
        {
            //arrange
            BasicResolverSettings sut = BasicResolverSettings.Empty;

            //act
            sut.RegisterResolver(this);

            //assert
            ConcreteDependencyResolverAdderMock.Verify(
                adder => adder.Add(this, It.IsAny<ImmutableDictionary<Type, Func<ActorBase>>>()));
        }

        [Fact]
        public void BasicResolverSettings_RegisterResolverWithNoFactories_AddsConcreteDependancyResolverWithEmptyFactories()
        {
            //arrange
            BasicResolverSettings sut = BasicResolverSettings.Empty;

            //act
            sut.RegisterResolver(this);

            //assert
            HandlersPassedIntoMock.Should().BeEmpty();
        }

        [Fact]
        public void BasicResolverSettings_RegisterResolverWithFuncFactories_AddsConcreteDependancyResolverWithCorrectFactories()
        {
            //arrange
            DummyActor1 actor1 = new DummyActor1();
            DummyActor2 actor2 = new DummyActor2();
            BasicResolverSettings sut = BasicResolverSettings
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
        public void BasicResolverSettings_RegisterResolverWithDuplicateFuncFactories_AddsConcreteDependancyResolverWithCorrectFactories()
        {
            //arrange
            DummyActor1 actor1 = new DummyActor1();
            DummyActor1 duplicateActor1 = new DummyActor1();
            DummyActor2 actor2 = new DummyActor2();
            BasicResolverSettings sut = BasicResolverSettings
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
        public void BasicResolverSettings_RegisterResolverWithDuplicateFuncFactoriesInDifferentInstances_AddsConcreteDependancyResolverWithCorrectFactories()
        {
            //arrange
            DummyActor1 actor1 = new DummyActor1();
            DummyActor2 actor2 = new DummyActor2();
            BasicResolverSettings sut = BasicResolverSettings
                .Empty
                .RegisterActor(() => actor1);
            BasicResolverSettings differentInstance = sut
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
        public void BasicResolverSettings_RegisterResolverWithGenericFactories_AddsConcreteDependancyResolverWithCorrectFactories()
        {
            //arrange
            BasicResolverSettings sut = BasicResolverSettings
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
        public void BasicResolverSettings_RegisterResolverWithDuplicateGenericFactories_AddsConcreteDependancyResolverWithCorrectFactories()
        {
            //arrange
            BasicResolverSettings sut = BasicResolverSettings
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
        public void BasicResolverSettings_RegisterResolverWithFactoriesInDifferentInstances_AddsConcreteDependancyResolverWithCorrectFactories()
        {
            //arrange
            BasicResolverSettings sut = BasicResolverSettings
                .Empty
                .RegisterActor<DummyActor1>();
            BasicResolverSettings differentInstance = sut
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