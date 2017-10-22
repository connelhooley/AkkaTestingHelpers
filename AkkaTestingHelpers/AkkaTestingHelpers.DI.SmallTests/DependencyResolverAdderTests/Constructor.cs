using System;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.DependencyResolverAdderTests
{
    public class Constructor : TestBase
    {
        [Fact]
        public void DependencyResolverAdder_Constructor_DoesNotThrow()
        {
            //act
            Action act = () => new DependencyResolverAdder();

            //assert
            act.ShouldNotThrow();
        }
    }
}