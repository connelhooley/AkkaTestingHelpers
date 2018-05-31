using System;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.TestProbeCreatorTests
{
    public class Constructor : TestBase
    {
        [Fact]
        public void TestProbeCreator_Constructor_DoesNotThrow()
        {
            //act
            Action act = () => new TestProbeCreator();

            //assert
            act.Should().NotThrow();
        }
    }
}