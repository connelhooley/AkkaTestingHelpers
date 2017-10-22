using System;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.TestProbeDependencyResolverAdderTests
{
    public class Constructor : TestBase
    {
        [Fact]
        public void TestProbeDependencyResolverAdder_Constructor_DoesNotThrow()
        {
            //act
            Action act = () => new TestProbeDependencyResolverAdder();

            //assert
            act.ShouldNotThrow();
        }
    }
}