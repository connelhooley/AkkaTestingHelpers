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
            Trace.WriteLine("Waiting for AutoResetEvent");
            if (_waitingToStart.WaitOne())
            {
                var x = expectedChildrenCount < 0 ? 0 : expectedChildrenCount;
                Trace.WriteLine($"Creating test latch {x}");
                _waitForChildren = testKit.CreateTestLatch(
                    expectedChildrenCount < 0 
                        ? 0 
                        : expectedChildrenCount);
            }
            Trace.WriteLine("Leaving start");
        }

        public void Wait()
        {
            Trace.WriteLine("Calling ready on test latch");
            _waitForChildren?.Ready();
            Trace.WriteLine("Called ready on test latch");
            _waitForChildren = null;
            _waitingToStart.Set();
            Trace.WriteLine("Leaving wait");
        }

        public void ResolvedChild()
        {
            Trace.WriteLine("Calling count down test latch");
            _waitForChildren?.CountDown();
            Trace.WriteLine("Leaving resolve child");
        }
    }
}