using System;
using System.Collections.Immutable;
using Akka.Actor;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract;

namespace ConnelHooley.AkkaTestingHelpers.DI
{
    public class TestProbeResolver
    {
        private readonly ISutCreator _sutCreator;
        private readonly IChildTeller _childTeller;
        private readonly IChildWaiter _childWaiter;
        private readonly IResolvedTestProbeStore _resolvedProbeStore;
        private readonly ITestProbeActorFactory _actorFactory;
        private readonly TestKitBase _testKit;
        private readonly ImmutableDictionary<Type, ImmutableDictionary<Type, Func<object, object>>> _handlers;

        internal TestProbeResolver(IDependencyResolverAdder resolverAdder, ISutCreator sutCreator, IChildTeller childTeller, IChildWaiter childWaiter, IResolvedTestProbeStore resolvedProbeStore, ITestProbeCreator testProbeCreator, ITestProbeActorFactory actorFactory, ITestProbeHandlersMapper handlersMapper, TestKitBase testKit, TestProbeResolverSettings settings)
        {
            _sutCreator = sutCreator;
            _childTeller = childTeller;
            _childWaiter = childWaiter;
            _resolvedProbeStore = resolvedProbeStore;
            _actorFactory = actorFactory;
            _testKit = testKit;
            _handlers = handlersMapper.Map(settings.Handlers);
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
            (ActorBase actor, ActorPath actorPath, TestProbe testProbe) = _actorFactory.Create(_testKit, actorType, _handlers);
            _resolvedProbeStore.ResolveProbe(actorPath, actorType, testProbe);
            _childWaiter.ResolvedChild();
            return actor;
        }
    }
}