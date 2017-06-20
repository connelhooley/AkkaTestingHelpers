using System;
using Akka.TestKit;
using Akka.TestKit.TestActors;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.TestProbeResolverTests
{
    internal class CreateSut : TestBase
    {
        [Fact]
        public void TestProbeResolver_CreateSutWithNullProps_ThrowsArgumentNullException()
        {
            //arrange   
            TestProbeResolver sut = CreateTestProbeResolver();

            //act
            Action act = () => sut.CreateSut<BlackHoleActor>(null, ExpectedChildrenCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeResolver_CreateSut_ReturnsCreatedActor()
        {
            //arrange   
            TestProbeResolver sut = CreateTestProbeResolver();

            //act
            TestActorRef<BlackHoleActor> result = sut.CreateSut<BlackHoleActor>(Props, ExpectedChildrenCount);

            //assert
            result.Should().BeSameAs(CreatedActor);
        }
        
        [Fact]
        public void TestProbeResolver_CreateSutWithNoProps_ReturnsCreatedActor()
        {
            //arrange   
            TestProbeResolver sut = CreateTestProbeResolver();

            //act
            TestActorRef<BlackHoleActor> result = sut.CreateSut<BlackHoleActor>(ExpectedChildrenCount);

            //assert
            result.Should().BeSameAs(CreatedActorNoProps);
        }
    }
}