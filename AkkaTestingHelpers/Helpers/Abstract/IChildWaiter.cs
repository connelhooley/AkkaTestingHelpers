using Akka.TestKit;

namespace ConnelHooley.AkkaTestingHelpers.Helpers.Abstract
{
    internal interface IChildWaiter
    {
        void Start(TestKitBase testKit, int expectedChildrenCount);
        void Wait();
        void ResolvedChild();
    }
}