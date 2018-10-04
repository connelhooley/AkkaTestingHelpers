using Akka.TestKit.Xunit2;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.DelayerTests
{
    public class TestBase : TestKit
    {
        public TestBase() : base(AkkaConfig.Config) { }

        internal Delayer CreateDelayer() => new Delayer();
    }
}
