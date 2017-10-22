using System;
using Akka.Actor;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.TestProbeCreatorTests
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
            act.ShouldThrow<ArgumentNullException>();
        }
        #endregion

        [Fact]
        public void TestProbeCreator_Create_DoesNotThrowException()
        {
            //arrange
            TestProbeCreator sut = CreateTestProbeCreator();

            //act
            Action act = () => sut.Create(this);

            //assert
            act.ShouldNotThrow();
        }

        [Fact]
        public void TestProbeCreator_Create_ReturnsTestProbeWithCorrectSystem()
        {
            //arrange
            TestProbeCreator sut = CreateTestProbeCreator();

            //act
            TestProbe result = sut.Create(this);

            //assert
            result.Sys.Should().BeSameAs(Sys);
        }
        
        [Fact]
        public void TestProbeCreator_Create_ReturnsTestProbeWithCorrectTestKitSettings()
        {
            //arrange
            TestProbeCreator sut = CreateTestProbeCreator();

            //act
            TestProbe result = sut.Create(this);

            //assert
            result.TestKitSettings.Should().BeSameAs(TestKitSettings);
        }

        [Fact]
        public void TestProbeCreator_Create_ReturnsWorkingTestProbe()
        {
            //arrange
            TestProbeCreator sut = CreateTestProbeCreator();

            //act
            TestProbe result = sut.Create(this);

            //assert
            object message = TestUtils.Create<object>();
            result.Tell(message);
            result.ExpectMsg(message);
        }

        [Fact]
        public void TestProbeCreator_Create_CreatesTestProbeFromTestkit()
        {
            //arrange
            TestProbeCreator sut = CreateTestProbeCreator();

            //act
            TestProbe result = sut.Create(this);

            //todo shims
        }
    }
}