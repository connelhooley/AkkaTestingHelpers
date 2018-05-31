using System;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;
using FluentAssertions;
using Xunit;

// ReSharper disable ObjectCreationAsStatement

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.ResolvedTestProbeStoreTests
{
    public class Constructor : TestBase
    {
        [Fact]
        public void ResolvedTestProbeStore_Constructor_DoesNotThrow()
        {
            //act
            Action act = () => new ResolvedTestProbeStore();

            //assert
            act.Should().NotThrow();
        }
    }
}