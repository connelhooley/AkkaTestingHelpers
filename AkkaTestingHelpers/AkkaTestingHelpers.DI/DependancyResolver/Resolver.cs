using System;
using System.Collections.Immutable;
using Akka.Actor;
using Akka.TestKit;

namespace ConnelHooley.AkkaTestingHelpers.DI.DependancyResolver
{
    public class Resolver : ResolverBase
    {
        private readonly IImmutableDictionary<Type, Func<ActorBase>> _factories;

        internal Resolver(TestKitBase testKit, Settings settings) : base(testKit)
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