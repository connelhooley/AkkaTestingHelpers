using System;
using System.Collections.Immutable;
using Akka.Actor;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract;

namespace ConnelHooley.AkkaTestingHelpers.DI
{
    public class ConcreteResolver
    {
        private readonly ISutCreator _sutCreator;
        private readonly IChildTeller _childTeller;
        private readonly IChildWaiter _childWaiter;
        private readonly TestKitBase _testKit;
        private readonly ImmutableDictionary<Type, Func<ActorBase>> _factories;

        internal ConcreteResolver(
            IDependencyResolverAdder resolverAdder, 
            ISutCreator sutCreator, 
            IChildTeller childTeller,
            IChildWaiter childWaiter, 
            TestKitBase testKit, 
            ImmutableDictionary<Type, Func<ActorBase>> factories)
        {
            _sutCreator = sutCreator;
            _childTeller = childTeller;
            _childWaiter = childWaiter;
            _testKit = testKit;
            _factories = factories;

            resolverAdder.Add(testKit, Resolve);
        }
        
        public TestActorRef<TActor> CreateSut<TActor>(Props props, int expectedChildrenCount) where TActor : ActorBase =>
            _sutCreator.Create<TActor>(
                _childWaiter, 
                _testKit, 
                props,
                expectedChildrenCount);
        
        public TestActorRef<TActor> CreateSut<TActor>(int expectedChildrenCount) where TActor : ActorBase, new() =>
            _sutCreator.Create<TActor>(
                _childWaiter,
                _testKit,
                Props.Create<TActor>(),
                expectedChildrenCount);
        
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
            if (!_factories.ContainsKey(actorType))
            {
                throw new InvalidOperationException($"Please register the type '{actorType.Name}' in the settings");
            }
            ActorBase result = _factories[actorType]();
            _childWaiter.ResolvedChild();
            return result;
        }
    }
}