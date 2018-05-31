using System;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;
using FluentAssertions;
using Xunit;

// ReSharper disable ObjectCreationAsStatement

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.UnitTestFrameworkCreatorTests
{
    public class Constructor
    {
        [Fact]
        public void UnitTestFrameworkCreator_Constructor_DoesNotThrow()
        {
            //act
            Action act = () => new UnitTestFrameworkCreator();

            //assert
            act.Should().NotThrow();
        }
    }
}