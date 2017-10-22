using System;
using Akka.TestKit;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.ConcreteResolverTests
{
    public class CreateSut : TestBase
    {
        #region Null tests
        [Fact]
        public void ConcreteResolver_CreateSutWithNullProps_ThrowsArgumentNullException()
        {
            //arrange   
            ConcreteResolver sut = CreateConcreteResolver();

            //act
            Action act = () => sut.CreateSut<DummyActor>(null, ExpectedChildCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }
        #endregion

        [Fact]
        public void ConcreteResolver_CreateSut_ReturnsCreatedActor()
        {
            //arrange   
            ConcreteResolver sut = CreateConcreteResolver();

            //act
            TestActorRef<DummyActor> result = sut.CreateSut<DummyActor>(Props, ExpectedChildCount);

            //assert
            result.Should().BeSameAs(CreatedSutWithProps);
        }
        
        [Fact]
        public void ConcreteResolver_CreateSutWithNoProps_ReturnsCreatedActor()
        {
            //arrange   
            ConcreteResolver sut = CreateConcreteResolver();
            
            //act
            TestActorRef<DummyActor> result = sut.CreateSut<DummyActor>(ExpectedChildCount);

            //assert
            result.Should().BeSameAs(CreatedSutWithoutProps);
        }
    }
}