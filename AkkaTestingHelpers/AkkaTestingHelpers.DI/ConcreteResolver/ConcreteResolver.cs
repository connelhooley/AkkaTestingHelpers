using System;
using System.Collections.Immutable;
using Akka.Actor;
using Akka.TestKit;

namespace ConnelHooley.AkkaTestingHelpers.DI.ConcreteResolver
{
    public class ConcreteResolver : ResolverBase
    {
        private readonly IImmutableDictionary<Type, Func<ActorBase>> _factories;

        internal ConcreteResolver(TestKitBase testKit, ConcreteResolverSettings settings) : base(testKit)
        {
            _factories = settings.Factories;
        }

        protected override Func<ActorBase> Resolve(Type actorType)
        {
            if (!_factories.ContainsKey(actorType))
            {
                throw new InvalidOperationException($"Please register the type '{actorType.Name}' in the settings");
            }
            return () =>
            {
                ActorBase actor = _factories[actorType]();
                ResolvedChild();
                return actor;
            };
        }
    }
}