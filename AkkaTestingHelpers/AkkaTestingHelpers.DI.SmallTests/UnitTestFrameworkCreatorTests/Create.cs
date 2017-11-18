using System;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.UnitTestFrameworkCreatorTests
{
    public class Create : TestBase
    {
        #region Null tests
        [Fact]
        public void UnitTestFrameworkSettings_CreateWithNullHandlers_ThrowsArgumentNullException()
        {
            //arrange
            UnitTestFrameworkCreator sut = CreateUnitTestFrameworkCreator();

            //act
            Action act = () => sut.Create<DummyActor1>(null, TestKitPassedIntoSut, PropsPassedIntoSut, NumberOfChildrenIntoSut);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void UnitTestFrameworkSettings_CreateWithNullTestKit_ThrowsArgumentNullException()
        {
            //arrange
            UnitTestFrameworkCreator sut = CreateUnitTestFrameworkCreator();

            //act
            Action act = () => sut.Create<DummyActor1>(HandlersPassedIntoSut, null, PropsPassedIntoSut, NumberOfChildrenIntoSut);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }
        
        [Fact]
        public void UnitTestFrameworkSettings_CreateWithNullProps_ThrowsArgumentNullException()
        {
            //arrange
            UnitTestFrameworkCreator sut = CreateUnitTestFrameworkCreator();

            //act
            Action act = () => sut.Create<DummyActor1>(HandlersPassedIntoSut, TestKitPassedIntoSut, null, NumberOfChildrenIntoSut);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }
        
        [Fact]
        public void UnitTestFrameworkSettings_CreateWithNullHandlersAndTestKitAndProps_ThrowsArgumentNullException()
        {
            //arrange
            UnitTestFrameworkCreator sut = CreateUnitTestFrameworkCreator();

            //act
            Action act = () => sut.Create<DummyActor1>(null, null, null, NumberOfChildrenIntoSut);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }
        #endregion

        [Fact]
        public void UnitTestFrameworkSettings_Create_ReturnsUnitTestFramework()
        {
            //arrange
            UnitTestFrameworkCreator sut = CreateUnitTestFrameworkCreator();

            //act
            UnitTestFramework<DummyActor1> result = sut.Create<DummyActor1>(HandlersPassedIntoSut, TestKitPassedIntoSut, PropsPassedIntoSut, NumberOfChildrenIntoSut);

            //assert
            result.Should().BeSameAs(ConstructedUnitTestFramework);
        }

        [Fact]
        public void UnitTestFrameworkSettings_Create_ConstructsOnlyOneUnitTestFramework()
        {
            //arrange
            UnitTestFrameworkCreator sut = CreateUnitTestFrameworkCreator();

            //act
            sut.Create<DummyActor1>(HandlersPassedIntoSut, TestKitPassedIntoSut, PropsPassedIntoSut, NumberOfChildrenIntoSut);

            //assert
            UnitTestFrameworkConstructorCount.Should().Be(1);
        }

        [Fact]
        public void UnitTestFrameworkSettings_Create_ConstructsUnitTestFrameworkWithSutCreatorClass()
        {
            //arrange
            UnitTestFrameworkCreator sut = CreateUnitTestFrameworkCreator();

            //act
            sut.Create<DummyActor1>(HandlersPassedIntoSut, TestKitPassedIntoSut, PropsPassedIntoSut, NumberOfChildrenIntoSut);

            //assert
            SutCreatorPassedIntoShim.Should().BeSameAs(ConstructedSutCreator);
        }

        [Fact]
        public void UnitTestFrameworkSettings_Create_OnlyConstructsOneSutCreator()
        {
            //arrange
            UnitTestFrameworkCreator sut = CreateUnitTestFrameworkCreator();

            //act
            sut.Create<DummyActor1>(HandlersPassedIntoSut, TestKitPassedIntoSut, PropsPassedIntoSut, NumberOfChildrenIntoSut);

            //assert
            SutCreatorConstructorCount.Should().Be(1);
        }

        [Fact]
        public void UnitTestFrameworkSettings_Create_ConstructsUnitTestFrameworkWithChildTellerClass()
        {
            //arrange
            UnitTestFrameworkCreator sut = CreateUnitTestFrameworkCreator();

            //act
            sut.Create<DummyActor1>(HandlersPassedIntoSut, TestKitPassedIntoSut, PropsPassedIntoSut, NumberOfChildrenIntoSut);

            //assert
            TellChildWaiterPassedIntoShim.Should().BeSameAs(ConstructedTellChildWaiter);
        }

        [Fact]
        public void UnitTestFrameworkSettings_Create_OnlyConstructsOneChildTeller()
        {
            //arrange
            UnitTestFrameworkCreator sut = CreateUnitTestFrameworkCreator();

            //act
            sut.Create<DummyActor1>(HandlersPassedIntoSut, TestKitPassedIntoSut, PropsPassedIntoSut, NumberOfChildrenIntoSut);

            //assert
            TellChildWaiterConstructorCount.Should().Be(1);
        }

        [Fact]
        public void UnitTestFrameworkSettings_Create_ConstructsUnitTestFrameworkWithChildWaiterClass()
        {
            //arrange
            UnitTestFrameworkCreator sut = CreateUnitTestFrameworkCreator();

            //act
            sut.Create<DummyActor1>(HandlersPassedIntoSut, TestKitPassedIntoSut, PropsPassedIntoSut, NumberOfChildrenIntoSut);

            //assert
            ChildWaiterPassedIntoShim.Should().BeSameAs(ConstructedChildWaiter);
        }

        [Fact]
        public void UnitTestFrameworkSettings_Create_OnlyConstructsOneChildWaiter()
        {
            //arrange
            UnitTestFrameworkCreator sut = CreateUnitTestFrameworkCreator();

            //act
            sut.Create<DummyActor1>(HandlersPassedIntoSut, TestKitPassedIntoSut, PropsPassedIntoSut, NumberOfChildrenIntoSut);

            //assert
            ChildWaiterConstructorCount.Should().Be(1);
        }

        [Fact]
        public void UnitTestFrameworkSettings_Create_ConstructsUnitTestFrameworkWithDependencyResolverAdderClass()
        {
            //arrange
            UnitTestFrameworkCreator sut = CreateUnitTestFrameworkCreator();

            //act
            sut.Create<DummyActor1>(HandlersPassedIntoSut, TestKitPassedIntoSut, PropsPassedIntoSut, NumberOfChildrenIntoSut);

            //assert
            DependencyResolverAdderPassedIntoShim.Should().BeSameAs(ConstructedDependencyResolverAdder);
        }

        [Fact]
        public void UnitTestFrameworkSettings_Create_OnlyConstructsOneDependencyResolverAdder()
        {
            //arrange
            UnitTestFrameworkCreator sut = CreateUnitTestFrameworkCreator();

            //act
            sut.Create<DummyActor1>(HandlersPassedIntoSut, TestKitPassedIntoSut, PropsPassedIntoSut, NumberOfChildrenIntoSut);

            //assert
            DependencyResolverAdderConstructorCount.Should().Be(1);
        }

        [Fact]
        public void UnitTestFrameworkSettings_Create_ConstructsUnitTestFrameworkWithConcreteDependencyResolverAdder()
        {
            //arrange
            UnitTestFrameworkCreator sut = CreateUnitTestFrameworkCreator();

            //act
            sut.Create<DummyActor1>(HandlersPassedIntoSut, TestKitPassedIntoSut, PropsPassedIntoSut, NumberOfChildrenIntoSut);

            //assert
            TestProbeDependencyResolverAdderPassedIntoShim.Should().BeSameAs(ConstructedTestProbeDependencyResolverAdder);
        }

        [Fact]
        public void UnitTestFrameworkSettings_Create_OnlyConstructsOneConcreteDependencyResolverAdder()
        {
            //arrange
            UnitTestFrameworkCreator sut = CreateUnitTestFrameworkCreator();

            //act
            sut.Create<DummyActor1>(HandlersPassedIntoSut, TestKitPassedIntoSut, PropsPassedIntoSut, NumberOfChildrenIntoSut);

            //assert
            TestProbeDependencyResolverAdderConstructorCount.Should().Be(1);
        }

        [Fact]
        public void UnitTestFrameworkSettings_Create_ConstructsUnitTestFrameworkWithTestProbeCreator()
        {
            //arrange
            UnitTestFrameworkCreator sut = CreateUnitTestFrameworkCreator();

            //act
            sut.Create<DummyActor1>(HandlersPassedIntoSut, TestKitPassedIntoSut, PropsPassedIntoSut, NumberOfChildrenIntoSut);

            //assert
            TestProbeCreatorPassedIntoShim.Should().BeSameAs(ConstructedTestProbeCreator);
        }

        [Fact]
        public void UnitTestFrameworkSettings_Create_OnlyConstructsOneTestProbeCreator()
        {
            //arrange
            UnitTestFrameworkCreator sut = CreateUnitTestFrameworkCreator();

            //act
            sut.Create<DummyActor1>(HandlersPassedIntoSut, TestKitPassedIntoSut, PropsPassedIntoSut, NumberOfChildrenIntoSut);

            //assert
            TestProbeCreatorConstructorCount.Should().Be(1);
        }

        [Fact]
        public void UnitTestFrameworkSettings_Create_ConstructsUnitTestFrameworkWithResolvedTestProbeStore()
        {
            //arrange
            UnitTestFrameworkCreator sut = CreateUnitTestFrameworkCreator();

            //act
            sut.Create<DummyActor1>(HandlersPassedIntoSut, TestKitPassedIntoSut, PropsPassedIntoSut, NumberOfChildrenIntoSut);

            //assert
            ResolvedTestProbeStorePassedIntoShim.Should().BeSameAs(ConstructedResolvedTestProbeStore);
        }

        [Fact]
        public void UnitTestFrameworkSettings_Create_OnlyConstructsOneResolvedTestProbeStore()
        {
            //arrange
            UnitTestFrameworkCreator sut = CreateUnitTestFrameworkCreator();

            //act
            sut.Create<DummyActor1>(HandlersPassedIntoSut, TestKitPassedIntoSut, PropsPassedIntoSut, NumberOfChildrenIntoSut);

            //assert
            ResolvedTestProbeStoreConstructorCount.Should().Be(1);
        }

        [Fact]
        public void UnitTestFrameworkSettings_Create_ConstructsUnitTestFrameworkWithTestProbeActorCreator()
        {
            //arrange
            UnitTestFrameworkCreator sut = CreateUnitTestFrameworkCreator();

            //act
            sut.Create<DummyActor1>(HandlersPassedIntoSut, TestKitPassedIntoSut, PropsPassedIntoSut, NumberOfChildrenIntoSut);

            //assert
            TestProbeActorCreatorPassedIntoShim.Should().BeSameAs(ConstructedTestProbeActorCreator);
        }

        [Fact]
        public void UnitTestFrameworkSettings_Create_OnlyConstructsOneTestProbeActorCreator()
        {
            //arrange
            UnitTestFrameworkCreator sut = CreateUnitTestFrameworkCreator();

            //act
            sut.Create<DummyActor1>(HandlersPassedIntoSut, TestKitPassedIntoSut, PropsPassedIntoSut, NumberOfChildrenIntoSut);

            //assert
            TestProbeActorCreatorConstructorCount.Should().Be(1);
        }

        [Fact]
        public void UnitTestFrameworkSettings_Create_ConstructsUnitTestFrameworkWithTestProbeHandlersMapper()
        {
            //arrange
            UnitTestFrameworkCreator sut = CreateUnitTestFrameworkCreator();

            //act
            sut.Create<DummyActor1>(HandlersPassedIntoSut, TestKitPassedIntoSut, PropsPassedIntoSut, NumberOfChildrenIntoSut);

            //assert
            TestProbeHandlersMapperPassedIntoShim.Should().BeSameAs(ConstructedTestProbeHandlersMapper);
        }

        [Fact]
        public void UnitTestFrameworkSettings_Create_OnlyConstructsOneTestProbeHandlersMapper()
        {
            //arrange
            UnitTestFrameworkCreator sut = CreateUnitTestFrameworkCreator();

            //act
            sut.Create<DummyActor1>(HandlersPassedIntoSut, TestKitPassedIntoSut, PropsPassedIntoSut, NumberOfChildrenIntoSut);

            //assert
            TestProbeHandlersMapperConstructorCount.Should().Be(1);
        }
        
        [Fact]
        public void UnitTestFrameworkSettings_Create_ConstructsUnitTestFrameworkWithSutSupervisorStrategyGetter()
        {
            //arrange
            UnitTestFrameworkCreator sut = CreateUnitTestFrameworkCreator();

            //act
            sut.Create<DummyActor1>(HandlersPassedIntoSut, TestKitPassedIntoSut, PropsPassedIntoSut, NumberOfChildrenIntoSut);

            //assert
            SutSupervisorStrategyGetterIntoShim.Should().BeSameAs(ConstructedSutSupervisorStrategyGetter);
        }

        [Fact]
        public void UnitTestFrameworkSettings_Create_OnlyConstructsOneSutSupervisorStrategyGetter()
        {
            //arrange
            UnitTestFrameworkCreator sut = CreateUnitTestFrameworkCreator();

            //act
            sut.Create<DummyActor1>(HandlersPassedIntoSut, TestKitPassedIntoSut, PropsPassedIntoSut, NumberOfChildrenIntoSut);

            //assert
            SutSupervisorStrategyGetterConstructorCount.Should().Be(1);
        }
        
        [Fact]
        public void UnitTestFrameworkSettings_Create_ConstructsUnitTestFrameworkWithCorrectTestKit()
        {
            //arrange
            UnitTestFrameworkCreator sut = CreateUnitTestFrameworkCreator();

            //act
            sut.Create<DummyActor1>(HandlersPassedIntoSut, TestKitPassedIntoSut, PropsPassedIntoSut, NumberOfChildrenIntoSut);

            //assert
            TestKitPassedIntoShim.Should().BeSameAs(TestKitPassedIntoSut);
        }

        [Fact]
        public void UnitTestFrameworkSettings_Create_ConstructsUnitTestFrameworkWithProps()
        {
            //arrange
            UnitTestFrameworkCreator sut = CreateUnitTestFrameworkCreator();

            //act
            sut.Create<DummyActor1>(HandlersPassedIntoSut, TestKitPassedIntoSut, PropsPassedIntoSut, NumberOfChildrenIntoSut);

            //assert
            PropsPassedIntoShim.Should().BeSameAs(PropsPassedIntoSut);
        }

        [Fact]
        public void UnitTestFrameworkSettings_Create_ConstructsUnitTestFrameworkWithHandlers()
        {
            //arrange
            UnitTestFrameworkCreator sut = CreateUnitTestFrameworkCreator();

            //act
            sut.Create<DummyActor1>(HandlersPassedIntoSut, TestKitPassedIntoSut, PropsPassedIntoSut, NumberOfChildrenIntoSut);

            //assert
            HandlersPassedIntoShim.Should().BeSameAs(HandlersPassedIntoSut);
        }
    }
}