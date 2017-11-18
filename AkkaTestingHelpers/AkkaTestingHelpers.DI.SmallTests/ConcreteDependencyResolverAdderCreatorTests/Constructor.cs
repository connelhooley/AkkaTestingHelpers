using System;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;
using FluentAssertions;
using Xunit;
// ReSharper disable ObjectCreationAsStatement

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.ConcreteDependencyResolverAdderCreatorTests
{
    public class Constructor
    {
        [Fact]
        public void ConcreteDependencyResolverAdderCreator_Constructor_DoesNotThrow()
        {
            //act
            Action act = () => new ConcreteDependencyResolverAdderCreator();

            //assert
            act.ShouldNotThrow();
        }
    }
}