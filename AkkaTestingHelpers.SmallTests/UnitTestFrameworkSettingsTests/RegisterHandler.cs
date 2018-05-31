using System;
using ConnelHooley.TestHelpers;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.UnitTestFrameworkSettingsTests
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
            Action act = () => sut.RegisterHandler<DummyChildActor1, Message1>(null);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }
        #endregion

        [Fact]
        public void UnitTestFrameworkSettings_RegisterWithFactory_ReturnsNewInstance()
        {
            //arrange
            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings
                .Empty
                .RegisterHandler<DummyChildActor1, Message1>(message1 => TestHelper.Generate<object>());

            //act
            UnitTestFrameworkSettings result = sut.RegisterHandler<DummyChildActor2, Message1>(message1 => TestHelper.Generate<object>());

            //assert
            result.Should().NotBe(sut);
        }
    }
}