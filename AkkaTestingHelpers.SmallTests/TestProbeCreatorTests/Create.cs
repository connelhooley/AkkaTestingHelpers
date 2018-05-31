using System;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.TestProbeCreatorTests
{
    public class Create : TestBase
    {
        #region Null tests
        [Fact]
        public void TestProbeCreator_CreateWithNullTestKitBase_ThrowsArgumentNullException()
        {
            //arrange
            TestProbeCreator sut = CreateTestProbeCreator();

            //act
            Action act = () => sut.Create(null);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }
        #endregion
        
        [Fact]
        public void TestProbeCreator_Create_ReturnsTestProbeFromTestKit()
        {
            //arrange
            TestProbeCreator sut = CreateTestProbeCreator();

            //act
            TestProbe result = sut.Create(TestKitBase);

            //assert
            result.Should().BeSameAs(TestProbeReturnedFromShim);
        }
        
        [Fact]
        public void TestProbeCreator_Create_CreatesTestProbeWithNoName()
        {
            //arrange
            TestProbeCreator sut = CreateTestProbeCreator();

            //act
            sut.Create(TestKitBase);

            //assert
            NamePassedIntoShim.Should().BeNull();
        }

        [Fact]
        public void TestProbeCreator_Create_OnlyCreatesOneTestProbe()
        {
            //arrange
            TestProbeCreator sut = CreateTestProbeCreator();

            //act
            sut.Create(TestKitBase);

            //assert
            CallCount.Should().Be(1);
        }

        [Fact]
        public void TestProbeCreator_Create_CreatesANewTestProbeEveryTime()
        {
            //arrange
            TestProbeCreator sut = CreateTestProbeCreator();
            TestProbe result1 = sut.Create(TestKitBase);

            //act
            TestProbe result2 = sut.Create(TestKitBase);

            //assert
            result1.Should().NotBeSameAs(result2);
        }
    }
}