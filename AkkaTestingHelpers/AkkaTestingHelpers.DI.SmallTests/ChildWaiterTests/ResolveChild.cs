using System;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;
using FluentAssertions;
using NUnit.Framework;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.ChildWaiterTests
{
    internal class ResolveChild : TestBase
    {
        [Test]
        public void ChildWaiter_NotStarted_ResolveChild_DoesNotThrowAnyExceptions()
        {
            //arrange
            ChildWaiter sut = CreateChildWaiter();

            //act
            Action act = () => sut.ResolvedChild();

            //assert
            act.ShouldNotThrow();
        }

        [Test]
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