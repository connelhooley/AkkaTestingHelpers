using System;
using System.Collections.Immutable;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.Xunit2;
using ConnelHooley.AkkaTestingHelpers.DI.Fakes;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
// ReSharper disable RedundantAssignment

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.UnitTestFrameworkSettingsTests
{
    public class TestBase : TestKit
    {
        private readonly IDisposable _shimContext;
        
        internal int UnitTestFrameworkCreatorConstructorCount;
        internal int UnitTestFrameworkCreatorCreateCount;
        protected UnitTestFramework<DummyActor1> UnitTestFrameworkReturnedFromShim;

        internal TestKitBase TestKitPassedIntoShim;
        internal ImmutableDictionary<(Type, Type), Func<object, object>> HandlersPassedIntoShim;
        internal Props PropsPassedIntoShim;
        internal int ExpectedChildrenCountPassedIntoShim;

        internal TestKitBase TestKitPassedIntoSut;
        internal ImmutableDictionary<(Type, Type), Func<object, object>> HandlersPassedIntoSut;
        internal Props PropsPassedIntoSut;
        internal int ExpectedChildrenCountPassedIntoSut;

        public TestBase() : base(AkkaConfig.Config)
        {
            // Create objects passed into sut
            TestKitPassedIntoSut = this;
            HandlersPassedIntoSut = ImmutableDictionary<(Type, Type), Func<object, object>>.Empty;
            PropsPassedIntoSut = Props.Create<DummyActor1>();
            ExpectedChildrenCountPassedIntoSut = TestUtils.Create<int>();

            // Create shims
            _shimContext = ShimsContext.Create();

            //Set up shims
            UnitTestFrameworkReturnedFromShim = new ShimUnitTestFramework<DummyActor1>();

            ShimUnitTestFrameworkCreator.Constructor = @this => ++UnitTestFrameworkCreatorConstructorCount;

            ShimUnitTestFrameworkCreator.AllInstances
                .CreateOf1ImmutableDictionaryOfValueTupleOfTypeTypeFuncOfObjectObjectTestKitBasePropsInt32(
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

        protected class DummyActor1 : ReceiveActor { }

        protected class DummyActor2 : ReceiveActor { }

        protected class Message1 { }

        protected class Reply1 { }

        protected class Message2 { }

        protected class Reply2 { }
    }
}