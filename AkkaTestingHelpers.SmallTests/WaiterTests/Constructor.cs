using System;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;
using FluentAssertions;
using Xunit;

// ReSharper disable ObjectCreationAsStatement

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.WaiterTests
{
    public class Constructor
    {
        [Fact]
        public void Waiter_Constructor_DoesNotThrow()
        {
            //act
            Action act = () => new Waiter();

            //assert
            act.Should().NotThrow();
        }
    }
}