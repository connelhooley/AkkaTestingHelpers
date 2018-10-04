using Akka.Configuration;
using Akka.TestKit.Xunit2;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;
using ConnelHooley.TestHelpers;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.DelayerTests
{
    public class TestBase
    {
        internal int TimeFactor;
        internal TestKit TestKit;

        public TestBase()
        {
            TimeFactor = TestHelper.GenerateNumberBetween(2, 5);
            Config config = ConfigurationFactory.ParseString($"akka.test.timefactor = {TimeFactor}");
            TestKit = new TestKit(config.WithFallback(AkkaConfig.Config));
        }

        internal Delayer CreateDelayer() => new Delayer();
    }
}
