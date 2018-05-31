using System;
using System.Threading.Tasks;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;
using ConnelHooley.TestHelpers;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.ChildWaiterTests
{
    public class Wait : TestBase
    {
        [Fact]
        public void ChildWaiter_NotStarted_Wait_DoesNotThrowAnyExceptions()
        {
            this.WithinTimeout(() =>
            {
                //arrange
                ChildWaiter sut = CreateChildWaiter();

                //act
                Action act = () => sut.Wait();

                //assert
                act.Should().NotThrow();
            });
        }

        [Fact]
        public void ChildWaiter_Started_Wait_ThrowsTimeoutExceptionWhenNotAllChildrenAreResolved()
        {
            this.WithinTimeout(() =>
            {
                //arrange
                ChildWaiter sut = CreateChildWaiter();
                int expectedChildrenCount = TestHelper.GenerateNumberBetween(2, 5);
                sut.Start(this, expectedChildrenCount);
                Task.Run(() =>
                {
                    this.Sleep(50);
                    Parallel.For(0, expectedChildrenCount - 1, i => sut.ResolvedChild());
                });

                //act
                Action act = () => sut.Wait();

                //assert
                act.Should().Throw<TimeoutException>();
            });
        }

        [Fact]
        public void ChildWaiter_Started_Wait_BlockThreadUntilChildrenAreResolved()
        {
            this.WithinTimeout(() =>
            {
                //arrange
                ChildWaiter sut = CreateChildWaiter();
                int expectedChildrenCount = TestHelper.GenerateNumberBetween(1, 5);
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
            this.WithinTimeout(act);
        }

        [Fact]
        public void ChildWaiter_StartedWithNegativeExpectedChildren_Wait_DoesNotBlockThread()
        {
            //arrange
            ChildWaiter sut = CreateChildWaiter();
            sut.Start(this, TestHelper.GenerateNumberBetween(int.MinValue, -1));

            //act
            Action act = () => sut.Wait();

            //assert
            this.WithinTimeout(act);
        }
        
        [Fact]
        public void ChildWaiter_Waited_Wait_BlockThreadUntilChildrenAreResolved()
        {
            this.WithinTimeout(() =>
            {
                //arrange
                ChildWaiter sut = CreateChildWaiter();
                sut.Start(this, 0);
                sut.Wait();
                int expectedChildrenCount = TestHelper.GenerateNumberBetween(1, 5);
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