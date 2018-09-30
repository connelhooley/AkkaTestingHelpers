using System;
using FluentAssertions;
using Moq;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.UnitTestFrameworkTests
{
    public class TellMessageAndWaitForChildrenMessageExpectedChildCount : TestBase
    {
        #region Null tests
        [Fact]
        public void UnitTestFramework_TellMessageAndWaitForChildrenNoSenderWithNullMessage_ThrowsArgumentNullException()
        {
            //arrange
            UnitTestFramework<DummyActor> sut = CreateUnitTestFramework();

            //act
            Action act = () => sut.TellMessageAndWaitForChildren<object>(
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
            sut.TellMessageAndWaitForChildren(Message, ExpectedChildCount);

            //assert
            TellWaiterMock.Verify(
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