using System;
using System.Threading.Tasks;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;
using FluentAssertions;
using NUnit.Framework;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.ChildWaiterTests
{
    internal class Start : TestBase
    {
        [Test, Timeout(2000)]
        public void ChildWaiter_StartWithNullTestKitBase_ThrowsArgumentNullException()
        {
            //arrange
            ChildWaiter sut = CreateChildWaiter();

            //act
            Action act = () => sut.Start(null, TestUtils.Create<int>());

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Test, Timeout(2000)]
        public void ChildWaiter_Start_DoesNotThrowAnyExceptions()
        {
            //arrange
            ChildWaiter sut = CreateChildWaiter();

            //act
            Action act = () => sut.Start(this, TestUtils.Create<int>());

            //assert
            act.ShouldNotThrow();
        }
        
        [Test, Timeout(2000)]
        public void ChildWaiter_Started_Start_ShouldBlockThread()
        {
            //arrange
            ChildWaiter sut = CreateChildWaiter();
            int expectedChildrenCount = TestUtils.RandomBetween(0, 5);
            bool isSecondStartRan = false;
            sut.Start(this, expectedChildrenCount);
            
            Task.Run(() =>
            {
                //act
                sut.Start(this, TestUtils.RandomBetween(0, 5));
                isSecondStartRan = true;
            });

            //assert
            this.Sleep(TestKitSettings.DefaultTimeout);
            isSecondStartRan.Should().BeFalse();
        }

        [Test, Timeout(2000)]
        public void ChildWaiter_Started_Start_ShouldUnblockThreadWhenFirstStartsChildrenAreResolved()
        {
            //arrange
            ChildWaiter sut = CreateChildWaiter();
            int expectedChildrenCount = TestUtils.RandomBetween(0, 5);
            bool isSecondStartRan = false;
            sut.Start(this, expectedChildrenCount);

            Task.Run(() =>
            {
                //act
                sut.Start(this, TestUtils.RandomBetween(0, 5));
                isSecondStartRan = true;
            });
            this.Sleep(50); //ensures start is called before continuing

            //assert
            Task.Run(() =>
            {
                Parallel.For(0, expectedChildrenCount, i =>
                {
                    sut.ResolvedChild();
                });
            });
            sut.Wait();
            AwaitAssert(() => isSecondStartRan.Should().BeTrue());
        }
    }
}