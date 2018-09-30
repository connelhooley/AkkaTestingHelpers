using System;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;
using ConnelHooley.TestHelpers;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.WaiterTests
{
    public class ResolveChild : TestBase
    {
        [Fact]
        public void Waiter_NotStarted_ResolveEvent_DoesNotThrowAnyExceptions()
        {
            //arrange
            Waiter sut = CreateWaiter();

            //act
            Action act = () => sut.ResolveEvent();

            //assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Waiter_Started_ResolveEvent_DoesNotThrowAnyExceptions()
        {
            //arrange
            Waiter sut = CreateWaiter();
            sut.Start(this, TestHelper.GenerateNumber());

            //act
            Action act = () => sut.ResolveEvent();

            //assert
            act.Should().NotThrow();
        }
    }
}