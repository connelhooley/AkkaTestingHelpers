using System;
using System.Collections.Immutable;
using Akka.Actor;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract;

namespace ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete
{
    internal sealed class ConcreteDependencyResolverAdder : IConcreteDependencyResolverAdder
    {
        public void Add(
            IDependencyResolverAdder dependencyResolverAdder, 
            IChildWaiter childWaiter, 
            TestKitBase testKit,
            ImmutableDictionary<Type, Func<ActorBase>> factories) => 
                dependencyResolverAdder.Add(testKit, actorType =>
                {
                    if (factories.TryGetValue(actorType, out Func<ActorBase> factory))
                    {
                        ActorBase actor = factory();
                        childWaiter.ResolvedChild();
                        return actor;
                    }
                    throw new InvalidOperationException($"Please register the type '{actorType.Name}' in the settings");
                });
    }
}