using System;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.TestProbeParentActorCreatorTests
{
    public class Constructor : TestBase
    {
        [Fact]
        public void TestProbeParentActorCreator_Constructor_DoesNotThrow()
        {
            //act
            Action act = () => new TestProbeParentActorCreator();

            //assert
            act.Should().NotThrow();
        }
    }
}