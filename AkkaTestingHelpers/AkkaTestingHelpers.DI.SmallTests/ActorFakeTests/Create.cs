using System;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.ActorFakeTests
{
    public class Create : TestBase
    {
        [Fact]
        public void FakeActor_Create_DoesNotThrow()
        {
            //act
            Action act = () => ActorFake.Create();

            //assert
            act.ShouldNotThrow();
        }
    }
}