using System;
using Akka.TestKit;

namespace ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract
{
    internal interface IChildWaiter
    {
        void Wait(TestKitBase teskKit, Action act, int expectedChildrenCount);
        void ResolvedChild();
    }
}