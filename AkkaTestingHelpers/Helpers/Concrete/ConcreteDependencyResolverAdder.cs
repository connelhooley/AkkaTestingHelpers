using System;
using System.Collections.Immutable;
using Akka.Actor;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.Helpers.Abstract;

namespace ConnelHooley.AkkaTestingHelpers.Helpers.Concrete
{
    internal sealed class ConcreteDependencyResolverAdder : IConcreteDependencyResolverAdder
    {
        private readonly IDependencyResolverAdder _dependencyResolverAdder;

        public ConcreteDependencyResolverAdder(IDependencyResolverAdder dependencyResolverAdder) => _dependencyResolverAdder = dependencyResolverAdder;

        public void Add(
            TestKitBase testKit,
            ImmutableDictionary<Type, Func<ActorBase>> factories) => 
                _dependencyResolverAdder.Add(testKit, actorType =>
                {
                    if (factories.TryGetValue(actorType, out Func<ActorBase> factory))
                    {
                        return factory();
                    }
                    throw new InvalidOperationException($"Please register the type '{actorType.Name}' in the settings");
                });
    }
}