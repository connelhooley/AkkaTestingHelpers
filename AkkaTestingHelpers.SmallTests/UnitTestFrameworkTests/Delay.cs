using Moq;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.UnitTestFrameworkTests
{
    public class Delay : TestBase
    {
        [Fact]
        public void UnitTestFramework_Delay_InvokesDelayer()
        {
            //arrange
            UnitTestFramework<DummyActor> sut = CreateUnitTestFramework();

            //act
            sut.Delay(DelayDuration);

            //assert
            DelayerMock.Verify(
                delayer => delayer.Delay(this, DelayDuration),
                Times.Once);
        }
    }
}
