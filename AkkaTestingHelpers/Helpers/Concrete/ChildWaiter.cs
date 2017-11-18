using System.Threading;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.Helpers.Abstract;

namespace ConnelHooley.AkkaTestingHelpers.Helpers.Concrete
{
    internal sealed class ChildWaiter : IChildWaiter
    {
        private readonly SemaphoreSlim _waitingToStart;
        private TestLatch _waitForChildren;

        public ChildWaiter() => 
            _waitingToStart = new SemaphoreSlim(1);

        public void Start(TestKitBase testKit, int expectedChildrenCount)
        {
            _waitingToStart.Wait();
            _waitForChildren = testKit.CreateTestLatch(
                expectedChildrenCount < 0
                    ? 0
                    : expectedChildrenCount);
        }

        public void Wait()
        {
            _waitForChildren?.Ready();
            _waitForChildren = null;
            _waitingToStart.Release();
        }

        public void ResolvedChild() => 
            _waitForChildren?.CountDown();
    }
}