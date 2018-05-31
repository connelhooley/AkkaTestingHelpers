using System;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.TestProbeDependencyResolverAdderTests
{
    public class Constructor : TestBase
    {
        [Fact]
        public void TestProbeDependencyResolverAdder_Constructor_DoesNotThrow()
        {
            //act
            Action act = () => new TestProbeDependencyResolverAdder();

            //assert
            act.Should().NotThrow();
        }
    }
}