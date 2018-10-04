using System;
using System.Threading.Tasks;

namespace ConnelHooley.AkkaTestingHelpers.Helpers.Abstract
{
    internal interface IDelayer
    {
        void Delay(TimeSpan duration);

        Task DelayAsync(TimeSpan duration);
    }
}
