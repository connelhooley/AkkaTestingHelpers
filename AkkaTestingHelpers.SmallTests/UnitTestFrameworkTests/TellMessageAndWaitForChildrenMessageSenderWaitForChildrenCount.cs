using System;
using FluentAssertions;
using Moq;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.UnitTestFrameworkTests
{
    public class TellMessageAndWaitForChildrenMessageSenderWaitForChildrenCount : TestBase
    {
        #region Null tests
        [Fact]
        public void UnitTestFramework_TellMessageAndWaitForChildrenWithNullMessage_ThrowsArgumentNullException()
        {
            //arrange
            UnitTestFramework<DummyActor> sut = CreateUnitTestFramework();

            //act
            Action act = () => sut.TellMessageAndWaitForChildren<object>(
                null, 
                SutActor, 
                ExpectedChildCount);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void UnitTestFramework_TellMessageAndWaitForChildrenWithNullSender_ThrowsArgumentNullException()
        {
            //arrange
            UnitTestFramework<DummyActor> sut = CreateUnitTestFramework();

            //act
            Action act = () => sut.TellMessageAndWaitForChildren(
                Message,
                null,
                ExpectedChildCount);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void UnitTestFramework_TellMessageAndWaitForChildrenWithNullMessageAndSender_ThrowsArgumentNullException()
        {
            //arrange
            UnitTestFramework<DummyActor> sut = CreateUnitTestFramework();

            //act
            Action act = () => sut.TellMessageAndWaitForChildren<object>(
                null, 
                null, 
                ExpectedChildCount);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }
        #endregion

        [Fact]
        public void UnitTestFramework_TellMessageAndWaitForChildren_InvokesTellWaiter()
        {
            //arrange
            UnitTestFramework<DummyActor> sut = CreateUnitTestFramework();

            //act
            sut.TellMessageAndWaitForChildren(Message, Sender, ExpectedChildCount);

            //assert
            TellWaiterMock.Verify(
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