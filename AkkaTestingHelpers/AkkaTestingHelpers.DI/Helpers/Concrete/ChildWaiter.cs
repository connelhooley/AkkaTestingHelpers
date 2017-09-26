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
            Debug.Write("Waiting for AutoResetEvent");
            if (_waitingToStart.WaitOne())
            {
                var x = expectedChildrenCount < 0 ? 0 : expectedChildrenCount;
                Debug.Write($"Creating test latch {x}");
                _waitForChildren = testKit.CreateTestLatch(
                    expectedChildrenCount < 0 
                        ? 0 
                        : expectedChildrenCount);
            }
        }

        public void Wait()
        {
            Debug.Write("Calling ready on test latch");
            _waitForChildren?.Ready();
            _waitForChildren = null;
            _waitingToStart.Set();
        }

        public void ResolvedChild()
        {
            Debug.Write("Calling count down test latch");
            _waitForChildren?.CountDown();
        }
    }
}