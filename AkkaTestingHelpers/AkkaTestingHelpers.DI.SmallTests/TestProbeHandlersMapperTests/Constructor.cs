using System;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;
using FluentAssertions;
using Xunit;

// ReSharper disable ObjectCreationAsStatement

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.TestProbeHandlersMapperTests
{
    public class Constructor : TestBase
    {
        [Fact]
        public void TestProbeHandlersMapper_Constructor_DoesNotThrow()
        {
            //act
            Action act = () => new TestProbeHandlersMapper();

            //assert
            act.ShouldNotThrow();
        }
    }
}