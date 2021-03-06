﻿using System;
using Akka.Actor;
using FluentAssertions;
using Moq;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.UnitTestFrameworkTests
{
    public class ResolvedSupervisorStrategy : TestBase
    {
        #region Null tests
        [Fact]
        public void UnitTestFramework_ResolvedSupervisorStratergyWithNullChildName_ThrowsArgumentNullException()
        {
            //arrange
            UnitTestFramework<DummyActor> sut = CreateUnitTestFramework();

            //act
            Action act = () => sut.ResolvedSupervisorStrategy(null);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }
        #endregion

        [Fact]
        public void UnitTestFramework_ResolvedSupervisorStratergyWithChildNameCreatedWithSupervisoryStratergy_ReturnsResolvedSupervisoryStratergy()
        {
            //arrange
            UnitTestFramework<DummyActor> sut = CreateUnitTestFramework();

            //act
            SupervisorStrategy result = sut.ResolvedSupervisorStrategy(ChildName);

            //assert
            result.Should().BeSameAs(ResolvedSupervisorStrategy);
        }

        [Fact]
        public void UnitTestFramework_ResolvedSupervisorStratergyWithChildNameCreatedWithoutSupervisoryStratergy_ReturnsSutSupervisoryStratergy()
        {
            //arrange
            UnitTestFramework<DummyActor> sut = CreateUnitTestFramework();

            //act
            SupervisorStrategy result = sut.ResolvedSupervisorStrategy(ChildNameWithoutSupervisor);

            //assert
            result.Should().BeSameAs(SutSupervisorStrategy);
        }
        
        [Fact]
        public void UnitTestFramework_ResolvedSupervisorStratergyWithChildNameCreatedWithoutSupervisoryStratergy_OnlyCallsSupervisoryStratergyGetterOnce()
        {
            //arrange
            UnitTestFramework<DummyActor> sut = CreateUnitTestFramework();
            sut.ResolvedSupervisorStrategy(ChildNameWithoutSupervisor);

            //act
            sut.ResolvedSupervisorStrategy(ChildNameWithoutSupervisor);

            //assert
            SutSupervisorStrategyGetterMock.Verify(
                getter => getter.Get(It.IsAny<ActorBase>()),
                Times.Once);
        }
    }
}