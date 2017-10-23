using System;
using System.Collections.Immutable;
using Akka.Actor;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.TestProbeResolverSettingsTests
{
    public class CreateResolver : TestBase
    {
        #region Null tests
        [Fact]
        public void TestProbeResolverSettings_CreateResolverWithNullTestKit_ThrowsArgumentNullException()
        {
            //arrange
            TestProbeResolverSettings sut = TestProbeResolverSettings.Empty;

            //act
            Action act = () => sut.CreateResolver(null);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }
        #endregion

        [Fact]
        public void TestProbeResolverSettings_CreateResolver_ReturnsTestProbeResolver()
        {
            //arrange
            TestProbeResolverSettings sut = TestProbeResolverSettings.Empty;

            //act
            TestProbeResolver result = sut.CreateResolver(this);

            //assert
            result.Should().BeSameAs(ConstructedTestProbeResolver);
        }

        [Fact]
        public void TestProbeResolverSettings_CreateResolver_ConstructsOnlyOneTestProbeResolver()
        {
            //arrange
            TestProbeResolverSettings sut = TestProbeResolverSettings.Empty;

            //act
            sut.CreateResolver(this);

            //assert
            TestProbeResolverConstructorCount.Should().Be(1);
        }

        [Fact]
        public void TestProbeResolverSettings_CreateResolver_ConstructsTestProbeResolverWithSutCreatorClass()
        {
            //arrange
            TestProbeResolverSettings sut = TestProbeResolverSettings.Empty;

            //act
            sut.CreateResolver(this);

            //assert
            SutCreatorPassedIntoShim.Should().BeSameAs(ConstructedSutCreator);
        }

        [Fact]
        public void TestProbeResolverSettings_CreateResolver_OnlyConstructsOneSutCreator()
        {
            //arrange
            TestProbeResolverSettings sut = TestProbeResolverSettings.Empty;

            //act
            sut.CreateResolver(this);

            //assert
            SutCreatorConstructorCount.Should().Be(1);
        }

        [Fact]
        public void TestProbeResolverSettings_CreateResolver_ConstructsTestProbeResolverWithChildTellerClass()
        {
            //arrange
            TestProbeResolverSettings sut = TestProbeResolverSettings.Empty;

            //act
            sut.CreateResolver(this);

            //assert
            ChildTellerPassedIntoShim.Should().BeSameAs(ConstructedChildTeller);
        }

        [Fact]
        public void TestProbeResolverSettings_CreateResolver_OnlyConstructsOneChildTeller()
        {
            //arrange
            TestProbeResolverSettings sut = TestProbeResolverSettings.Empty;

            //act
            sut.CreateResolver(this);

            //assert
            ChildTellerConstructorCount.Should().Be(1);
        }

        [Fact]
        public void TestProbeResolverSettings_CreateResolver_ConstructsTestProbeResolverWithChildWaiterClass()
        {
            //arrange
            TestProbeResolverSettings sut = TestProbeResolverSettings.Empty;

            //act
            sut.CreateResolver(this);

            //assert
            ChildWaiterPassedIntoShim.Should().BeSameAs(ConstructedChildWaiter);
        }

        [Fact]
        public void TestProbeResolverSettings_CreateResolver_OnlyConstructsOneChildWaiter()
        {
            //arrange
            TestProbeResolverSettings sut = TestProbeResolverSettings.Empty;

            //act
            sut.CreateResolver(this);

            //assert
            ChildWaiterConstructorCount.Should().Be(1);
        }

        [Fact]
        public void TestProbeResolverSettings_CreateResolver_ConstructsTestProbeResolverWithDependencyResolverAdderClass()
        {
            //arrange
            TestProbeResolverSettings sut = TestProbeResolverSettings.Empty;

            //act
            sut.CreateResolver(this);

            //assert
            DependencyResolverAdderPassedIntoShim.Should().BeSameAs(ConstructedDependencyResolverAdder);
        }

        [Fact]
        public void TestProbeResolverSettings_CreateResolver_OnlyConstructsOneDependencyResolverAdder()
        {
            //arrange
            TestProbeResolverSettings sut = TestProbeResolverSettings.Empty;

            //act
            sut.CreateResolver(this);

            //assert
            DependencyResolverAdderConstructorCount.Should().Be(1);
        }

        [Fact]
        public void TestProbeResolverSettings_CreateResolver_ConstructsTestProbeResolverWithConcreteDependencyResolverAdder()
        {
            //arrange
            TestProbeResolverSettings sut = TestProbeResolverSettings.Empty;

            //act
            sut.CreateResolver(this);

            //assert
            TestProbeDependencyResolverAdderPassedIntoShim.Should().BeSameAs(ConstructedTestProbeDependencyResolverAdder);
        }

        [Fact]
        public void TestProbeResolverSettings_CreateResolver_OnlyConstructsOneConcreteDependencyResolverAdder()
        {
            //arrange
            TestProbeResolverSettings sut = TestProbeResolverSettings.Empty;

            //act
            sut.CreateResolver(this);

            //assert
            TestProbeDependencyResolverAdderConstructorCount.Should().Be(1);
        }
        
        [Fact]
        public void TestProbeResolverSettings_CreateResolver_ConstructsTestProbeResolverWithTestProbeCreator()
        {
            //arrange
            TestProbeResolverSettings sut = TestProbeResolverSettings.Empty;

            //act
            sut.CreateResolver(this);

            //assert
            TestProbeCreatorPassedIntoShim.Should().BeSameAs(ConstructedTestProbeCreator);
        }

        [Fact]
        public void TestProbeResolverSettings_CreateResolver_OnlyConstructsOneTestProbeCreator()
        {
            //arrange
            TestProbeResolverSettings sut = TestProbeResolverSettings.Empty;

            //act
            sut.CreateResolver(this);

            //assert
            TestProbeCreatorConstructorCount.Should().Be(1);
        }

        [Fact]
        public void TestProbeResolverSettings_CreateResolver_ConstructsTestProbeResolverWithResolvedTestProbeStore()
        {
            //arrange
            TestProbeResolverSettings sut = TestProbeResolverSettings.Empty;

            //act
            sut.CreateResolver(this);

            //assert
            ResolvedTestProbeStorePassedIntoShim.Should().BeSameAs(ConstructedResolvedTestProbeStore);
        }

        [Fact]
        public void TestProbeResolverSettings_CreateResolver_OnlyConstructsOneResolvedTestProbeStore()
        {
            //arrange
            TestProbeResolverSettings sut = TestProbeResolverSettings.Empty;

            //act
            sut.CreateResolver(this);

            //assert
            ResolvedTestProbeStoreConstructorCount.Should().Be(1);
        }

        [Fact]
        public void TestProbeResolverSettings_CreateResolver_ConstructsTestProbeResolverWithTestProbeActorCreator()
        {
            //arrange
            TestProbeResolverSettings sut = TestProbeResolverSettings.Empty;

            //act
            sut.CreateResolver(this);

            //assert
            TestProbeActorCreatorPassedIntoShim.Should().BeSameAs(ConstructedTestProbeActorCreator);
        }

        [Fact]
        public void TestProbeResolverSettings_CreateResolver_OnlyConstructsOneTestProbeActorCreator()
        {
            //arrange
            TestProbeResolverSettings sut = TestProbeResolverSettings.Empty;

            //act
            sut.CreateResolver(this);

            //assert
            TestProbeActorCreatorConstructorCount.Should().Be(1);
        }

        [Fact]
        public void TestProbeResolverSettings_CreateResolver_ConstructsTestProbeResolverWithTestProbeHandlersMapper()
        {
            //arrange
            TestProbeResolverSettings sut = TestProbeResolverSettings.Empty;

            //act
            sut.CreateResolver(this);

            //assert
            TestProbeHandlersMapperPassedIntoShim.Should().BeSameAs(ConstructedTestProbeHandlersMapper);
        }

        [Fact]
        public void TestProbeResolverSettings_CreateResolver_OnlyConstructsOneTestProbeHandlersMapper()
        {
            //arrange
            TestProbeResolverSettings sut = TestProbeResolverSettings.Empty;

            //act
            sut.CreateResolver(this);

            //assert
            TestProbeHandlersMapperConstructorCount.Should().Be(1);
        }
        
        [Fact]
        public void TestProbeResolverSettings_CreateResolver_ConstructsTestProbeResolverWithCorrectTestKit()
        {
            //arrange
            TestProbeResolverSettings sut = TestProbeResolverSettings.Empty;

            //act
            sut.CreateResolver(this);

            //assert
            TestKitPassedIntoShim.Should().BeSameAs(this);
        }
        
        //todo dictionary tests
        //[Fact]
        //public void TestProbeResolverSettings_CreateResolverWithNoFactories_ConstructsTestProbeResolverWithEmptyFactories()
        //{
        //    //arrange
        //    TestProbeResolverSettings sut = TestProbeResolverSettings.Empty;

        //    //act
        //    sut.CreateResolver(this);

        //    //assert
        //    FactoriesPassedIntoShim.Should().BeEmpty();
        //}

        //[Fact]
        //public void TestProbeResolverSettings_CreateResolverWithFuncFactories_ConstructsTestProbeResolverWithCorrectFactories()
        //{
        //    //arrange
        //    DummyActor1 actor1 = new DummyActor1();
        //    DummyActor2 actor2 = new DummyActor2();
        //    TestProbeResolverSettings sut = TestProbeResolverSettings
        //        .Empty
        //        .Register(() => actor1)
        //        .Register(() => actor2);

        //    //act
        //    sut.CreateResolver(this);

        //    //assert
        //    ImmutableDictionary<Type, Func<ActorBase>> expected = ImmutableDictionary<Type, Func<ActorBase>>
        //        .Empty
        //        .Add(typeof(DummyActor1), () => actor1)
        //        .Add(typeof(DummyActor2), () => actor2);
        //    FactoriesPassedIntoShim.ShouldAllBeEquivalentTo(
        //        expected,
        //        options => options
        //            .Using<Func<ActorBase>>(context => context.Subject.Invoke().Should().BeSameAs(context.Expectation.Invoke()))
        //            .WhenTypeIs<Func<ActorBase>>());
        //}

        //[Fact]
        //public void TestProbeResolverSettings_CreateResolverWithDuplicateFuncFactories_ConstructsTestProbeResolverWithCorrectFactories()
        //{
        //    //arrange
        //    DummyActor1 actor1 = new DummyActor1();
        //    DummyActor1 duplicateActor1 = new DummyActor1();
        //    DummyActor2 actor2 = new DummyActor2();
        //    TestProbeResolverSettings sut = TestProbeResolverSettings
        //        .Empty
        //        .Register(() => actor1)
        //        .Register(() => actor2)
        //        .Register(() => duplicateActor1);

        //    //act
        //    sut.CreateResolver(this);

        //    //assert
        //    ImmutableDictionary<Type, Func<ActorBase>> expected = ImmutableDictionary<Type, Func<ActorBase>>
        //        .Empty
        //        .Add(typeof(DummyActor1), () => actor2)
        //        .Add(typeof(DummyActor2), () => duplicateActor1);
        //    FactoriesPassedIntoShim.ShouldAllBeEquivalentTo(
        //        expected,
        //        options => options
        //            .Using<Func<ActorBase>>(context => context.Subject.Invoke().Should().BeSameAs(context.Expectation.Invoke()))
        //            .WhenTypeIs<Func<ActorBase>>());
        //}

        //[Fact]
        //public void TestProbeResolverSettings_CreateResolverWithGenericFactories_ConstructsTestProbeResolverWithCorrectFactories()
        //{
        //    //arrange
        //    TestProbeResolverSettings sut = TestProbeResolverSettings
        //        .Empty
        //        .Register<DummyActor1>()
        //        .Register<DummyActor2>();

        //    //act
        //    sut.CreateResolver(this);

        //    //assert
        //    ImmutableDictionary<Type, Func<ActorBase>> expected = ImmutableDictionary<Type, Func<ActorBase>>
        //        .Empty
        //        .Add(typeof(DummyActor1), () => new DummyActor1())
        //        .Add(typeof(DummyActor2), () => new DummyActor2());
        //    FactoriesPassedIntoShim.ShouldAllBeEquivalentTo(
        //        expected,
        //        options => options
        //            .Using<Func<ActorBase>>(context => context.Subject.Invoke().GetType().Should().Be(context.Expectation.Invoke().GetType()))
        //            .WhenTypeIs<Func<ActorBase>>());
        //}

        //[Fact]
        //public void TestProbeResolverSettings_CreateResolverWithDuplicateGenericFactories_ConstructsTestProbeResolverWithCorrectFactories()
        //{
        //    //arrange
        //    TestProbeResolverSettings sut = TestProbeResolverSettings
        //        .Empty
        //        .Register<DummyActor1>()
        //        .Register<DummyActor2>()
        //        .Register<DummyActor1>();

        //    //act
        //    sut.CreateResolver(this);

        //    //assert
        //    ImmutableDictionary<Type, Func<ActorBase>> expected = ImmutableDictionary<Type, Func<ActorBase>>
        //        .Empty
        //        .Add(typeof(DummyActor2), () => new DummyActor2())
        //        .Add(typeof(DummyActor1), () => new DummyActor1());
        //    FactoriesPassedIntoShim.ShouldAllBeEquivalentTo(
        //        expected,
        //        options => options
        //            .Using<Func<ActorBase>>(context => context.Subject.Invoke().GetType().Should().Be(context.Expectation.Invoke().GetType()))
        //            .WhenTypeIs<Func<ActorBase>>());
        //}
    }
}