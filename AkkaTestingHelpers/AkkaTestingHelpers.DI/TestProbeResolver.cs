using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Akka.Actor;
using Akka.TestKit;

namespace ConnelHooley.AkkaTestingHelpers.DI
{
    public class TestProbeResolver : ResolverBase
    {
        private readonly ImmutableDictionary<Type, ImmutableDictionary<Type, Func<object, object>>> _handlers;
        private readonly IDictionary<ActorPath, (Type, TestProbe)> _resolved;
        private readonly TestProbe _supervisor;

        internal TestProbeResolver(TestKitBase testKit, TestProbeResolverSettings settings) : base(testKit)
        {
            _handlers = settings.Handlers
                .ToLookup(
                    pair => pair.Key.Item1, 
                    pair => new {messageType = pair.Key.Item2, messageHandler = pair.Value})
                .ToImmutableDictionary(
                    grouping => grouping.Key,
                    grouping => grouping.ToImmutableDictionary(
                        item => item.messageType, item => item.messageHandler));
            _resolved = new ConcurrentDictionary<ActorPath, (Type, TestProbe)>();
            _supervisor = testKit.CreateTestProbe();
        }
        
        public TestProbe GetSupervisor() => _supervisor;

        public TestProbe GetTestProbe(IActorRef parentActor, string childName) => 
            GetResolved(parentActor, childName).Item2;

        public Type GetType(IActorRef parentActor, string childName) => 
            GetResolved(parentActor, childName).Item1;

        private (Type, TestProbe) GetResolved(IActorRef parentActor, string childName)
        {
            ActorPath childPath = parentActor.Path.Child(childName);
            if (!_resolved.ContainsKey(childPath))
            {
                throw new InvalidOperationException($"No child has been resolved for the path '{childPath}'");
            }
            return _resolved[childPath];
        }

        protected override ActorBase Resolve(Type actorType)
        {
            TestProbeActor actor = _handlers.ContainsKey(actorType)
                ? new TestProbeActor(TestKit, _handlers[actorType])
                : new TestProbeActor(TestKit);
            _resolved[actor.ActorPath] = (actorType, actor.TestProbe);
            return actor;
        }
    }
}