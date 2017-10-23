using ConnelHooley.AkkaTestingHelpers.DI.Actors.Abstract;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.TestProbeActorCreatorTests
{
    public class Create : TestBase
    {
        [Fact]
        public void TestProbeActorCreator_Create_ReturnsTestProbeActor()
        {
            //arrange
            TestProbeActorCreator sut = CreateTestProbeCreator();

            //act
            ITestProbeActor result = sut.Create(
                TestProbeCreatorPassedIntoSut, 
                TestKitPassedIntoSut, 
                HandlersPassedIntoSut);

            //assert
            result.Should().BeSameAs(TestProbeActorReturnedByShim);
        }

        [Fact]
        public void TestProbeActorCreator_Create_ConstructsActorWithCorrectTestProbeCreator()
        {
            //arrange
            TestProbeActorCreator sut = CreateTestProbeCreator();

            //act
            sut.Create(
                TestProbeCreatorPassedIntoSut,
                TestKitPassedIntoSut,
                HandlersPassedIntoSut);

            //assert
            TestProbeCreatorPassedIntoShim.Should().BeSameAs(TestProbeCreatorPassedIntoSut);
        }

        [Fact]
        public void TestProbeActorCreator_Create_ConstructsActorWithCorrectTestKit()
        {
            //arrange
            TestProbeActorCreator sut = CreateTestProbeCreator();

            //act
            sut.Create(
                TestProbeCreatorPassedIntoSut,
                TestKitPassedIntoSut,
                HandlersPassedIntoSut);

            //assert
            TestKitPassedIntoShim.Should().BeSameAs(TestKitPassedIntoSut);
        }
        
        [Fact]
        public void TestProbeActorCreator_Create_ConstructsActorWithCorrectHandlers()
        {
            //arrange
            TestProbeActorCreator sut = CreateTestProbeCreator();

            //act
            sut.Create(
                TestProbeCreatorPassedIntoSut,
                TestKitPassedIntoSut,
                HandlersPassedIntoSut);

            //assert
            HandlersPassedIntoShim.Should().BeSameAs(HandlersPassedIntoSut);
        }

        [Fact]
        public void TestProbeActorCreator_Create_OnlyConstructsOneActor()
        {
            //arrange
            TestProbeActorCreator sut = CreateTestProbeCreator();

            //act
            sut.Create(
                TestProbeCreatorPassedIntoSut,
                TestKitPassedIntoSut,
                HandlersPassedIntoSut);

            //assert
            ShimConstructorCallCount.Should().Be(1);
        }
    }
}