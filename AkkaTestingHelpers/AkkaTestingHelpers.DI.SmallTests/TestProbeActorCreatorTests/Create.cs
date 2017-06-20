using System;
using ConnelHooley.AkkaTestingHelpers.DI.Actors.Abstract;
using ConnelHooley.AkkaTestingHelpers.DI.Actors.Concrete;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.TestProbeActorCreatorTests
{
    public class Create : TestBase
    {
        [Fact]
        public void TestProbeActorCreator_CreateWithNullTestKitBase_ThrowsArgumentNullException()
        {
            //arrange
            TestProbeActorCreator sut = CreateTestProbeActorFactory();

            //act
            Action act = () => sut.Create(null);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeActorCreator_Create_ReturnsTestProbeActor()
        {
            //arrange
            TestProbeActorCreator sut = CreateTestProbeActorFactory();

            //act
            ITestProbeActor result = sut.Create(this);

            //assert
            result.Should().BeOfType<TestProbeActor>();
        }

        [Fact]
        public void TestProbeActorCreator_Create_UsesCorrectTestKitSettings()
        {
            //arrange
            TestProbeActorCreator sut = CreateTestProbeActorFactory();

            //act
            ITestProbeActor result = sut.Create(this);

            //assert
            result.TestProbe.TestKitSettings.Should().BeSameAs(TestKitSettings);
        }
    }
}