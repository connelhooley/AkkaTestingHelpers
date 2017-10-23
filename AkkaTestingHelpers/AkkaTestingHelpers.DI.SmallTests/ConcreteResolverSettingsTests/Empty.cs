using System;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.ConcreteResolverSettingsTests
{
    public class Empty : TestBase
    {
        [Fact]
        public void ConcreteResolverSettings_Empty_DoesNotThrow()
        {
            //act
            Action act = () => { ConcreteResolverSettings sut = ConcreteResolverSettings.Empty; };

            //assert
            act.ShouldNotThrow();
        }
    }
}