using System;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.UnitTestFrameworkTests
{
    public class ResolvedType : TestBase
    {
        #region Null tests
        [Fact]
        public void TestProbeResolver_ResolvedTypeWithNullChildName_ThrowsArgumentNullException()
        {
            //arrange
            UnitTestFramework<DummyActor> sut = CreateTestProbeResolver();

            //act
            Action act = () => sut.ResolvedType(null);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }
        #endregion

        [Fact]
        public void TestProbeResolver_ResolvedType_ReturnsCorrectProbe()
        {
            //arrange
            UnitTestFramework<DummyActor> sut = CreateTestProbeResolver();

            //act
            Type result = sut.ResolvedType(ChildName);

            //assert
            result.Should().BeSameAs(ResolvedType);
        }
    }
}