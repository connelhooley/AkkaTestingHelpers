using System;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;
using FluentAssertions;
using Xunit;

// ReSharper disable ObjectCreationAsStatement

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.TestProbeChildHandlersMapperTests
{
    public class Constructor : TestBase
    {
        [Fact]
        public void TestProbeChildHandlersMapper_Constructor_DoesNotThrow()
        {
            //act
            Action act = () => new TestProbeChildHandlersMapper();

            //assert
            act.Should().NotThrow();
        }
    }
}