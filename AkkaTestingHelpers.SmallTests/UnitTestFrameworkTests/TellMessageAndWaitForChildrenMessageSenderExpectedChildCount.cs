using System;
using FluentAssertions;
using Moq;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.UnitTestFrameworkTests
{
    public class TellMessageAndWaitForChildrenMessageSenderExpectedChildCount : TestBase
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
        public void UnitTestFramework_TellMessageAndWaitForChildren_TellsChild()
        {
            //arrange
            UnitTestFramework<DummyActor> sut = CreateUnitTestFramework();

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