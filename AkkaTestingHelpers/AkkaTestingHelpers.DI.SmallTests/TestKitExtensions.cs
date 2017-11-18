using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Akka.TestKit.Xunit2;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests
{
    public static class TestKitExtensions
    {
        public static void Sleep(this TestKit @this, int milliseconds) => 
            Thread.Sleep(@this.Dilated(TimeSpan.FromMilliseconds(milliseconds)));

        public static void Sleep(this TestKit @this, TimeSpan span) =>
            Thread.Sleep(@this.Dilated(span));

        public static TimeSpan GetTimeoutHalved(this TestKit @this) =>
            new TimeSpan(@this.TestKitSettings.DefaultTimeout.Ticks / 2);

        public static TimeSpan GetTimeoutDoubled(this TestKit @this) =>
            new TimeSpan(@this.TestKitSettings.DefaultTimeout.Ticks * 2);

        public static void WithinTimeout(this TestKit @this, Action act)
        {
            // Double timeout as some tests need to wait for time out for the TestLatch to throw
            TimeSpan span = @this.Dilated(@this.GetTimeoutDoubled());
            if (!Task.Run(act).Wait(span)) throw new TimeoutException();
        }
    }
}