using System;
using Akka.TestKit;
using Akka.TestKit.Fakes;
using Akka.TestKit.Xunit2;
using Akka.TestKit.Xunit2.Fakes;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;
using Microsoft.QualityTools.Testing.Fakes;

// ReSharper disable VirtualMemberCallInConstructor

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.TestProbeCreatorTests
{
    public class TestBase : TestKit
    {
        private readonly IDisposable _shimContext;
        internal readonly ShimTestKitBase TestKitBaseShim;
        internal readonly TestKitBase TestKitBase;
        internal TestProbe TestProbeReturnedFromShim;
        internal int CallCount;
        internal string NamePassedIntoShim;

        public TestBase() : base(AkkaConfig.Config)
        {
            // Create shims
            _shimContext = ShimsContext.Create();
            TestKitBaseShim = new ShimTestKitBase(new ShimTestKit());
            
            // Create objects passed into sut methods
            TestKitBase = TestKitBaseShim.Instance;
            
            // Set up shims
            TestKitBaseShim.CreateTestProbeString =
                name =>
                {
                    CallCount++;
                    NamePassedIntoShim = name;
                    TestProbeReturnedFromShim = CreateTestProbe();
                    return TestProbeReturnedFromShim;
                };
        }

        protected override void Dispose(bool disposing)
        {
            _shimContext.Dispose();
            base.Dispose(disposing);
        }

        internal TestProbeCreator CreateTestProbeCreator() => new TestProbeCreator();
    }
}