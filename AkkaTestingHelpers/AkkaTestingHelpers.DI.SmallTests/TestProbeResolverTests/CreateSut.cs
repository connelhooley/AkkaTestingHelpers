using System;
using Akka.TestKit;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.TestProbeResolverTests
{
    public class CreateSut : TestBase
    {
        #region Null tests
        [Fact]
        public void TestProbeResolver_CreateSutWithNullProps_ThrowsArgumentNullException()
        {
            //arrange   
            TestProbeResolver sut = CreateTestProbeResolver();

            //act
            Action act = () => sut.CreateSut<DummyActor>(null, ExpectedChildCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }
        #endregion

        [Fact]
        public void TestProbeResolver_CreateSut_ReturnsCreatedActor()
        {
            //arrange   
            TestProbeResolver sut = CreateTestProbeResolver();

            //act
            TestActorRef<DummyActor> result = sut.CreateSut<DummyActor>(Props, ExpectedChildCount);

            //assert
            result.Should().BeSameAs(CreatedActor);
        }
        
        [Fact]
        public void TestProbeResolver_CreateSutWithNoProps_ReturnsCreatedActor()
        {
            //arrange   
            TestProbeResolver sut = CreateTestProbeResolver();

            //act
            TestActorRef<DummyActor> result = sut.CreateSut<DummyActor>(ExpectedChildCount);

            //assert
            result.Should().BeSameAs(CreatedActorNoProps);
        }
    }
}