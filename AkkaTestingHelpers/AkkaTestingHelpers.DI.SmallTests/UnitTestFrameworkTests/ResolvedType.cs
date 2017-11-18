using System;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.UnitTestFrameworkTests
{
    public class ResolvedType : TestBase
    {
        #region Null tests
        [Fact]
        public void UnitTestFramework_ResolvedTypeWithNullChildName_ThrowsArgumentNullException()
        {
            //arrange
            UnitTestFramework<DummyActor> sut = CreateUnitTestFramework();

            //act
            Action act = () => sut.ResolvedType(null);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }
        #endregion

        [Fact]
        public void UnitTestFramework_ResolvedType_ReturnsCorrectProbe()
        {
            //arrange
            UnitTestFramework<DummyActor> sut = CreateUnitTestFramework();

            //act
            Type result = sut.ResolvedType(ChildName);

            //assert
            result.Should().BeSameAs(ResolvedType);
        }
    }
}