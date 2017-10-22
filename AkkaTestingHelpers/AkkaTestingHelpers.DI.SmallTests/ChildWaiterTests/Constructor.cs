using System;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.ChildWaiterTests
{
    public class Constructor
    {
        [Fact]
        public void ChildWaiter_Constructor_DoesNotThrow()
        {
            //act
            Action act = () => new ChildWaiter();

            //assert
            act.ShouldNotThrow();
        }
    }
}