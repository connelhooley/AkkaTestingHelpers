using Akka.TestKit;
using System;
using System.Threading.Tasks;

namespace ConnelHooley.AkkaTestingHelpers.Helpers.Abstract
{
    internal interface IDelayer
    {
        void Delay(TestKitBase testKit, TimeSpan duration);

        Task DelayAsync(TestKitBase testKit, TimeSpan duration);
    }
}
