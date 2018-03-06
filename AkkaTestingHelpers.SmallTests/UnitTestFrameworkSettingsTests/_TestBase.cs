using System;
using System.Collections.Immutable;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.Xunit2;
using ConnelHooley.AkkaTestingHelpers.Fakes;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete.Fakes;
using ConnelHooley.TestHelpers;
using Microsoft.QualityTools.Testing.Fakes;

// ReSharper disable RedundantAssignment

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.UnitTestFrameworkSettingsTests
{
    public class TestBase : TestKit
    {
        private readonly IDisposable _shimContext;
        
        internal int UnitTestFrameworkCreatorConstructorCount;
        internal int UnitTestFrameworkCreatorCreateCount;
        protected UnitTestFramework<DummyParentActor> UnitTestFrameworkReturnedFromShim;

        internal TestKitBase TestKitPassedIntoShim;
        internal ImmutableDictionary<(Type, Type), Func<object, object>> HandlersPassedIntoShim;
        internal Props PropsPassedIntoShim;
        internal int ExpectedChildrenCountPassedIntoShim;

        internal TestKitBase TestKitPassedIntoSut;
        internal Props PropsPassedIntoSut;
        internal int ExpectedChildrenCountPassedIntoSut;

        public TestBase() : base(AkkaConfig.Config)
        {
            // Create objects passed into sut
            TestKitPassedIntoSut = this;
            PropsPassedIntoSut = Props.Create<DummyChildActor1>();
            ExpectedChildrenCountPassedIntoSut = TestHelper.GenerateNumber();

            // Create shims
            _shimContext = ShimsContext.Create();

            //Set up shims
            UnitTestFrameworkReturnedFromShim = new ShimUnitTestFramework<DummyParentActor>();

            ShimUnitTestFrameworkCreator.Constructor = @this => ++UnitTestFrameworkCreatorConstructorCount;

            ShimUnitTestFrameworkCreator.AllInstances
                .CreateOf1ImmutableDictionaryOfValueTupleOfTypeTypeFuncOfObjectObjectTestKitBasePropsInt32<DummyParentActor>(
                    (@this, handlers, testKit, props, numberOfChildrenIntoShim) =>
                    {
                        UnitTestFrameworkCreatorCreateCount++;
                        HandlersPassedIntoShim = handlers;
                        TestKitPassedIntoShim = testKit;
                        PropsPassedIntoShim = props;
                        ExpectedChildrenCountPassedIntoShim = numberOfChildrenIntoShim;
                        return UnitTestFrameworkReturnedFromShim;
                    });
        }

        protected override void Dispose(bool disposing)
        {
            _shimContext.Dispose();
            base.Dispose(disposing);
        }

        protected class DummyParentActor : ReceiveActor { }

        protected class DummyChildActor1 : ReceiveActor { }

        protected class DummyChildActor2 : ReceiveActor { }

        protected class Message1 { }

        protected class Reply1 { }

        protected class Message2 { }

        protected class Reply2 { }
    }
}