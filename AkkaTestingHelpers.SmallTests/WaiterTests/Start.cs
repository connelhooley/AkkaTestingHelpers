using System;
using System.Threading.Tasks;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;
using ConnelHooley.TestHelpers;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.WaiterTests
{
    public class Start : TestBase
    {
        #region Null tests
        [Fact]
        public void Waiter_StartWithNullTestKitBase_ThrowsArgumentNullException()
        {
            this.WithinTimeout(() =>
            {
                //arrange
                Waiter sut = CreateWaiter();

                //act
                Action act = () => sut.Start(null, TestHelper.GenerateNumber());

                //assert
                act.Should().Throw<ArgumentNullException>();
            });
        }
        #endregion

        [Fact]
        public void Waiter_Start_DoesNotThrowAnyExceptions()
        {
            this.WithinTimeout(() =>
            {
                //arrange
                Waiter sut = CreateWaiter();

                //act
                Action act = () => sut.Start(this, TestHelper.GenerateNumber());

                //assert
                act.Should().NotThrow();
            });
        }
        
        [Fact]
        public void Waiter_Started_Start_ShouldBlockThread()
        {
            this.WithinTimeout(() =>
            {
                //arrange
                Waiter sut = CreateWaiter();
                bool isSecondStartRan = false;
                sut.Start(this, TestHelper.GenerateNumberBetween(0, 5));

                Task.Run(() =>
                {
                    //act
                    sut.Start(this, TestHelper.GenerateNumberBetween(0, 5));
                    isSecondStartRan = true;
                });

                //assert
                this.Sleep(this.GetTimeoutHalved());
                isSecondStartRan.Should().BeFalse();
            });
        }

        [Fact]
        public void Waiter_Started_Start_ShouldUnblockThreadWhenFirstStartsEventsAreResolved()
        {
            this.WithinTimeout(() =>
            {
                //arrange
                Waiter sut = CreateWaiter();
                int expectedEventCount = TestHelper.GenerateNumberBetween(0, 5);
                bool isSecondStartRan = false;
                sut.Start(this, expectedEventCount);

                Task.Run(() =>
                {
                    //act
                    sut.Start(this, TestHelper.GenerateNumberBetween(0, 5));
                    isSecondStartRan = true;
                });
                this.Sleep(50); //ensures second start is called before continuing

                //assert
                Task.Run(() =>
                {
                    Parallel.For(0, expectedEventCount, i =>
                    {
                        sut.ResolveEvent();
                    });
                });
                sut.Wait();
                AwaitAssert(() => isSecondStartRan.Should().BeTrue());
            });
        }
    }
}