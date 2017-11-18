using System;
using Akka.Actor;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.SutSupervisorStrategyGetterTests
{
    public class Get : TestBase
    {
        #region Null tests
        [Fact]
        public void SutSupervisorStrategyGetter_GetWithNullActor_ThrowsArgumentNullException()
        {
            //arrange
            SutSupervisorStrategyGetter sut = CreateSutSupervisorStrategyGetter();

            //act
            Action act = () => sut.Get(null);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }
        #endregion

        [Fact]
        public void SutSupervisorStrategyGetter_GetWithActorWithNonDefaultSupervisorStrategy_ReturnsCorrectSupervisorStrategy()
        {
            //arrange
            SutSupervisorStrategyGetter sut = CreateSutSupervisorStrategyGetter();

            //act
            SupervisorStrategy result = sut.Get(ActorWithNonDefaultSupervisorStrategy);

            //assert
            result.Should().BeSameAs(SupervisorStrategy);
        }
        
        [Fact]
        public void SutSupervisorStrategyGetter_GetWithActorWithDefaultSupervisorStrategy_ReturnsCorrectSupervisorStrategy()
        {
            //arrange
            SutSupervisorStrategyGetter sut = CreateSutSupervisorStrategyGetter();

            //act
            SupervisorStrategy result = sut.Get(ActorWithDefaultSupervisorStrategy);

            //assert
            result.Should().BeSameAs(SupervisorStrategy.DefaultStrategy);
        }
    }
}