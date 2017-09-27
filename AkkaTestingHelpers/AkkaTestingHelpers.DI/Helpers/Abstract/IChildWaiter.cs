using Akka.TestKit;

namespace ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract
{
    internal interface IChildWaiter
    {
        void Start(TestKitBase testKit, int expectedChildrenCount);
        void Wait();
        void ResolvedChild();
    }
}