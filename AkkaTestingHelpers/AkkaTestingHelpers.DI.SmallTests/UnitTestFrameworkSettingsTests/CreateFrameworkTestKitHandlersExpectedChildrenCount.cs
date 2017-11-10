using System;
using System.Collections.Immutable;
using Akka.Actor;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.UnitTestFrameworkSettingsTests
{
    public class CreateFrameworkTestKitHandlersExpectedChildrenCount : TestBase
    {
        #region Null tests
        [Fact]
        public void UnitTestFrameworkSettings_CreateFrameworkWithNullTestKit_ThrowsArgumentNullException()
        {
            //arrange
            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings.Empty;

            //act
            Action act = () => sut.CreateFramework<DummyActor1>(null, HandlersPassedIntoSut, ExpectedChildrenCountPassedIntoSut);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void UnitTestFrameworkSettings_CreateFrameworkWithNullHandlers_ThrowsArgumentNullException()
        {
            //arrange
            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings.Empty;

            //act
            Action act = () => sut.CreateFramework<DummyActor1>(TestKitPassedIntoShim, (ImmutableDictionary<(Type, Type), Func<object, object>>)null, ExpectedChildrenCountPassedIntoSut);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void UnitTestFrameworkSettings_CreateFrameworkWithNullTestKitAndHandlers_ThrowsArgumentNullException()
        {
            //arrange
            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings.Empty;

            //act
            Action act = () => sut.CreateFramework<DummyActor1>(null, (ImmutableDictionary<(Type, Type), Func<object, object>>)null, ExpectedChildrenCountPassedIntoSut);

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
            sut.CreateFramework<DummyActor1>(TestKitPassedIntoSut, HandlersPassedIntoSut, ExpectedChildrenCountPassedIntoSut);

            //assert
            UnitTestFrameworkCreatorConstructorCount.Should().Be(1);
        }

        [Fact]
        public void UnitTestFrameworkSettings_CreateFramework_CreatesUnitTestFrameworkWithCorrectHandlers()
        {
            //arrange
            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings.Empty;

            //act
            sut.CreateFramework<DummyActor1>(TestKitPassedIntoSut, HandlersPassedIntoSut, ExpectedChildrenCountPassedIntoSut);

            //assert
            HandlersPassedIntoShim.Should().BeSameAs(HandlersPassedIntoSut);
        }

        [Fact]
        public void UnitTestFrameworkSettings_CreateFramework_CreatesUnitTestFrameworkWithCorrectTestKit()
        {
            //arrange
            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings.Empty;

            //act
            sut.CreateFramework<DummyActor1>(TestKitPassedIntoSut, HandlersPassedIntoSut, ExpectedChildrenCountPassedIntoSut);

            //assert
            TestKitPassedIntoShim.Should().BeSameAs(TestKitPassedIntoSut);
        }

        [Fact]
        public void UnitTestFrameworkSettings_CreateFramework_CreatesUnitTestFrameworkWithCorrectProps()
        {
            //arrange
            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings.Empty;

            //act
            sut.CreateFramework<DummyActor1>(TestKitPassedIntoSut, HandlersPassedIntoSut, ExpectedChildrenCountPassedIntoSut);

            //assert
            PropsPassedIntoShim.Should().Be(Props.Create<DummyActor1>());
        }

        [Fact]
        public void UnitTestFrameworkSettings_CreateFramework_CreatesUnitTestFrameworkWithCorrectExpectedChildrenCount()
        {
            //arrange
            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings.Empty;

            //act
            sut.CreateFramework<DummyActor1>(TestKitPassedIntoSut, HandlersPassedIntoSut, ExpectedChildrenCountPassedIntoSut);

            //assert
            ExpectedChildrenCountPassedIntoShim.Should().Be(ExpectedChildrenCountPassedIntoSut);
        }

        [Fact]
        public void UnitTestFrameworkSettings_CreateFramework_OnlyCreatesOneUnitTestFramework()
        {
            //arrange
            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings.Empty;

            //act
            sut.CreateFramework<DummyActor1>(TestKitPassedIntoSut, HandlersPassedIntoSut, ExpectedChildrenCountPassedIntoSut);

            //assert
            UnitTestFrameworkCreatorCreateCount.Should().Be(1);
        }

        [Fact]
        public void UnitTestFrameworkSettings_CreateFramework_ReturnsCreatedOneUnitTestFramework()
        {
            //arrange
            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings.Empty;

            //act
            UnitTestFramework<DummyActor1> result = sut.CreateFramework<DummyActor1>(TestKitPassedIntoSut, HandlersPassedIntoSut, ExpectedChildrenCountPassedIntoSut);

            //assert
            result.Should().BeSameAs(UnitTestFrameworkReturnedFromShim);
        }
    }
}