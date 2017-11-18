using System;
using Akka.TestKit.Xunit2;
using ConnelHooley.AkkaTestingHelpers.Helpers.Abstract;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete.Fakes;
using Microsoft.QualityTools.Testing.Fakes;

// ReSharper disable VirtualMemberCallInConstructor
// ReSharper disable RedundantAssignment

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.ConcreteDependencyResolverAdderCreatorTests
{
    public class TestBase : TestKit
    {
        internal int ConcreteDependencyResolverAdderConstructorCount;
        internal ConcreteDependencyResolverAdder ConstructedConcreteDependencyResolverAdder;
        internal int DependencyResolverAdderConstructorCount;
        internal DependencyResolverAdder ConstructedDependencyResolverAdder;
        internal IDependencyResolverAdder DependencyResolverAdderPassedIntoShim;
        private readonly IDisposable _shimContext;

        public TestBase() : base(AkkaConfig.Config)
        {
            // Create shims
            _shimContext = ShimsContext.Create();

            // Set up shims
            ShimDependencyResolverAdder.Constructor = @this =>
            {
                DependencyResolverAdderConstructorCount++;
                ConstructedDependencyResolverAdder = @this;
            };
            
            ShimConcreteDependencyResolverAdder.ConstructorIDependencyResolverAdder = (@this, dependencyResolverAdder) =>
            {
                ConcreteDependencyResolverAdderConstructorCount++;
                ConstructedConcreteDependencyResolverAdder = @this;
                DependencyResolverAdderPassedIntoShim = dependencyResolverAdder;
            };
        }

        internal ConcreteDependencyResolverAdderCreator CreateConcreteDependencyResolverAdderConcreteDependencyResolverAdderCreator() => 
            new ConcreteDependencyResolverAdderCreator();
        
        protected override void Dispose(bool disposing)
        {
            _shimContext.Dispose();
            base.Dispose(disposing);
        }
    }
}