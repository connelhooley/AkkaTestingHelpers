using Akka.TestKit;
using Akka.TestKit.TestActors;
using FluentAssertions;
using NUnit.Framework;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.ConcreteResolverTests
{
    internal class CreateSut : TestBase
    {
        [Test]
        public void ConcreteResolver_CreateSut_ReturnsCreatedActor()
        {
            //arrange   
            ConcreteResolver sut = CreateConcreteResolver(ConcreteResolverSettings.Empty);

            //act
            TestActorRef<BlackHoleActor> result = sut.CreateSut<BlackHoleActor>(Props, ExpectedChildrenCount);

            //assert
            result.Should().BeSameAs(CreatedActor);
        }
        
        [Test]
        public void ConcreteResolver_CreateSutWithNoProps_ReturnsCreatedActor()
        {
            //arrange   
            ConcreteResolver sut = CreateConcreteResolver(ConcreteResolverSettings.Empty);

            //act
            TestActorRef<BlackHoleActor> result = sut.CreateSut<BlackHoleActor>(ExpectedChildrenCount);

            //assert
            result.Should().BeSameAs(CreatedActorNoProps);
        }
    }
}