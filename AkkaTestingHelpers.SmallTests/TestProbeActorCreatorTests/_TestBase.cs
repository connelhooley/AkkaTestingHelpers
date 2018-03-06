using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Akka.TestKit;
using Akka.TestKit.Xunit2;
using ConnelHooley.AkkaTestingHelpers.Helpers.Abstract;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete.Fakes;
using ConnelHooley.TestHelpers;
using Microsoft.QualityTools.Testing.Fakes;
using Moq;

// ReSharper disable RedundantAssignment

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.TestProbeActorCreatorTests
{
    public class TestBase : TestKit
    {
        private readonly IDisposable _shimContext;
        internal readonly Mock<ITestProbeCreator> TestProbeCreatorMock;

        internal readonly ITestProbeCreator TestProbeCreatorPassedIntoSut;
        internal readonly TestKitBase TestKitPassedIntoSut;
        internal readonly IReadOnlyDictionary<Type, Func<object, object>> HandlersPassedIntoSut;

        internal ITestProbeCreator TestProbeCreatorPassedIntoShim;
        internal TestKitBase TestKitPassedIntoShim;
        internal IReadOnlyDictionary<Type, Func<object, object>> HandlersPassedIntoShim;
        
        internal TestProbeActor TestProbeActorReturnedByShim;
        internal int ShimConstructorCallCount;
        
        public TestBase() : base(AkkaConfig.Config)
        {
            Func<Type> generateType = TestHelper.GetRandomTypeGenerator();

            // Create mocks
            TestProbeCreatorMock = new Mock<ITestProbeCreator>();

            // Create shims
            _shimContext = ShimsContext.Create();
            
            // Create objects passed into sut methods
            TestProbeCreatorPassedIntoSut = TestProbeCreatorMock.Object;
            TestKitPassedIntoSut = this;
            HandlersPassedIntoSut = ImmutableDictionary<Type, Func<object, object>>
                .Empty
                .Add(generateType(), mess => TestHelper.Generate<object>());
            
            //Set up shims
            ShimTestProbeActor.ConstructorITestProbeCreatorTestKitBaseIReadOnlyDictionaryOfTypeFuncOfObjectObject =
                (@this, testProbeCreator, testKit, handlers) =>
                {
                    ShimConstructorCallCount++;
                    TestProbeCreatorPassedIntoShim = testProbeCreator;
                    TestKitPassedIntoShim = testKit;
                    HandlersPassedIntoShim = handlers;
                    TestProbeActorReturnedByShim  = @this;
                };
        }

        protected override void Dispose(bool disposing)
        {
            _shimContext.Dispose();
            base.Dispose(disposing);
        }

        internal TestProbeActorCreator CreateTestProbeCreator() => new TestProbeActorCreator();
    }
}