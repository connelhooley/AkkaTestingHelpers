using System;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.UnitTestFrameworkSettingsTests
{
    public class Empty : TestBase
    {
        [Fact]
        public void TestProbeResolverSettings_Empty_DoesNotThrow()
        {
            //act
            Action act = () => { UnitTestFrameworkSettings sut = UnitTestFrameworkSettings.Empty; };

            //assert
            act.Should().NotThrow();
        }
    }
}