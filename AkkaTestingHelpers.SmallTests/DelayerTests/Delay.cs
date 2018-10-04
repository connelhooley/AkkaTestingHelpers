using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;
using ConnelHooley.TestHelpers;
using FluentAssertions;
using System;
using System.Diagnostics;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.DelayerTests
{
    public class Delay : TestBase
    {
        #region Null tests
        [Fact]
        public void Delayer_DelayWithNullTestKitBase_ThrowsArgumentNullException()
        {
            //arrange
            Delayer sut = CreateDelayer();

            //act
            Action act = () => sut.Delay(null, TimeSpan.Zero);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }
        #endregion

        [Fact]
        public void Delayer_DelayWithPositiveDuration_DelaysForDilatedDuration()
        {
            //arrange
            TimeSpan duration = TimeSpan.FromSeconds(1);
            Delayer sut = CreateDelayer();

            //act
            Action act = () => sut.Delay(TestKit, duration);

            //assert
            TimeSpan expected = TimeSpan.FromSeconds(duration.TotalSeconds * TimeFactor);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            act();
            stopwatch.Stop();
            stopwatch.Elapsed.Should().BeCloseTo(expected, 250);
        }

        [Fact]
        public void Delayer_DelayWithZeroDuration_DoesNotDelay()
        {
            //arrange
            TimeSpan duration = TimeSpan.Zero;
            Delayer sut = CreateDelayer();

            //act
            Action act = () => sut.Delay(TestKit, duration);

            //assert
            TimeSpan expected = TimeSpan.Zero;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            act();
            stopwatch.Stop();
            stopwatch.Elapsed.Should().BeCloseTo(expected, 250);
        }

        [Fact]
        public void Delayer_DelayWithNegativeDuration_DoesNotDelay()
        {
            //arrange
            TimeSpan duration = TimeSpan.FromSeconds(TestHelper.GenerateNumberBetween(-1000, -1));
            Delayer sut = CreateDelayer();

            //act
            Action act = () => sut.Delay(TestKit, duration);

            //assert
            TimeSpan expected = TimeSpan.Zero;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            act();
            stopwatch.Stop();
            stopwatch.Elapsed.Should().BeCloseTo(expected, 250);
        }
    }
}
