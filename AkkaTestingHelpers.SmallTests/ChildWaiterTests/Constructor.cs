using System;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;
using FluentAssertions;
using Xunit;

// ReSharper disable ObjectCreationAsStatement

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.ChildWaiterTests
{
    public class Constructor
    {
        [Fact]
        public void ChildWaiter_Constructor_DoesNotThrow()
        {
            //act
            Action act = () => new ChildWaiter();

            //assert
            act.Should().NotThrow();
        }
    }
}