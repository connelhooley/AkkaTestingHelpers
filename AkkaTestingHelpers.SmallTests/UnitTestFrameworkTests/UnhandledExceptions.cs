using FluentAssertions;
using Xunit;
using System.Collections.Generic;
using System;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.UnitTestFrameworkTests
{
    public class UnhandledExceptions : TestBase
    {
        [Fact]
        public void UnitTestFramework_UnhandledExceptions_ReturnsUnhandledExceptions()
        {
            //arrange
            UnitTestFramework<DummyActor> sut = CreateUnitTestFramework();

            //act
            IEnumerable<Exception> result = sut.UnhandledExceptions;

            //assert
            result.Should().BeSameAs(TestProbeParentActorUnhandledExceptions);
        }
    }
}