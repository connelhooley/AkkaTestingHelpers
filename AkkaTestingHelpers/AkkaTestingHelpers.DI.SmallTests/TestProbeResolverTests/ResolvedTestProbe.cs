using System;
using Akka.TestKit;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.TestProbeResolverTests
{
    public class ResolvedTestProbe : TestBase
    {
        #region Null tests
        [Fact]
        public void TestProbeResolver_ResolvedTestProbeWithNullParentActor_ThrowsArgumentNullException()
        {
            //arrange
            TestProbeResolver sut = CreateTestProbeResolver();

            //act
            Action act = () => sut.ResolvedTestProbe(null, ChildName);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeResolver_ResolvedTestProbeWithNullChildName_ThrowsArgumentNullException()
        {
            //arrange
            TestProbeResolver sut = CreateTestProbeResolver();

            //act
            Action act = () => sut.ResolvedTestProbe(TestActor, null);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeResolver_ResolvedTestProbeWithAllNulls_ThrowsArgumentNullException()
        {
            //arrange
            TestProbeResolver sut = CreateTestProbeResolver();

            //act
            Action act = () => sut.ResolvedTestProbe(null, null);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }
        #endregion

        [Fact]
        public void TestProbeResolver_ResolvedTestProbe_ReturnsCorrectProbe()
        {
            //arrange
            TestProbeResolver sut = CreateTestProbeResolver();

            //act
            TestProbe result = sut.ResolvedTestProbe(TestActor, ChildName);

            //assert
            result.Should().BeSameAs(ResolvedTestProbe);
        }
    }
}