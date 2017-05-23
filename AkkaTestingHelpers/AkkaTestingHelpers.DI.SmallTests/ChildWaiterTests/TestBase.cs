using Akka.TestKit.NUnit3;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.ChildWaiterTests
{
    internal class TestBase : TestKit
    {
        public TestBase(): base(@"akka.test.timefactor = 0.6") { }

        protected ChildWaiter CreateChildWaiter() => new ChildWaiter();
    }
}