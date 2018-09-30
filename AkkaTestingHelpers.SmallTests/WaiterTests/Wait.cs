using System;
using System.Threading.Tasks;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;
using ConnelHooley.TestHelpers;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.WaiterTests
{
    public class Wait : TestBase
    {
        [Fact]
        public void Waiter_NotStarted_Wait_DoesNotThrowAnyExceptions()
        {
            this.WithinTimeout(() =>
            {
                //arrange
                Waiter sut = CreateWaiter();

                //act
                Action act = () => sut.Wait();

                //assert
                act.Should().NotThrow();
            });
        }

        [Fact]
        public void Waiter_Started_Wait_ThrowsTimeoutExceptionWhenNotAllEventsAreResolved()
        {
            this.WithinTimeout(() =>
            {
                //arrange
                Waiter sut = CreateWaiter();
                int expectedEventCount = TestHelper.GenerateNumberBetween(2, 5);
                sut.Start(this, expectedEventCount);
                Task.Run(() =>
                {
                    this.Sleep(50);
                    Parallel.For(0, expectedEventCount - 1, i => sut.ResolveEvent());
                });

                //act
                Action act = () => sut.Wait();

                //assert
                act.Should().Throw<TimeoutException>();
            });
        }

        [Fact]
        public void Waiter_Started_Wait_BlockThreadUntilEventsAreResolved()
        {
            this.WithinTimeout(() =>
            {
                //arrange
                Waiter sut = CreateWaiter();
                int expectedEventCount = TestHelper.GenerateNumberBetween(1, 5);
                int resolvedEventCount = 0;
                sut.Start(this, expectedEventCount);
                Task.Run(() =>
                {
                    this.Sleep(50);
                    Parallel.For(0, expectedEventCount, i =>
                    {
                        resolvedEventCount++;
                        sut.ResolveEvent();
                    });
                });

                //act
                sut.Wait();

                //assert
                resolvedEventCount.Should().Be(expectedEventCount);
            });
        }

        [Fact]
        public void Waiter_StartedWithNoExpectedEvents_Wait_DoesNotBlockThread()
        {
            //arrange
            Waiter sut = CreateWaiter();
            sut.Start(this, 0);

            //act
            Action act = () => sut.Wait();

            //assert
            this.WithinTimeout(act);
        }

        [Fact]
        public void Waiter_StartedWithNegativeExpectedEvents_Wait_DoesNotBlockThread()
        {
            //arrange
            Waiter sut = CreateWaiter();
            sut.Start(this, TestHelper.GenerateNumberBetween(int.MinValue, -1));

            //act
            Action act = () => sut.Wait();

            //assert
            this.WithinTimeout(act);
        }
        
        [Fact]
        public void Waiter_Waited_Wait_BlockThreadUntilEventsAreResolved()
        {
            this.WithinTimeout(() =>
            {
                //arrange
                Waiter sut = CreateWaiter();
                sut.Start(this, 0);
                sut.Wait();
                int expectedEventCount = TestHelper.GenerateNumberBetween(1, 5);
                int resolvedEventCount = 0;
                sut.Start(this, expectedEventCount);
                Task.Run(() =>
                {
                    this.Sleep(50);
                    Parallel.For(0, expectedEventCount, i =>
                    {
                        resolvedEventCount++;
                        sut.ResolveEvent();
                    });
                });

                //act
                sut.Wait();

                //assert
                resolvedEventCount.Should().Be(expectedEventCount);
            });
        }
    }
}