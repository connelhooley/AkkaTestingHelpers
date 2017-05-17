using System;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract;

namespace ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete
{
    internal class ChildWaiter : IChildWaiter
    {
        private readonly object _waitLock = new object();
        private TestLatch _waitForChildren;

        public void Wait(TestKitBase testKit, Action act, int expectedChildrenCount)
        {
            if (expectedChildrenCount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(expectedChildrenCount), "Cannot be less than 1");
            }
            lock (_waitLock)
            {
                _waitForChildren = testKit.CreateTestLatch(expectedChildrenCount);
                act();
                _waitForChildren.Ready();
                _waitForChildren = null;
            }
        }

        public void ResolvedChild()
        {
            _waitForChildren?.CountDown();
        }
    }
}