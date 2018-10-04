using Moq;
using Xunit;
using System.Threading.Tasks;
using System.Collections.Generic;
using FluentAssertions;
using System.Threading;
using System;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.UnitTestFrameworkTests
{
    public class DelayAsync : TestBase
    {
        [Fact]
        public async Task UnitTestFramework_DelayAsync_InvokesDelayerAsync()
        {
            //arrange
            UnitTestFramework<DummyActor> sut = CreateUnitTestFramework();

            //act
            await sut.DelayAsync(DelayDuration);

            //assert
            DelayerMock.Verify(
                delayer => delayer.DelayAsync(this, DelayDuration),
                Times.Once);
        }

        [Fact]
        public async Task UnitTestFramework_DelayAsync_AwaitsDelayerAsync()
        {
            //arrange
            List<string> callOrder = new List<string>();

            DelayerMock
                .Setup(delayer => delayer.DelayAsync(this, DelayDuration))
                .Returns(() => Task.Run(() => {
                    callOrder.Add("ReturnedTask1");
                    Thread.Sleep(TimeSpan.FromMilliseconds(200));
                    callOrder.Add("ReturnedTask2");
                }));

            UnitTestFramework<DummyActor> sut = CreateUnitTestFramework();

            //act
            Task result = sut.DelayAsync(DelayDuration);

            //assert
            await result;
            callOrder.Add("AfterTask");
            callOrder.Should().Equal("ReturnedTask1", "ReturnedTask2", "AfterTask");
        }
    }
}
