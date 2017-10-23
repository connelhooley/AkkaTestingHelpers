using System;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.TestProbeResolverSettingsTests
{
    public class RegisterHandler : TestBase
    {
        #region Null tests
        [Fact]
        public void TestProbeResolverSettings_RegisterWithFactory_ThrowsArgumentNullException()
        {
            //arrange
            TestProbeResolverSettings sut = TestProbeResolverSettings.Empty;

            //act
            Action act = () => sut.RegisterHandler<DummyActor1, Message1>(null);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }
        #endregion

        [Fact]
        public void TestProbeResolverSettings_RegisterWithFactory_ReturnsNewInstance()
        {
            //arrange
            TestProbeResolverSettings sut = TestProbeResolverSettings.Empty;
            
            //act
            TestProbeResolverSettings result = sut.RegisterHandler<DummyActor1, Message1>(message1 => TestUtils.Create<object>());

            //assert
            result.Should().NotBe(sut);
        }
    }
}