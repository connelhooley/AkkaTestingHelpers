using System;
using System.Collections.Immutable;
using Akka.Actor;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.DI.Actors.Abstract;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract;
using EitherFactory = Akka.Util.Either<System.Func<Akka.Actor.ActorBase>, ConnelHooley.AkkaTestingHelpers.DI.IRegisterableActorFake>;

namespace ConnelHooley.AkkaTestingHelpers.DI
{
    public sealed class ConcreteResolver
    {
        private readonly ISutCreator _sutCreator;
        private readonly IChildTeller _childTeller;
        private readonly IChildWaiter _childWaiter;
        private readonly ITestProbeActorCreator _testProbeActorCreator;
        private readonly TestKitBase _testKit;
        private readonly ImmutableDictionary<Type, EitherFactory> _factories;
        
        internal ConcreteResolver(
            IDependencyResolverAdder resolverAdder, 
            ISutCreator sutCreator, 
            IChildTeller childTeller,
            IChildWaiter childWaiter,
            ITestProbeActorCreator testProbeActorCreator,
            TestKitBase testKit, 
            ImmutableDictionary<Type, EitherFactory> factories)
        {
            _sutCreator = sutCreator;
            _childTeller = childTeller;
            _childWaiter = childWaiter;
            _testKit = testKit;
            _factories = factories;
            _testProbeActorCreator = testProbeActorCreator;

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
            ActorBase result = _factories[actorType].Fold(
                factory => factory(),
                fake =>
                {
                    ITestProbeActor propeActor = _testProbeActorCreator.Create(_testKit);
                    fake.RegisterActor(propeActor);
                    return propeActor.Actor;
                });
            _childWaiter.ResolvedChild();
            return result;
        }
    }
}