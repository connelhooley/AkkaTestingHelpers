using System;
using System.Threading;
using System.Threading.Tasks;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;
using FluentAssertions;
using NUnit.Framework;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.ChildWaiterTests
{
    internal class Wait : TestBase
    {
        [Test]
        public void ChildWaiter_Wait_NotStarted_DoesNotThrowAnyExceptions()
        {
            //arrange
            ChildWaiter sut = CreateChildWaiter();

            //act
            Action act = () => sut.Wait();

            //assert
            act.ShouldNotThrow();
        }

        [Test]
        public void ChildWaiter_Wait_Started_ThrowsTimeoutExceptionWhenChildrenAreNotResolved()
        {
            //arrange
            ChildWaiter sut = CreateChildWaiter();
            int expectedChildrenCount = TestUtils.RandomBetween(2, 5);
            sut.Start(this, expectedChildrenCount);
            Task.Run(() =>
            {
                Thread.Sleep(100);
                Parallel.For(0, expectedChildrenCount-1, i => sut.ResolvedChild());
            });

            //act
            Action act = () => sut.Wait();

            //assert
            act.ShouldThrow<TimeoutException>();
        }

        [Test]
        public void ChildWaiter_Wait_Started_BlockThreadUntilChildrenAreResolved()
        {
            //arrange
            ChildWaiter sut = CreateChildWaiter();
            int expectedChildrenCount = TestUtils.RandomBetween(1, 5);
            int resolvedChildrenCount = 0;
            sut.Start(this, expectedChildrenCount);
            Task.Run(() =>
            {
                Thread.Sleep(100);
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
        }

        [Test]
        [Timeout(500)]
        public void ChildWaiter_Wait_StartedWithNoExpectedChildren_DoesNotBlockThread()
        {
            //arrange
            ChildWaiter sut = CreateChildWaiter();
            sut.Start(this, 0);

            //act
            sut.Wait();

            //assert
            //timeout attribute
        }

        [Test]
        [Timeout(500)]
        public void ChildWaiter_Wait_StartedWithNegativeExpectedChildren_DoesNotBlockThread()
        {
            //arrange
            ChildWaiter sut = CreateChildWaiter();
            sut.Start(this, TestUtils.RandomBetween(int.MinValue, -1));

            //act
            sut.Wait();

            //assert
            //timeout attribute
        }

        [Test]
        [Timeout(500)]
        public void ChildWaiter_Wait_Waited_BlockThreads()
        {
            //arrange
            ChildWaiter sut = CreateChildWaiter();
            sut.Start(this, 0);
            sut.Wait();
            int expectedChildrenCount = TestUtils.RandomBetween(1, 5);
            sut.Start(this, expectedChildrenCount);
            Task.Run(() =>
            {
                Thread.Sleep(100);
                Parallel.For(0, expectedChildrenCount, i =>
                {
                    sut.ResolvedChild();
                });
            });

            //act
            sut.Wait();
            
            //assert
            //timeout attribute
        }
    }
}