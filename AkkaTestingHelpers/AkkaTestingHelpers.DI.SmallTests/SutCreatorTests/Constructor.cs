using System;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.SutCreatorTests
{
    public class Constructor : TestBase
    {
        [Fact]
        public void SutCreator_Constructor_DoesNotThrow()
        {
            //act
            Action act = () => new SutCreator();

            //assert
            act.ShouldNotThrow();
        }
    }
}