using System;
using Akka.Actor;
using Akka.TestKit;
using FluentAssertions;
using Moq;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.UnitTestFrameworkTests
{
    public class ResolvedSupervisorStratergy : TestBase
    {
        #region Null tests
        [Fact]
        public void UnitTestFramework_ResolvedSupervisorStratergyWithNullChildName_ThrowsArgumentNullException()
        {
            //arrange
            UnitTestFramework<DummyActor> sut = CreateUnitTestFramework();

            //act
            Action act = () => sut.ResolvedSupervisorStratergy(null);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }
        #endregion

        [Fact]
        public void UnitTestFramework_ResolvedSupervisorStratergyWithChildNameCreatedWithSupervisoryStratergy_ReturnsResolvedSupervisoryStratergy()
        {
            //arrange
            UnitTestFramework<DummyActor> sut = CreateUnitTestFramework();

            //act
            SupervisorStrategy result = sut.ResolvedSupervisorStratergy(ChildName);

            //assert
            result.Should().BeSameAs(ResolvedSupervisorStrategy);
        }

        [Fact]
        public void UnitTestFramework_ResolvedSupervisorStratergyWithChildNameCreatedWithoutSupervisoryStratergy_ReturnsSutSupervisoryStratergy()
        {
            //arrange
            UnitTestFramework<DummyActor> sut = CreateUnitTestFramework();

            //act
            SupervisorStrategy result = sut.ResolvedSupervisorStratergy(ChildNameWithoutSupervisor);

            //assert
            result.Should().BeSameAs(SutSupervisorStrategy);
        }
        
        [Fact]
        public void UnitTestFramework_ResolvedSupervisorStratergyWithChildNameCreatedWithoutSupervisoryStratergy_OnlyCallsSupervisoryStratergyGetterOnce()
        {
            //arrange
            UnitTestFramework<DummyActor> sut = CreateUnitTestFramework();
            sut.ResolvedSupervisorStratergy(ChildNameWithoutSupervisor);

            //act
            sut.ResolvedSupervisorStratergy(ChildNameWithoutSupervisor);

            //assert
            SutSupervisorStrategyGetterMock.Verify(
                getter => getter.Get(It.IsAny<ActorBase>()),
                Times.Once);
        }
    }
}