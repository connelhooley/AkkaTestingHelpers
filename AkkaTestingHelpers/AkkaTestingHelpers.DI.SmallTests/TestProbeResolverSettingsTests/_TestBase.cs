using System;
using System.Collections.Immutable;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.Xunit2;
using ConnelHooley.AkkaTestingHelpers.DI.Fakes;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete.Fakes;
using Microsoft.QualityTools.Testing.Fakes;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.TestProbeResolverSettingsTests
{
    public class TestBase : TestKit
    {
        private readonly IDisposable _shimContext;

        internal TestProbeResolver ConstructedTestProbeResolver;
        internal int TestProbeResolverConstructorCount;
        internal SutCreator ConstructedSutCreator;
        internal int SutCreatorConstructorCount;
        internal ChildTeller ConstructedChildTeller;
        internal int ChildTellerConstructorCount;
        internal ChildWaiter ConstructedChildWaiter;
        internal int ChildWaiterConstructorCount;
        internal DependencyResolverAdder ConstructedDependencyResolverAdder;
        internal int DependencyResolverAdderConstructorCount;
        internal TestProbeDependencyResolverAdder ConstructedTestProbeDependencyResolverAdder;
        internal int TestProbeDependencyResolverAdderConstructorCount;
        internal TestProbeCreator ConstructedTestProbeCreator;
        internal int TestProbeCreatorConstructorCount;
        internal ResolvedTestProbeStore ConstructedResolvedTestProbeStore;
        internal int ResolvedTestProbeStoreConstructorCount;
        internal TestProbeActorCreator ConstructedTestProbeActorCreator;
        internal int TestProbeActorCreatorConstructorCount;
        internal TestProbeHandlersMapper ConstructedTestProbeHandlersMapper;
        internal int TestProbeHandlersMapperConstructorCount;

        internal ISutCreator SutCreatorPassedIntoShim;
        internal IChildTeller ChildTellerPassedIntoShim;
        internal IChildWaiter ChildWaiterPassedIntoShim;
        internal IDependencyResolverAdder DependencyResolverAdderPassedIntoShim;
        internal ITestProbeDependencyResolverAdder TestProbeDependencyResolverAdderPassedIntoShim;
        internal ITestProbeCreator TestProbeCreatorPassedIntoShim;
        internal IResolvedTestProbeStore ResolvedTestProbeStorePassedIntoShim;
        internal ITestProbeActorCreator TestProbeActorCreatorPassedIntoShim;
        internal ITestProbeHandlersMapper TestProbeHandlersMapperPassedIntoShim;

        internal TestKitBase TestKitPassedIntoShim;
        internal ImmutableDictionary<(Type, Type), Func<object, object>> HandlersPassedIntoShim;

        public TestBase() : base(AkkaConfig.Config)
        {
            // Create shims
            _shimContext = ShimsContext.Create();

            //Set up shims
            ShimSutCreator.Constructor = @this =>
            {
                SutCreatorConstructorCount++;
                ConstructedSutCreator = @this;
            };

            ShimChildTeller.Constructor = @this =>
            {
                ChildTellerConstructorCount++;
                ConstructedChildTeller = @this;
            };

            ShimChildWaiter.Constructor = @this =>
            {
                ChildWaiterConstructorCount++;
                ConstructedChildWaiter = @this;
            };

            ShimDependencyResolverAdder.Constructor = @this =>
            {
                DependencyResolverAdderConstructorCount++;
                ConstructedDependencyResolverAdder = @this;
            };

            ShimTestProbeDependencyResolverAdder.Constructor = @this =>
            {
                TestProbeDependencyResolverAdderConstructorCount++;
                ConstructedTestProbeDependencyResolverAdder = @this;
            };

            ShimTestProbeCreator.Constructor = @this =>
            {
                TestProbeCreatorConstructorCount++;
                ConstructedTestProbeCreator = @this;
            };

            ShimResolvedTestProbeStore.Constructor = @this =>
            {
                ResolvedTestProbeStoreConstructorCount++;
                ConstructedResolvedTestProbeStore = @this;
            };

            ShimTestProbeActorCreator.Constructor = @this =>
            {
                TestProbeActorCreatorConstructorCount++;
                ConstructedTestProbeActorCreator = @this;
            };

            ShimTestProbeHandlersMapper.Constructor = @this =>
            {
                TestProbeHandlersMapperConstructorCount++;
                ConstructedTestProbeHandlersMapper = @this;
            };
            
            ShimTestProbeResolver.ConstructorISutCreatorIChildTellerIChildWaiterIDependencyResolverAdderITestProbeDependencyResolverAdderITestProbeCreatorIResolvedTestProbeStoreITestProbeActorCreatorITestProbeHandlersMapperTestKitBaseImmutableDictionaryOfValueTupleOfTypeTypeFuncOfObjectObj =
                (@this, sutCreator, childTeller, childWaiter, dependencyResolverAdder, testProbeDependencyResolverAdder, testProbeCreator, resolvedTestProbeStore, testProbeActorCreator, testProbeHandlersMapper, testKit, handlers) =>
                {
                    TestProbeResolverConstructorCount++;
                    ConstructedTestProbeResolver = @this;
                    SutCreatorPassedIntoShim = sutCreator;
                    ChildTellerPassedIntoShim = childTeller;
                    ChildWaiterPassedIntoShim = childWaiter;
                    DependencyResolverAdderPassedIntoShim = dependencyResolverAdder;
                    TestProbeDependencyResolverAdderPassedIntoShim = testProbeDependencyResolverAdder;
                    TestProbeCreatorPassedIntoShim = testProbeCreator;
                    ResolvedTestProbeStorePassedIntoShim = resolvedTestProbeStore;
                    TestProbeActorCreatorPassedIntoShim = testProbeActorCreator;
                    TestProbeHandlersMapperPassedIntoShim = testProbeHandlersMapper;
                    TestKitPassedIntoShim = testKit;
                    HandlersPassedIntoShim = handlers;
                };
        }

        protected override void Dispose(bool disposing)
        {
            _shimContext.Dispose();
            base.Dispose(disposing);
        }

        protected class DummyActor1 : ReceiveActor { }

        protected class DummyActor2 : ReceiveActor { }
        
        protected class Message1 { }
        
        protected class Reply1
        {
            //public Message1 Message1 { get; }

            //public Reply1(Message1 message1)
            //{
            //    Message1 = message1;
            //}
        }

        protected class Message2 { }

        protected class Reply2
        {
            //public Message2 Message2 { get; }

            //public Reply2(Message2 message2)
            //{
            //    Message2 = message2;
            //}
        }
    }
}