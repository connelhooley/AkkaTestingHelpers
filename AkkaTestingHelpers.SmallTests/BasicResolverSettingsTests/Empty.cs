using System;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.BasicResolverSettingsTests
{
    public class Empty : TestBase
    {
        [Fact]
        public void BasicResolverSettings_Empty_DoesNotThrow()
        {
            //act
            Action act = () => { BasicResolverSettings sut = BasicResolverSettings.Empty; };

            //assert
            act.ShouldNotThrow();
        }
    }
}