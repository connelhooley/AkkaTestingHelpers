using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;
using FluentAssertions;
using System;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.DelayerTests
{
    public class Constructor : TestBase
    {
        [Fact]
        public void Delayer_Constructor_DoesNotThrow()
        {
            //act
            Action act = () => new Delayer();

            //assert
            act.Should().NotThrow();
        }
    }
}
