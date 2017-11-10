using System;
using FluentAssertions;
using Moq;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.UnitTestFrameworkTests
{
    public class TellMessageAndWaitForChildrenMessageExpectedChildCount : TestBase
    {
        #region Null tests
        [Fact]
        public void TestProbeResolver_TellMessageAndWaitForChildrenNoSenderWithNullMessage_ThrowsArgumentNullException()
        {
            //arrange
            UnitTestFramework<DummyActor> sut = CreateTestProbeResolver();

            //act
            Action act = () => sut.TellMessageAndWaitForChildren<object>(
                null, 
                ExpectedChildCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }
        #endregion

        [Fact]
        public void TestProbeResolver_TellMessageAndWaitForChildren_TellsChild()
        {
            //arrange
            UnitTestFramework<DummyActor> sut = CreateTestProbeResolver();

            //act
            sut.TellMessageAndWaitForChildren(Message, ExpectedChildCount);

            //assert
            ChildTellerMock.Verify(
                teller => teller.TellMessage(
                    ChildWaiter, 
                    this, 
                    SutActor, 
                    Message, 
                    ExpectedChildCount, 
                    null),
                Times.Once);
        }
    }
}