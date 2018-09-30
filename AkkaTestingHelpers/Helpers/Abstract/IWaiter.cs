using Akka.TestKit;

namespace ConnelHooley.AkkaTestingHelpers.Helpers.Abstract
{
    internal interface IWaiter
    {
        void Start(TestKitBase testKit, int expectedEventCount);
        void Wait();
        void ResolveEvent();
    }
}