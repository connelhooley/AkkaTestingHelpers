using System;
using Akka.TestKit.Xunit2;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;
using Microsoft.QualityTools.Testing.Fakes;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.TestProbeCreatorTests
{
    public class TestBase : TestKit
    {
        private readonly IDisposable _shimContext;

        public TestBase() : base(AkkaConfig.Config)
        {
            _shimContext = ShimsContext.Create();
            
        }

        protected override void Dispose(bool disposing)
        {
            _shimContext.Dispose();
            base.Dispose(disposing);
        }

        internal TestProbeCreator CreateTestProbeCreator() => new TestProbeCreator();
    }
}