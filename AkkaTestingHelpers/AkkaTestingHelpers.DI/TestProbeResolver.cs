using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using Akka.Actor;
using Akka.TestKit;

namespace ConnelHooley.AkkaTestingHelpers.DI
{
    public class TestProbeResolver : ResolverBase
    {
        private readonly IImmutableDictionary<Type, Func<object, object>> _handlers;
        private readonly IDictionary<ActorPath, (Type, TestProbe)> _resolvedProbes;

        private TestProbeResolver(TestKitBase testKit, TestProbeResolverSettings settings) : base(testKit)
        {
            _handlers = settings.Handlers;
            _resolvedProbes = new ConcurrentDictionary<ActorPath, (Type, TestProbe)>();
        }

        public static TestProbeResolver Create(TestKitBase testKit, TestProbeResolverSettings settings) =>
            new TestProbeResolver(testKit, settings);

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
            ActorPath childPath = parentActor.Path.Child(childName);
            if (!_resolvedProbes.ContainsKey(childPath))
            {
                throw new InvalidOperationException($"No child has been resolved for the path '{childPath}'");
            }
            return _resolvedProbes[parentActor.Path.Child(childName)].Item1;
        }

        protected override ActorBase Resolve(Type actorType)
        {
            TestProbeActor actor = _handlers.ContainsKey(actorType)
                ? new TestProbeActor(TestKit, _handlers[actorType])
                : new TestProbeActor(TestKit);
            _resolvedProbes[actor.ActorPath] = (actorType, actor.TestProbe);
            return actor;
        }
    }
}