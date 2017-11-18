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

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.UnitTestFrameworkCreatorTests
{
    public class TestBase : TestKit
    {
        private readonly IDisposable _shimContext;

        internal TestKitBase TestKitPassedIntoSut;
        internal ImmutableDictionary<(Type, Type), Func<object, object>> HandlersPassedIntoSut;
        internal Props PropsPassedIntoSut;
        internal int NumberOfChildrenIntoSut;

        protected UnitTestFramework<DummyActor1> ConstructedUnitTestFramework;
        internal int UnitTestFrameworkConstructorCount;
        internal SutCreator ConstructedSutCreator;
        internal int SutCreatorConstructorCount;
        internal TellChildWaiter ConstructedTellChildWaiter;
        internal int TellChildWaiterConstructorCount;
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
        internal SutSupervisorStrategyGetter ConstructedSutSupervisorStrategyGetter;
        internal int SutSupervisorStrategyGetterConstructorCount;

        internal ISutCreator SutCreatorPassedIntoShim;
        internal ITellChildWaiter TellChildWaiterPassedIntoShim;
        internal IChildWaiter ChildWaiterPassedIntoShim;
        internal IDependencyResolverAdder DependencyResolverAdderPassedIntoShim;
        internal ITestProbeDependencyResolverAdder TestProbeDependencyResolverAdderPassedIntoShim;
        internal ITestProbeCreator TestProbeCreatorPassedIntoShim;
        internal IResolvedTestProbeStore ResolvedTestProbeStorePassedIntoShim;
        internal ITestProbeActorCreator TestProbeActorCreatorPassedIntoShim;
        internal ITestProbeHandlersMapper TestProbeHandlersMapperPassedIntoShim;
        internal ISutSupervisorStrategyGetter SutSupervisorStrategyGetterIntoShim;

        internal TestKitBase TestKitPassedIntoShim;
        internal ImmutableDictionary<(Type, Type), Func<object, object>> HandlersPassedIntoShim;
        internal Props PropsPassedIntoShim;
        internal int NumberOfChildrenIntoShim;

        public TestBase() : base(AkkaConfig.Config)
        {
            // Create values passed into sut
            TestKitPassedIntoSut = this;
            HandlersPassedIntoSut = ImmutableDictionary<(Type, Type), Func<object, object>>
                .Empty
                .Add((typeof(DummyActor1), typeof(Message1)), message1 => new Reply1())
                .Add((typeof(DummyActor1), typeof(Message2)), message1 => new Reply2())
                .Add((typeof(DummyActor2), typeof(Message1)), message1 => new Reply1());
            PropsPassedIntoSut = Props.Create<DummyActor1>();
            NumberOfChildrenIntoSut = TestUtils.Create<int>();

            // Create shims
            _shimContext = ShimsContext.Create();

            // Set up shims
            ShimSutCreator.Constructor = @this =>
            {
                SutCreatorConstructorCount++;
                ConstructedSutCreator = @this;
            };

            ShimTellChildWaiter.Constructor = @this =>
            {
                TellChildWaiterConstructorCount++;
                ConstructedTellChildWaiter = @this;
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

            ShimSutSupervisorStrategyGetter.Constructor = @this =>
            {
                SutSupervisorStrategyGetterConstructorCount++;
                ConstructedSutSupervisorStrategyGetter = @this;
            };


            ShimUnitTestFramework<DummyActor1>.ConstructorISutCreatorITellChildWaiterIChildWaiterIDependencyResolverAdderITestProbeDependencyResolverAdderITestProbeCreatorIResolvedTestProbeStoreITestProbeActorCreatorITestProbeHandlersMapperISutSupervisorStrategyGetterImmutableDictionaryOfValueTupleOfTy =
                (@this, sutCreator, tellChildWaiter, childWaiter, dependencyResolverAdder, testProbeDependencyResolverAdder, testProbeCreator, resolvedTestProbeStore, testProbeActorCreator, testProbeHandlersMapper, sutSupervisorStrategyGetter, handlers, testKit, props, numberOfChildren) =>
                {
                    UnitTestFrameworkConstructorCount++;
                    ConstructedUnitTestFramework = @this;
                    SutCreatorPassedIntoShim = sutCreator;
                    TellChildWaiterPassedIntoShim = tellChildWaiter;
                    ChildWaiterPassedIntoShim = childWaiter;
                    DependencyResolverAdderPassedIntoShim = dependencyResolverAdder;
                    TestProbeDependencyResolverAdderPassedIntoShim = testProbeDependencyResolverAdder;
                    TestProbeCreatorPassedIntoShim = testProbeCreator;
                    ResolvedTestProbeStorePassedIntoShim = resolvedTestProbeStore;
                    TestProbeActorCreatorPassedIntoShim = testProbeActorCreator;
                    TestProbeHandlersMapperPassedIntoShim = testProbeHandlersMapper;
                    SutSupervisorStrategyGetterIntoShim = sutSupervisorStrategyGetter;
                    HandlersPassedIntoShim = handlers;
                    TestKitPassedIntoShim = testKit;
                    PropsPassedIntoShim = props;
                    NumberOfChildrenIntoShim = numberOfChildren;
                };
        }

        protected UnitTestFrameworkCreator CreateUnitTestFrameworkCreator() => new UnitTestFrameworkCreator();

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