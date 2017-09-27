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
            Trace.WriteLine("Waiting for AutoResetEvent");
            Debug.WriteLine("Waiting for AutoResetEvent");
            if (_waitingToStart.WaitOne())
            {
                var x = expectedChildrenCount < 0 ? 0 : expectedChildrenCount;
                Console.WriteLine($"Creating test latch {x}");
                Trace.WriteLine($"Creating test latch {x}");
                Debug.WriteLine($"Creating test latch {x}");
                _waitForChildren = testKit.CreateTestLatch(
                    expectedChildrenCount < 0 
                        ? 0 
                        : expectedChildrenCount);
            }
        }

        public void Wait()
        {
            Console.WriteLine("Calling ready on test latch");
            Trace.WriteLine("Calling ready on test latch");
            Debug.WriteLine("Calling ready on test latch");
            _waitForChildren?.Ready();
            _waitForChildren = null;
            _waitingToStart.Set();
        }

        public void ResolvedChild()
        {
            Console.WriteLine("Calling count down test latch");
            Trace.WriteLine("Calling count down test latch");
            Debug.Write("Calling count down test latch");
            _waitForChildren?.CountDown();
        }
    }
}