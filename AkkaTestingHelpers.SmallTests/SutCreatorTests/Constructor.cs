using System;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.SutCreatorTests
{
    public class Constructor : TestBase
    {
        [Fact]
        public void SutCreator_Constructor_DoesNotThrow()
        {
            //act
            Action act = () => new SutCreator();

            //assert
            act.Should().NotThrow();
        }
    }
}