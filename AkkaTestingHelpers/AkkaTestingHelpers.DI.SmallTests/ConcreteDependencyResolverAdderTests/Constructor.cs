using System;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.ConcreteDependencyResolverAdderTests
{
    public class Constructor
    {
        [Fact]
        public void ConcreteDependencyResolverAdder_Constructor_DoesNotThrow()
        {
            //act
            Action act = () => new ConcreteDependencyResolverAdder();

            //assert
            act.ShouldNotThrow();
        }
    }
}