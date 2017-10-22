using System;
using System.Collections.Immutable;
using Akka.TestKit;

namespace ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract
{
    internal interface ITestProbeDependencyResolverAdder
    {
        void Add(
            IDependencyResolverAdder dependencyResolverAdder, 
            ITestProbeActorCreator testProbeActorCreator,
            ITestProbeCreator testProbeCreator,
            IResolvedTestProbeStore resolvedTestProbeStore, 
            IChildWaiter childWaiter, 
            TestKitBase testKit, 
            ImmutableDictionary<Type, ImmutableDictionary<Type, Func<object, object>>> handlers);
    }
}