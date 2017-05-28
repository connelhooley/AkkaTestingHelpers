using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Akka.Actor;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract;

namespace ConnelHooley.AkkaTestingHelpers.DI
{
    public class TestProbeResolver
    {
        private readonly ISutCreator _sutCreator;
        private readonly IChildWaiter _childWaiter;
        private readonly TestKitBase _testKit;
        private readonly ImmutableDictionary<Type, ImmutableDictionary<Type, Func<object, object>>> _handlers;
        private readonly IDictionary<ActorPath, (Type, TestProbe)> _resolved;

        internal TestProbeResolver(IDependencyResolverAdder resolverAdder, ISutCreator sutCreator, IChildWaiter childWaiter, TestKitBase testKit, TestProbeResolverSettings settings)
        {
            _sutCreator = sutCreator;
            _childWaiter = childWaiter;
            _testKit = testKit;
            _handlers = settings.Handlers
                .ToLookup(
                    pair => pair.Key.Item1, 
                    pair => new {messageType = pair.Key.Item2, messageHandler = pair.Value})
                .ToImmutableDictionary(
                    grouping => grouping.Key,
                    grouping => grouping.ToImmutableDictionary(
                        item => item.messageType, item => item.messageHandler));
            _resolved = new ConcurrentDictionary<ActorPath, (Type, TestProbe)>();
            Supervisor = testKit.CreateTestProbe();
            resolverAdder.Add(testKit, Resolve);
        }

        public TestProbe Supervisor { get; }

        public TestProbe ResolvedTestProbe(IActorRef parentActor, string childName) => 
            FindResolved(parentActor, childName).Item2;

        public Type ResolvedType(IActorRef parentActor, string childName) => 
            FindResolved(parentActor, childName).Item1;

        public TestActorRef<TActor> CreateSut<TActor>(Props props, int expectedChildrenCount) where TActor : ActorBase =>
            _sutCreator.Create<TActor>(
                _childWaiter,
                _testKit,
                props,
                expectedChildrenCount);

        public TestActorRef<TActor> CreateSut<TActor>(Props props, IActorRef supervisor, int expectedChildrenCount) where TActor : ActorBase =>
            _sutCreator.Create<TActor>(
                _childWaiter,
                _testKit,
                props,
                expectedChildrenCount,
                supervisor);

        public TestActorRef<TActor> CreateSut<TActor>(int expectedChildrenCount) where TActor : ActorBase, new() =>
            _sutCreator.Create<TActor>(
                _childWaiter,
                _testKit,
                Props.Create<TActor>(),
                expectedChildrenCount);

        public TestActorRef<TActor> CreateSut<TActor>(IActorRef supervisor, int expectedChildrenCount) where TActor : ActorBase, new() =>
            _sutCreator.Create<TActor>(
                _childWaiter,
                _testKit,
                Props.Create<TActor>(),
                expectedChildrenCount,
                supervisor);

        public void TellMessage<TMessage>(IActorRef recipient, TMessage message, int waitForChildrenCount)
        {
            _childWaiter.Start(_testKit, waitForChildrenCount);
            recipient.Tell(message);
            _childWaiter.Wait();
        }

        public void TellMessage<TMessage>(IActorRef recipient, TMessage message, IActorRef sender, int waitForChildrenCount)
        {
            _childWaiter.Start(_testKit, waitForChildrenCount);
            recipient.Tell(message, sender);
            _childWaiter.Wait();
        }

        private ActorBase Resolve(Type actorType)
        {
            TestProbeActor actor = _handlers.ContainsKey(actorType)
                ? new TestProbeActor(_testKit, _handlers[actorType])
                : new TestProbeActor(_testKit);
            _resolved[actor.ActorPath] = (actorType, actor.TestProbe);
            _childWaiter.ResolvedChild();
            return actor;
        }

        private (Type, TestProbe) FindResolved(IActorRef parentActor, string childName)
        {
            ActorPath childPath = parentActor.Path.Child(childName);
            if (!_resolved.ContainsKey(childPath))
            {
                throw new InvalidOperationException($"No child has been resolved for the path '{childPath}'");
            }
            return _resolved[childPath];
        }
    }
}