using System;
using System.Threading;
using System.Threading.Tasks;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.Helpers.Abstract;

namespace ConnelHooley.AkkaTestingHelpers.Helpers.Concrete
{
    internal class Delayer : IDelayer
    {
        public void Delay(TestKitBase testKit, TimeSpan duration)
        {
            if (duration < TimeSpan.Zero) return;
            Thread.Sleep(testKit.Dilated(duration));
        }

        public async Task DelayAsync(TestKitBase testKit, TimeSpan duration)
        {
            if (duration < TimeSpan.Zero) return;
            await Task.Delay(testKit.Dilated(duration)).ConfigureAwait(false);
        }
    }
}
