using System;
using FluentAssertions;
using Moq;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.UnitTestFrameworkTests
{
    public class TellMessageAndWaitForExceptionsMessageWaitForExceptionCount : TestBase
    {
        #region Null tests
        [Fact]
        public void UnitTestFramework_TellMessageAndWaitForExceptionsNoSenderWithNullMessage_ThrowsArgumentNullException()
        {
            //arrange
            UnitTestFramework<DummyActor> sut = CreateUnitTestFramework();

            //act
            Action act = () => sut.TellMessageAndWaitForExceptions<object>(
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
            sut.TellMessageAndWaitForExceptions(Message, ExpectedChildCount);

            //assert
            TellWaiterMock.Verify(
                teller => teller.TellMessage(
                    ExceptionWaiter,
                    this,
                    SutActor,
                    Message,
                    ExpectedChildCount,
                    null),
                Times.Once);
        }
    }
}