using Akka.TestKit.Xunit2;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.WaiterTests
{
    public class TestBase : TestKit
    {
        public TestBase() : base(AkkaConfig.Config) { }

        internal Waiter CreateWaiter() => new Waiter();
    }
}