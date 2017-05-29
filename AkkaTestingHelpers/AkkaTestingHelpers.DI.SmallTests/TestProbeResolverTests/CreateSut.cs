using System;
using Akka.TestKit;
using Akka.TestKit.TestActors;
using FluentAssertions;
using NUnit.Framework;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.TestProbeResolverTests
{
    internal class CreateSut : TestBase
    {
        [Test]
        public void TestProbeResolver_CreateSutWithNullProps_ThrowsArgumentNullException()
        {
            //arrange   
            TestProbeResolver sut = CreateTestProbeResolver(TestProbeResolverSettings.Empty);

            //act
            Action act = () => sut.CreateSut<BlackHoleActor>(null, ExpectedChildrenCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void TestProbeResolver_CreateSut_ReturnsCreatedActor()
        {
            //arrange   
            TestProbeResolver sut = CreateTestProbeResolver(TestProbeResolverSettings.Empty);

            //act
            TestActorRef<BlackHoleActor> result = sut.CreateSut<BlackHoleActor>(Props, ExpectedChildrenCount);

            //assert
            result.Should().BeSameAs(CreatedActor);
        }
        
        [Test]
        public void TestProbeResolver_CreateSutWithNoProps_ReturnsCreatedActor()
        {
            //arrange   
            TestProbeResolver sut = CreateTestProbeResolver(TestProbeResolverSettings.Empty);

            //act
            TestActorRef<BlackHoleActor> result = sut.CreateSut<BlackHoleActor>(ExpectedChildrenCount);

            //assert
            result.Should().BeSameAs(CreatedActorNoProps);
        }
    }
}