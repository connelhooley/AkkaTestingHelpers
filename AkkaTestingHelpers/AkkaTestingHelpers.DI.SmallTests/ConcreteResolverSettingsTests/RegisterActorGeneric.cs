using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.ConcreteResolverSettingsTests
{
    public class RegisterActorGeneric : TestBase
    {
        [Fact]
        public void ConcreteResolverSettings_RegisterGenericWithoutFactory_ReturnsNewInstance()
        {
            //arrange
            ConcreteResolverSettings sut = ConcreteResolverSettings.Empty;

            //act
            ConcreteResolverSettings result = sut.RegisterActor<DummyActor1>();

            //assert
            result.Should().NotBe(sut);
        }
    }
}