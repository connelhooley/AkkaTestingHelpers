using System;
using System.Collections.Immutable;
using Akka.Actor;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract;

namespace ConnelHooley.AkkaTestingHelpers.DI
{
    public sealed class ConcreteResolver
    {
        private readonly ISutCreator _sutCreator;
        private readonly IChildTeller _childTeller;
        private readonly IChildWaiter _childWaiter;
        private readonly TestKitBase _testKit;
        
        internal ConcreteResolver(
            ISutCreator sutCreator, 
            IChildTeller childTeller,
            IChildWaiter childWaiter,
            IDependencyResolverAdder resolverAdder, 
            IConcreteDependencyResolverAdder concreteDependencyResolverAdder,
            TestKitBase testKit, 
            ImmutableDictionary<Type, Func<ActorBase>> factories)
        {
            _sutCreator = sutCreator;
            _childTeller = childTeller;
            _childWaiter = childWaiter;
            _testKit = testKit;

            concreteDependencyResolverAdder.Add(
                resolverAdder,
                childWaiter,
                testKit,
                factories);
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
    }
}