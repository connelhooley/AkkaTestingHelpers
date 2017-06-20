using System;
using System.Threading;
using Akka.TestKit.Xunit2;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests
{
    public static class TestKitExtensions
    {
        public static void Sleep(this TestKit @this, int milliseconds) => 
            Thread.Sleep(@this.Dilated(TimeSpan.FromMilliseconds(milliseconds)));

        public static void Sleep(this TestKit @this, TimeSpan span) =>
            Thread.Sleep(@this.Dilated(span));
    }
}