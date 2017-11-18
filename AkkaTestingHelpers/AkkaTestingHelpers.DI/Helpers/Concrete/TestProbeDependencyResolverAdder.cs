using System;
using System.Collections.Immutable;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract;

namespace ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete
{
    internal sealed class TestProbeDependencyResolverAdder : ITestProbeDependencyResolverAdder
    {
        public void Add(
            IDependencyResolverAdder dependencyResolverAdder, 
            ITestProbeActorCreator testProbeActorCreator,
            ITestProbeCreator testProbeCreator,
            IResolvedTestProbeStore resolvedTestProbeStore,
            IChildWaiter childWaiter,
            TestKitBase testKit, 
            ImmutableDictionary<Type, ImmutableDictionary<Type, Func<object, object>>> handlers) => 
                dependencyResolverAdder.Add(testKit, actorType =>
                {
                    ImmutableDictionary<Type, Func<object, object>> actorHandlers = handlers.GetValueOrDefault(actorType, null);
                    ITestProbeActor probeActor = testProbeActorCreator.Create(testProbeCreator, testKit, actorHandlers);
                    resolvedTestProbeStore.ResolveProbe(probeActor.ActorPath, actorType, probeActor.TestProbe, probeActor.PropsSupervisorStrategy);
                    childWaiter.ResolvedChild();
                    return probeActor.Actor;
                });
    }
}