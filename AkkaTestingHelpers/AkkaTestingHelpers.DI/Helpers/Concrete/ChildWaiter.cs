using System;
using System.Diagnostics;
using System.Threading;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract;

namespace ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete
{
    internal sealed class ChildWaiter : IChildWaiter
    {
        private readonly AutoResetEvent _waitingToStart = new AutoResetEvent(true);
        private TestLatch _waitForChildren;
        
        public void Start(TestKitBase testKit, int expectedChildrenCount)
        {
            Console.WriteLine("Waiting for AutoResetEvent");
            if (_waitingToStart.WaitOne())
            {
                var x = expectedChildrenCount < 0 ? 0 : expectedChildrenCount;
                Console.WriteLine($"Creating test latch {x}");
                _waitForChildren = testKit.CreateTestLatch(
                    expectedChildrenCount < 0 
                        ? 0 
                        : expectedChildrenCount);
            }
            Console.WriteLine("Leaving start");
        }

        public void Wait()
        {
            Console.WriteLine("Calling ready on test latch");
            _waitForChildren?.Ready();
            Console.WriteLine("Called ready on test latch");
            _waitForChildren = null;
            _waitingToStart.Set();
            Console.WriteLine("Leaving wait");
        }

        public void ResolvedChild()
        {
            Console.WriteLine("Calling count down test latch");
            _waitForChildren?.CountDown();
            Console.WriteLine("Leaving resolve child");
        }
    }
}