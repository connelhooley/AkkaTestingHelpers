using System;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.TestProbeActorCreatorTests
{
    public class Constructor : TestBase
    {
        [Fact]
        public void TestProbeActorCreator_Constructor_DoesNotThrow()
        {
            //act
            Action act = () => new TestProbeActorCreator();

            //assert
            act.ShouldNotThrow();
        }
    }
}