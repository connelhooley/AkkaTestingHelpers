using System;
using System.Collections.Immutable;
using Akka.Actor;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.UnitTestFrameworkSettingsTests
{
    public class CreateFrameworkTestKitPropsExpectedChildrenCount : TestBase
    {
        #region Null tests
        [Fact]
        public void UnitTestFrameworkSettings_CreateFrameworkWithNullTestKit_ThrowsArgumentNullException()
        {
            //arrange
            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings.Empty;

            //act
            Action act = () => sut.CreateFramework<DummyActor1>(null, PropsPassedIntoSut, ExpectedChildrenCountPassedIntoSut);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }
        
        [Fact]
        public void UnitTestFrameworkSettings_CreateFrameworkWithNullProps_ThrowsArgumentNullException()
        {
            //arrange
            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings.Empty;

            //act
            Action act = () => sut.CreateFramework<DummyActor1>(TestKitPassedIntoShim, (Props)null, ExpectedChildrenCountPassedIntoSut);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void UnitTestFrameworkSettings_CreateFrameworkWithNullTestKitAndAndProps_ThrowsArgumentNullException()
        {
            //arrange
            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings.Empty;

            //act
            Action act = () => sut.CreateFramework<DummyActor1>(null, (Props)null, ExpectedChildrenCountPassedIntoSut);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }
        #endregion

        [Fact]
        public void UnitTestFrameworkSettings_CreateFramework_ConstructsOnlyOneUnitTestFrameworkCreator()
        {
            //arrange
            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings.Empty;

            //act
            sut.CreateFramework<DummyActor1>(TestKitPassedIntoSut, PropsPassedIntoSut, ExpectedChildrenCountPassedIntoSut);

            //assert
            UnitTestFrameworkCreatorConstructorCount.Should().Be(1);
        }

        [Fact]
        public void UnitTestFrameworkSettings_CreateFramework_CreatesUnitTestFrameworkWithCorrectHandlers()
        {
            //arrange
            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings.Empty;

            //act
            sut.CreateFramework<DummyActor1>(TestKitPassedIntoSut, PropsPassedIntoSut, ExpectedChildrenCountPassedIntoSut);

            //assert
            HandlersPassedIntoShim.Should().BeSameAs(ImmutableDictionary<(Type, Type), Func<object, object>>.Empty);
        }

        [Fact]
        public void UnitTestFrameworkSettings_CreateFramework_CreatesUnitTestFrameworkWithCorrectTestKit()
        {
            //arrange
            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings.Empty;

            //act
            sut.CreateFramework<DummyActor1>(TestKitPassedIntoSut, PropsPassedIntoSut, ExpectedChildrenCountPassedIntoSut);

            //assert
            TestKitPassedIntoShim.Should().BeSameAs(TestKitPassedIntoSut);
        }

        [Fact]
        public void UnitTestFrameworkSettings_CreateFramework_CreatesUnitTestFrameworkWithCorrectProps()
        {
            //arrange
            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings.Empty;

            //act
            sut.CreateFramework<DummyActor1>(TestKitPassedIntoSut, PropsPassedIntoSut, ExpectedChildrenCountPassedIntoSut);

            //assert
            PropsPassedIntoShim.Should().BeSameAs(PropsPassedIntoSut);
        }

        [Fact]
        public void UnitTestFrameworkSettings_CreateFramework_CreatesUnitTestFrameworkWithCorrectExpectedChildrenCount()
        {
            //arrange
            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings.Empty;

            //act
            sut.CreateFramework<DummyActor1>(TestKitPassedIntoSut, PropsPassedIntoSut, ExpectedChildrenCountPassedIntoSut);

            //assert
            ExpectedChildrenCountPassedIntoShim.Should().Be(ExpectedChildrenCountPassedIntoSut);
        }

        [Fact]
        public void UnitTestFrameworkSettings_CreateFramework_OnlyCreatesOneUnitTestFramework()
        {
            //arrange
            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings.Empty;

            //act
            sut.CreateFramework<DummyActor1>(TestKitPassedIntoSut, PropsPassedIntoSut, ExpectedChildrenCountPassedIntoSut);

            //assert
            UnitTestFrameworkCreatorCreateCount.Should().Be(1);
        }

        [Fact]
        public void UnitTestFrameworkSettings_CreateFramework_ReturnsCreatedOneUnitTestFramework()
        {
            //arrange
            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings.Empty;

            //act
            UnitTestFramework<DummyActor1> result = sut.CreateFramework<DummyActor1>(TestKitPassedIntoSut, PropsPassedIntoSut, ExpectedChildrenCountPassedIntoSut);

            //assert
            result.Should().BeSameAs(UnitTestFrameworkReturnedFromShim);
        }
    }
}