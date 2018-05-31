using System;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;
using FluentAssertions;
using Xunit;

// ReSharper disable ObjectCreationAsStatement

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.ConcreteDependencyResolverAdderCreatorTests
{
    public class Constructor
    {
        [Fact]
        public void ConcreteDependencyResolverAdderCreator_Constructor_DoesNotThrow()
        {
            //act
            Action act = () => new ConcreteDependencyResolverAdderCreator();

            //assert
            act.Should().NotThrow();
        }
    }
}