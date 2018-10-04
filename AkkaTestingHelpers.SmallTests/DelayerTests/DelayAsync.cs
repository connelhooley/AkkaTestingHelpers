using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;
using ConnelHooley.TestHelpers;
using FluentAssertions;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.DelayerTests
{
    public class DelayAsync : TestBase
    {
        #region Null tests
        [Fact]
        public void Delayer_DelayAsyncWithNullTestKitBase_ThrowsArgumentNullException()
        {
            //arrange
            Delayer sut = CreateDelayer();

            //act
            Func<Task> act = () => sut.DelayAsync(null, TimeSpan.Zero);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }
        #endregion

        [Fact]
        public async Task Delayer_DelayAsyncWithPositiveDuration_DelaysForDilatedDurationAsync()
        {
            //arrange
            TimeSpan duration = TimeSpan.FromSeconds(1);
            Delayer sut = CreateDelayer();

            //act
            Func<Task> act = () => sut.DelayAsync(TestKit, duration);

            //assert
            TimeSpan expected = TimeSpan.FromSeconds(duration.TotalSeconds * TimeFactor);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            await act().ConfigureAwait(false);
            stopwatch.Stop();
            stopwatch.Elapsed.Should().BeCloseTo(expected, 250);
        }

        [Fact]
        public async Task Delayer_DelayAsyncWithZeroDuration_DoesNotDelayAsync()
        {
            //arrange
            TimeSpan duration = TimeSpan.Zero;
            Delayer sut = CreateDelayer();

            //act
            Func<Task> act = () => sut.DelayAsync(TestKit, duration);

            //assert
            TimeSpan expected = TimeSpan.Zero;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            await act().ConfigureAwait(false);
            stopwatch.Stop();
            stopwatch.Elapsed.Should().BeCloseTo(expected, 250);
        }

        [Fact]
        public async Task Delayer_DelayAsyncWithNegativeDuration_DoesNotDelayAsync()
        {
            //arrange
            TimeSpan duration = TimeSpan.FromSeconds(TestHelper.GenerateNumberBetween(-1000, -1));
            Delayer sut = CreateDelayer();

            //act
            Func<Task> act = () => sut.DelayAsync(TestKit, duration);

            //assert
            TimeSpan expected = TimeSpan.Zero;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            await act().ConfigureAwait(false);
            stopwatch.Stop();
            stopwatch.Elapsed.Should().BeCloseTo(expected, 250);
        }
    }
}
