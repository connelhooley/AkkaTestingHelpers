using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using Akka.Actor;
using Akka.TestKit;

namespace ConnelHooley.AkkaTestingHelpers.DI.TestProbeDependancyResolver
{
    public class Resolver : ResolverBase
    {
        private readonly IImmutableDictionary<Type, Func<object, object>> _handlers;
        private readonly IDictionary<ActorPath, (Type, TestProbe)> _resolvedProbes;

        internal Resolver(TestKitBase testKit, Settings settings) : base(testKit)
        {
            _handlers = settings.Handlers;
            _resolvedProbes = new ConcurrentDictionary<ActorPath, (Type, TestProbe)>();
        }

        public TestProbe GetTestProbe(IActorRef parentActor, string childName)
        {
            ActorPath childPath = parentActor.Path.Child(childName);
            if (!_resolvedProbes.ContainsKey(childPath))
            {
                throw new InvalidOperationException($"No child has been resolved for the path '{childPath}'");
            }
            return _resolvedProbes[parentActor.Path.Child(childName)].Item2;
        }

        public Type GetType(IActorRef parentActor, string childName)
        {
            return _resolvedProbes[parentActor.Path.Child(childName)].Item1;
        }

        protected override Func<ActorBase> Resolve(Type actorType) => 
            () =>
            {
                Actor actor = _handlers.ContainsKey(actorType)
                    ? new Actor(TestKit, _handlers[actorType])
                    : new Actor(TestKit);
                _resolvedProbes[actor.ActorPath] = (actorType, actor.TestProbe);
                ResolvedChild();
                return actor;
            };
    }
}