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

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.ConcreteResolverSettingsTests
{
    public class TestBase : TestKit
    {
        private readonly IDisposable _shimContext;

        internal ConcreteResolver ConstructedConcreteResolver;
        internal int ConcreteResolverConstructorCount;
        internal SutCreator ConstructedSutCreator;
        internal int SutCreatorConstructorCount;
        internal ChildTeller ConstructedChildTeller;
        internal int ChildTellerConstructorCount;
        internal ChildWaiter ConstructedChildWaiter;
        internal int ChildWaiterConstructorCount;
        internal DependencyResolverAdder ConstructedDependencyResolverAdder;
        internal int DependencyResolverAdderConstructorCount;
        internal ConcreteDependencyResolverAdder ConstructedConcreteDependencyResolverAdder;
        internal int ConcreteDependencyResolverAdderConstructorCount;
        
        internal ISutCreator SutCreatorPassedIntoShim;
        internal IChildTeller ChildTellerPassedIntoShim;
        internal IChildWaiter ChildWaiterPassedIntoShim;
        internal IDependencyResolverAdder DependencyResolverAdderPassedIntoShim;
        internal IConcreteDependencyResolverAdder ConcreteDependencyResolverAdderPassedIntoShim;
        internal TestKitBase TestKitPassedIntoShim;
        internal ImmutableDictionary<Type, Func<ActorBase>> FactoriesPassedIntoShim;

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

            ShimConcreteDependencyResolverAdder.Constructor = @this =>
            {
                ConcreteDependencyResolverAdderConstructorCount++;
                ConstructedConcreteDependencyResolverAdder = @this;
            };

            ShimConcreteResolver.ConstructorISutCreatorIChildTellerIChildWaiterIDependencyResolverAdderIConcreteDependencyResolverAdderTestKitBaseImmutableDictionaryOfTypeFuncOfActorBase =
                (@this, sutCreator, childTeller, childWaiter, dependencyResolverAdder, concreteDependencyResolverAdder, testKit, factories) =>
                {
                    ConcreteResolverConstructorCount++; 
                    ConstructedConcreteResolver = @this;
                    SutCreatorPassedIntoShim = sutCreator;
                    ChildTellerPassedIntoShim = childTeller;
                    ChildWaiterPassedIntoShim = childWaiter;
                    DependencyResolverAdderPassedIntoShim = dependencyResolverAdder;
                    ConcreteDependencyResolverAdderPassedIntoShim = concreteDependencyResolverAdder;
                    TestKitPassedIntoShim = testKit;
                    FactoriesPassedIntoShim = factories;
                };
        }

        protected override void Dispose(bool disposing)
        {
            _shimContext.Dispose();
            base.Dispose(disposing);
        }

        protected class DummyActor1 : ReceiveActor { }

        protected class DummyActor2 : ReceiveActor { }
    }
}