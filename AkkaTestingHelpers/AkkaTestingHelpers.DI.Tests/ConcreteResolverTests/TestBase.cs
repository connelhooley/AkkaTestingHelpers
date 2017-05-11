using Akka.TestKit.NUnit3;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace ConnelHooley.AkkaTestingHelpers.DI.Tests.ConcreteResolverTests
{
    public class TestBase: TestKit
    {
        public TestBase() : base(@"akka.test.timefactor = 0.6") { }

        protected Fixture Fixture;

        [SetUp]
        public void Setup()
        {
            Fixture = new Fixture();
        }

        [TearDown]
        public void TearDown()
        {
            Fixture = null;
        }
    }
}