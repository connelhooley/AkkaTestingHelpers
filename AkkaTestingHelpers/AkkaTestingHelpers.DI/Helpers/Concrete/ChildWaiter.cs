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
            if (_waitingToStart.WaitOne())
            {
                _waitForChildren = testKit.CreateTestLatch(
                    expectedChildrenCount < 0 
                        ? 0 
                        : expectedChildrenCount);
            }
        }

        public void Wait()
        {
            _waitForChildren?.Ready();
            _waitForChildren = null;
            _waitingToStart.Set();
        }

        public void ResolvedChild()
        {
            _waitForChildren?.CountDown();
        }
    }
}