using System;
using FluentAssertions;
using Moq;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.UnitTestFrameworkTests
{
    public class TellMessageAndWaitForExceptionMessageSender : TestBase
    {
        #region Null tests
        [Fact]
        public void UnitTestFramework_TellMessageAndWaitForExceptionWithNullMessage_ThrowsArgumentNullException()
        {
            //arrange
            UnitTestFramework<DummyActor> sut = CreateUnitTestFramework();

            //act
            Action act = () => sut.TellMessageAndWaitForException<object>(
                null,
                SutActor);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void UnitTestFramework_TellMessageAndWaitForExceptionWithNullSender_ThrowsArgumentNullException()
        {
            //arrange
            UnitTestFramework<DummyActor> sut = CreateUnitTestFramework();

            //act
            Action act = () => sut.TellMessageAndWaitForException(
                Message,
                null);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void UnitTestFramework_TellMessageAndWaitForExceptionWithNullMessageAndSender_ThrowsArgumentNullException()
        {
            //arrange
            UnitTestFramework<DummyActor> sut = CreateUnitTestFramework();

            //act
            Action act = () => sut.TellMessageAndWaitForException<object>(
                null,
                null);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }
        #endregion

        [Fact]
        public void UnitTestFramework_TellMessageAndWaitForException_InvokesTellWaiter()
        {
            //arrange
            UnitTestFramework<DummyActor> sut = CreateUnitTestFramework();

            //act
            sut.TellMessageAndWaitForException(Message, Sender);

            //assert
            TellWaiterMock.Verify(
                teller => teller.TellMessage(
                    ExceptionWaiter,
                    this,
                    SutActor,
                    Message,
                    1,
                    Sender),
                Times.Once);
        }
    }
}