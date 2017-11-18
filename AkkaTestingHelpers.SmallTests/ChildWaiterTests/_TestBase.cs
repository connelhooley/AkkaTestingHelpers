using Akka.TestKit.Xunit2;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.ChildWaiterTests
{
    public class TestBase : TestKit
    {
        public TestBase() : base(AkkaConfig.Config) { }

        internal ChildWaiter CreateChildWaiter() => new ChildWaiter();
    }
}