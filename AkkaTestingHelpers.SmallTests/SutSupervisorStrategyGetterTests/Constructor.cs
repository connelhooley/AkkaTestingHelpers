using System;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;
using FluentAssertions;
using Xunit;

// ReSharper disable ObjectCreationAsStatement

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.SutSupervisorStrategyGetterTests
{
    public class Constructor
    {
        [Fact]
        public void SutSupervisorStrategyGetter_Constructor_DoesNotThrow()
        {
            //act
            Action act = () => new SutSupervisorStrategyGetter();

            //assert
            act.Should().NotThrow();
        }
    }
}