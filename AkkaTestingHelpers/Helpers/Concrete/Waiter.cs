using System.Threading;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.Helpers.Abstract;

namespace ConnelHooley.AkkaTestingHelpers.Helpers.Concrete
{
    internal sealed class Waiter : IWaiter
    {
        private readonly SemaphoreSlim _waitingToStart;
        private TestLatch _waitForEvents;

        public Waiter() => _waitingToStart = new SemaphoreSlim(1);

        public void Start(TestKitBase testKit, int expectedEventCount)
        {
            _waitingToStart.Wait();
            _waitForEvents = testKit.CreateTestLatch(
                expectedEventCount < 0
                    ? 0
                    : expectedEventCount);
        }

        public void Wait()
        {
            _waitForEvents?.Ready();
            _waitForEvents = null;
            _waitingToStart.Release();
        }

        public void ResolveEvent() =>
            _waitForEvents?.CountDown();
    }
}