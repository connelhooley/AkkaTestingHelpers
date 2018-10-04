using System;
using System.Threading.Tasks;
using ConnelHooley.AkkaTestingHelpers.Helpers.Abstract;

namespace ConnelHooley.AkkaTestingHelpers.Helpers.Concrete
{
    internal class Delayer : IDelayer
    {
        public void Delay(TimeSpan duration)
        {
            throw new NotImplementedException();
        }

        public Task DelayAsync(TimeSpan duration)
        {
            throw new NotImplementedException();
        }
    }
}
