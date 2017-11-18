using System;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;
using FluentAssertions;
using Xunit;
// ReSharper disable ObjectCreationAsStatement

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