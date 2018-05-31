using System;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;
using FluentAssertions;
using Xunit;

// ReSharper disable ObjectCreationAsStatement

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.ConcreteDependencyResolverAdderTests
{
    public class Constructor : TestBase
    {
        [Fact]
        public void ConcreteDependencyResolverAdder_ConstructorWithNullDependencyResolverAdder_ThrowsArgumentNullException()
        {
            //act
            Action act = () => new ConcreteDependencyResolverAdder(null);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ConcreteDependencyResolverAdder_Constructor_DoesNotThrow()
        {
            //act
            Action act = () => new ConcreteDependencyResolverAdder(DependencyResolverAdder);

            //assert
            act.Should().NotThrow();
        }
    }
}