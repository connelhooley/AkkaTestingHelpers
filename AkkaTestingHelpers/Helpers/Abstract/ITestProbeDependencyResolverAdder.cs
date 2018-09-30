using System;
using System.Collections.Immutable;
using Akka.TestKit;

namespace ConnelHooley.AkkaTestingHelpers.Helpers.Abstract
{
    internal interface ITestProbeDependencyResolverAdder
    {
        void Add(
            IDependencyResolverAdder dependencyResolverAdder, 
            ITestProbeChildActorCreator testProbeChildActorCreator,
            ITestProbeCreator testProbeCreator,
            IResolvedTestProbeStore resolvedTestProbeStore, 
            IWaiter childWaiter, 
            TestKitBase testKit, 
            ImmutableDictionary<Type, ImmutableDictionary<Type, Func<object, object>>> handlers);
    }
}