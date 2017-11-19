using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.BasicResolverSettingsTests
{
    public class RegisterActorGeneric : TestBase
    {
        [Fact]
        public void BasicResolverSettings_RegisterGenericWithoutFactory_ReturnsNewInstance()
        {
            //arrange
            BasicResolverSettings sut = BasicResolverSettings.Empty;

            //act
            BasicResolverSettings result = sut.RegisterActor<DummyActor1>();

            //assert
            result.Should().NotBe(sut);
        }
    }
}