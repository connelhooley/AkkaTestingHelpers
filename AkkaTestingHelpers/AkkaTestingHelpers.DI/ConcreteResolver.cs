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
        private readonly IChildWaiter _childWaiter;
        private readonly TestKitBase _testKit;
        private readonly IImmutableDictionary<Type, Func<ActorBase>> _factories;

        internal ConcreteResolver(
            IDependencyResolverAdder resolverAdder, 
            ISutCreator sutCreator, 
            IChildWaiter childWaiter, 
            TestKitBase testKit, 
            ConcreteResolverSettings settings)
        {
            _sutCreator = sutCreator;
            _childWaiter = childWaiter;
            _testKit = testKit;
            _factories = settings.Factories;
            resolverAdder.Add(testKit, Resolve);
        }
        
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
        
        public void TellMessage<TMessage>(IActorRef recipient, TMessage message, int expectedChildrenCount)
        {
            _childWaiter.Start(_testKit, expectedChildrenCount);
            recipient.Tell(message);
            _childWaiter.Wait();
        }

        public void TellMessage<TMessage>(IActorRef recipient, TMessage message, IActorRef sender, int expectedChildrenCount)
        {
            _childWaiter.Start(_testKit, expectedChildrenCount);
            recipient.Tell(message, sender);
            _childWaiter.Wait();
        }
        
        public void ForwardMessage<TMessage>(IActorRef recipient, TMessage message, int expectedChildrenCount)
        {
            _childWaiter.Start(_testKit, expectedChildrenCount);
            recipient.Forward(message);
            _childWaiter.Wait();
        }
        
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