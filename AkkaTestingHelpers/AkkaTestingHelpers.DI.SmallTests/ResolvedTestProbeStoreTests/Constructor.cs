using System;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;
using FluentAssertions;
using Xunit;
// ReSharper disable ObjectCreationAsStatement

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.ResolvedTestProbeStoreTests
{
    public class Constructor : TestBase
    {
        [Fact]
        public void ResolvedTestProbeStore_Constructor_DoesNotThrow()
        {
            //act
            Action act = () => new ResolvedTestProbeStore();

            //assert
            act.ShouldNotThrow();
        }
    }
}