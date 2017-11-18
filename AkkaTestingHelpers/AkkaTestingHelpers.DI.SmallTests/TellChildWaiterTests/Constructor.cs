using System;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.TellChildWaiterTests
{
    public class Constructor : TestBase
    {
        [Fact]
        public void ChildTeller_Constructor_DoesNotThrow()
        {
            //act
            Action act = () => new TellChildWaiter();

            //assert
            act.ShouldNotThrow();
        }
    }
}