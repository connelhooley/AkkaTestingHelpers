using System;
using System.Threading.Tasks;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.ChildWaiterTests
{
    internal class Wait : TestBase
    {
        [Fact]
        public void ChildWaiter_NotStarted_Wait_DoesNotThrowAnyExceptions()
        {
            Within(TimeSpan.FromSeconds(2), () =>
            {
                //arrange
                ChildWaiter sut = CreateChildWaiter();

                //act
                Action act = () => sut.Wait();

                //assert
                act.ShouldNotThrow();
            });

        }

        [Fact]
        public void ChildWaiter_Started_Wait_ThrowsTimeoutExceptionWhenChildrenAreNotResolved()
        {
            Within(TimeSpan.FromSeconds(2), () =>
            {
                //arrange
                ChildWaiter sut = CreateChildWaiter();
                int expectedChildrenCount = TestUtils.RandomBetween(2, 5);
                sut.Start(this, expectedChildrenCount);
                Task.Run(() =>
                {
                    this.Sleep(new TimeSpan(TestKitSettings.DefaultTimeout.Ticks / 2));
                    Parallel.For(0, expectedChildrenCount - 1, i => sut.ResolvedChild());
                });

                //act
                Action act = () => sut.Wait();

                //assert
                act.ShouldThrow<TimeoutException>();
            });
        }

        [Fact]
        public void ChildWaiter_Started_Wait_BlockThreadUntilChildrenAreResolved()
        {
            Within(TimeSpan.FromSeconds(2), () =>
            {
                //arrange
                ChildWaiter sut = CreateChildWaiter();
                int expectedChildrenCount = TestUtils.RandomBetween(1, 5);
                int resolvedChildrenCount = 0;
                sut.Start(this, expectedChildrenCount);
                Task.Run(() =>
                {
                    this.Sleep(50);
                    Parallel.For(0, expectedChildrenCount, i =>
                    {
                        resolvedChildrenCount++;
                        sut.ResolvedChild();
                    });
                });

                //act
                sut.Wait();

                //assert
                resolvedChildrenCount.Should().Be(expectedChildrenCount);
            });
        }

        [Fact]
        public void ChildWaiter_StartedWithNoExpectedChildren_Wait_DoesNotBlockThread()
        {
            //arrange
            ChildWaiter sut = CreateChildWaiter();
            sut.Start(this, 0);

            //act
            Action act = () => sut.Wait();

            //assert
            Within(TestKitSettings.DefaultTimeout, act);
        }

        [Fact]
        public void ChildWaiter_StartedWithNegativeExpectedChildren_Wait_DoesNotBlockThread()
        {
            //arrange
            ChildWaiter sut = CreateChildWaiter();
            sut.Start(this, TestUtils.RandomBetween(int.MinValue, -1));

            //act
            Action act = () => sut.Wait();

            //assert
            Within(TestKitSettings.DefaultTimeout, act);
        }
        
        [Fact]
        public void ChildWaiter_Waited_Wait_BlockThreadUntilChildrenAreResolved()
        {
            Within(TimeSpan.FromSeconds(2), () =>
            {
                //arrange
                ChildWaiter sut = CreateChildWaiter();
                sut.Start(this, 0);
                sut.Wait();
                int expectedChildrenCount = TestUtils.RandomBetween(1, 5);
                int resolvedChildrenCount = 0;
                sut.Start(this, expectedChildrenCount);
                Task.Run(() =>
                {
                    this.Sleep(50);
                    Parallel.For(0, expectedChildrenCount, i =>
                    {
                        resolvedChildrenCount++;
                        sut.ResolvedChild();
                    });
                });

                //act
                sut.Wait();

                //assert
                resolvedChildrenCount.Should().Be(expectedChildrenCount);
            });
        }
    }
}