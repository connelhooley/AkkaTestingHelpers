using System;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.ChildWaiterTests
{
    public class ResolveChild : TestBase
    {
        [Fact]
        public void ChildWaiter_NotStarted_ResolveChild_DoesNotThrowAnyExceptions()
        {
            //arrange
            ChildWaiter sut = CreateChildWaiter();

            //act
            Action act = () => sut.ResolvedChild();

            //assert
            act.ShouldNotThrow();
        }

        [Fact]
        public void ChildWaiter_Started_ResolveChild_DoesNotThrowAnyExceptions()
        {
            //arrange
            ChildWaiter sut = CreateChildWaiter();
            sut.Start(this, TestUtils.Create<int>());

            //act
            Action act = () => sut.ResolvedChild();

            //assert
            act.ShouldNotThrow();
        }
    }
}