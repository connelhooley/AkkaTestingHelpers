using System;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.BasicResolverSettingsTests
{
    public class RegisterActorFactory : TestBase
    {
        #region Null tests
        [Fact]
        public void BasicResolverSettings_RegisterWithFactory_ThrowsArgumentNullException()
        {
            //arrange
            BasicResolverSettings sut = BasicResolverSettings.Empty;

            //act
            Action act = () => sut.RegisterActor<DummyActor1>(null);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }
        #endregion

        [Fact]
        public void BasicResolverSettings_RegisterWithFactory_ReturnsNewInstance()
        {
            //arrange
            BasicResolverSettings sut = BasicResolverSettings.Empty;

            //act
            BasicResolverSettings result = sut.RegisterActor(() => new DummyActor1());

            //assert
            result.Should().NotBe(sut);
        }
    }
}