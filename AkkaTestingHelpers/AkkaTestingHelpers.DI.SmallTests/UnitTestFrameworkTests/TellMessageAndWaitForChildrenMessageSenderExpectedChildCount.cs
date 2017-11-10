using System;
using FluentAssertions;
using Moq;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.UnitTestFrameworkTests
{
    public class TellMessageAndWaitForChildrenMessageSenderExpectedChildCount : TestBase
    {
        #region Null tests
        [Fact]
        public void TestProbeResolver_TellMessageAndWaitForChildrenWithNullMessage_ThrowsArgumentNullException()
        {
            //arrange
            UnitTestFramework<DummyActor> sut = CreateTestProbeResolver();

            //act
            Action act = () => sut.TellMessageAndWaitForChildren<object>(
                null, 
                SutActor, 
                ExpectedChildCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeResolver_TellMessageAndWaitForChildrenWithNullSender_ThrowsArgumentNullException()
        {
            //arrange
            UnitTestFramework<DummyActor> sut = CreateTestProbeResolver();

            //act
            Action act = () => sut.TellMessageAndWaitForChildren(
                Message,
                null,
                ExpectedChildCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeResolver_TellMessageAndWaitForChildrenWithNullMessageAndSender_ThrowsArgumentNullException()
        {
            //arrange
            UnitTestFramework<DummyActor> sut = CreateTestProbeResolver();

            //act
            Action act = () => sut.TellMessageAndWaitForChildren<object>(
                null, 
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
            sut.TellMessageAndWaitForChildren(Message, Sender, ExpectedChildCount);

            //assert
            ChildTellerMock.Verify(
                teller => teller.TellMessage(
                    ChildWaiter, 
                    this, 
                    SutActor, 
                    Message, 
                    ExpectedChildCount, 
                    Sender),
                Times.Once);
        }
    }
}