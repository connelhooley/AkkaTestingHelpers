using System;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.UnitTestFrameworkSettingsTests
{
    public class RegisterHandler : TestBase
    {
        #region Null tests
        [Fact]
        public void UnitTestFrameworkSettings_RegisterWithFactory_ThrowsArgumentNullException()
        {
            //arrange
            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings.Empty;

            //act
            Action act = () => sut.RegisterHandler<DummyActor1, Message1>(null);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }
        #endregion

        [Fact]
        public void UnitTestFrameworkSettings_RegisterWithFactory_ReturnsNewInstance()
        {
            //arrange
            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings
                .Empty
                .RegisterHandler<DummyActor1, Message1>(message1 => TestUtils.Create<object>());

            //act
            UnitTestFrameworkSettings result = sut.RegisterHandler<DummyActor2, Message1>(message1 => TestUtils.Create<object>());

            //assert
            result.Should().NotBe(sut);
        }
    }
}