using System;
using FluentAssertions;
using Moq;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.UnitTestFrameworkTests
{
    public class TellMessageAndWaitForExceptionsMessageSenderWaitForExceptionCount : TestBase
    {
        #region Null tests
        [Fact]
        public void UnitTestFramework_TellMessageAndWaitForExceptionsWithNullMessage_ThrowsArgumentNullException()
        {
            //arrange
            UnitTestFramework<DummyActor> sut = CreateUnitTestFramework();

            //act
            Action act = () => sut.TellMessageAndWaitForExceptions<object>(
                null,
                SutActor,
                ExpectedExceptionCount);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void UnitTestFramework_TellMessageAndWaitForExceptionsWithNullSender_ThrowsArgumentNullException()
        {
            //arrange
            UnitTestFramework<DummyActor> sut = CreateUnitTestFramework();

            //act
            Action act = () => sut.TellMessageAndWaitForExceptions(
                Message,
                null,
                ExpectedExceptionCount);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void UnitTestFramework_TellMessageAndWaitForExceptionsWithNullMessageAndSender_ThrowsArgumentNullException()
        {
            //arrange
            UnitTestFramework<DummyActor> sut = CreateUnitTestFramework();

            //act
            Action act = () => sut.TellMessageAndWaitForExceptions<object>(
                null,
                null,
                ExpectedExceptionCount);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }
        #endregion

        [Fact]
        public void UnitTestFramework_TellMessageAndWaitForExceptions_InvokesTellWaiter()
        {
            //arrange
            UnitTestFramework<DummyActor> sut = CreateUnitTestFramework();

            //act
            sut.TellMessageAndWaitForExceptions(Message, Sender, ExpectedExceptionCount);

            //assert
            TellWaiterMock.Verify(
                teller => teller.TellMessage(
                    ExceptionWaiter,
                    this,
                    SutActor,
                    Message,
                    ExpectedExceptionCount,
                    Sender),
                Times.Once);
        }
    }
}