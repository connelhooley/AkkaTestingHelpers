using System;
using System.Collections.Immutable;
using Akka.Actor;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.DI.Actors.Abstract;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract;

namespace ConnelHooley.AkkaTestingHelpers.DI
{
    public class TestProbeResolver
    {
        private readonly ISutCreator _sutCreator;
        private readonly IChildTeller _childTeller;
        private readonly IChildWaiter _childWaiter;
        private readonly IResolvedTestProbeStore _resolvedProbeStore;
        private readonly ITestProbeActorCreator _actorCreator;
        private readonly TestKitBase _testKit;
        private readonly ImmutableDictionary<Type, ImmutableDictionary<Type, Func<object, object>>> _handlers;

        internal TestProbeResolver(
            IDependencyResolverAdder resolverAdder, 
            ISutCreator sutCreator, 
            IChildTeller childTeller, 
            IChildWaiter childWaiter, 
            IResolvedTestProbeStore resolvedProbeStore, 
            ITestProbeCreator testProbeCreator, 
            ITestProbeActorCreator testProbeActorCreator, 
            ITestProbeHandlersMapper handlersMapper, 
            TestKitBase testKit, 
            ImmutableDictionary<(Type, Type), Func<object, object>> handlers)
        {
            _sutCreator = sutCreator;
            _childTeller = childTeller;
            _childWaiter = childWaiter;
            _resolvedProbeStore = resolvedProbeStore;
            _actorCreator = testProbeActorCreator;
            _testKit = testKit;
            _handlers = handlersMapper.Map(handlers);
            Supervisor = testProbeCreator.Create(testKit);
            resolverAdder.Add(testKit, Resolve);
        }

        public TestProbe Supervisor { get; }

        public TestProbe ResolvedTestProbe(IActorRef parentActor, string childName) =>
            _resolvedProbeStore.FindResolvedTestProbe(parentActor, childName);

        public Type ResolvedType(IActorRef parentActor, string childName) =>
            _resolvedProbeStore.FindResolvedType(parentActor, childName);

        public TestActorRef<TActor> CreateSut<TActor>(Props props, int expectedChildrenCount) where TActor : ActorBase =>
            _sutCreator.Create<TActor>(
                _childWaiter,
                _testKit,
                props,
                expectedChildrenCount,
                Supervisor);
        
        public TestActorRef<TActor> CreateSut<TActor>(int expectedChildrenCount) where TActor : ActorBase, new() =>
            _sutCreator.Create<TActor>(
                _childWaiter,
                _testKit,
                Props.Create<TActor>(),
                expectedChildrenCount,
                Supervisor);

        public void TellMessage<TMessage>(IActorRef recipient, TMessage message, int waitForChildrenCount) =>
            _childTeller.TellMessage(
                _childWaiter,
                _testKit,
                recipient,
                message,
                waitForChildrenCount);

        public void TellMessage<TMessage>(IActorRef recipient, TMessage message, IActorRef sender, int waitForChildrenCount) =>
            _childTeller.TellMessage(
                _childWaiter,
                _testKit,
                recipient,
                message,
                waitForChildrenCount,
                sender);

        private ActorBase Resolve(Type actorType)
        {
            ITestProbeActor probeActor = _actorCreator.Create(_testKit);
            if (_handlers.ContainsKey(actorType))
            {
                probeActor.SetHandlers(_handlers[actorType]);
            }
            _resolvedProbeStore.ResolveProbe(probeActor.ActorPath, actorType, probeActor.TestProbe);
            _childWaiter.ResolvedChild();
            return probeActor.Actor;
        }
    }
}