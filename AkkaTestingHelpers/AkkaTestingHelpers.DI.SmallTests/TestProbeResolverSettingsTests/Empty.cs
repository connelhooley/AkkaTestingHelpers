using System;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.TestProbeResolverSettingsTests
{
    public class Empty : TestBase
    {
        [Fact]
        public void TestProbeResolverSettings_Empty_DoesNotThrow()
        {
            //act
            Action act = () => { TestProbeResolverSettings sut = TestProbeResolverSettings.Empty; };

            //assert
            act.ShouldNotThrow();
        }
    }
}