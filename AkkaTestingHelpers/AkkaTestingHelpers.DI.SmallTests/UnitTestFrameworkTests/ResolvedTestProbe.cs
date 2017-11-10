using System;
using Akka.TestKit;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.UnitTestFrameworkTests
{
    public class ResolvedTestProbe : TestBase
    {
        #region Null tests
        [Fact]
        public void TestProbeResolver_ResolvedTestProbeWithNullChildName_ThrowsArgumentNullException()
        {
            //arrange
            UnitTestFramework<DummyActor> sut = CreateTestProbeResolver();

            //act
            Action act = () => sut.ResolvedTestProbe(null);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }
        #endregion

        [Fact]
        public void TestProbeResolver_ResolvedTestProbe_ReturnsCorrectProbe()
        {
            //arrange
            UnitTestFramework<DummyActor> sut = CreateTestProbeResolver();

            //act
            TestProbe result = sut.ResolvedTestProbe(ChildName);

            //assert
            result.Should().BeSameAs(ResolvedTestProbe);
        }
    }
}