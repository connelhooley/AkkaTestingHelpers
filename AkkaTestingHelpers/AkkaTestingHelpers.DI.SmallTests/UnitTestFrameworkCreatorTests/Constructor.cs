using System;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;
using FluentAssertions;
using Xunit;
// ReSharper disable ObjectCreationAsStatement

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.UnitTestFrameworkCreatorTests
{
    public class Constructor
    {
        [Fact]
        public void UnitTestFrameworkCreator_Constructor_DoesNotThrow()
        {
            //act
            Action act = () => new UnitTestFrameworkCreator();

            //assert
            act.ShouldNotThrow();
        }
    }
}