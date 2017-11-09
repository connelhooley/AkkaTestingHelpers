using System;
using System.Collections.Immutable;
using Akka.Actor;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract;

namespace ConnelHooley.AkkaTestingHelpers.DI
{
    public sealed class TestProbeResolver<TActor> where TActor : ActorBase
    {
        private readonly ISutCreator _sutCreator;
        private readonly IChildTeller _childTeller;
        private readonly IChildWaiter _childWaiter;
        private readonly IResolvedTestProbeStore _resolvedProbeStore;
        private readonly TestKitBase _testKit;

        internal TestProbeResolver(
            ISutCreator sutCreator, 
            IChildTeller childTeller, 
            IChildWaiter childWaiter, 
            IDependencyResolverAdder resolverAdder, 
            ITestProbeDependencyResolverAdder testProbeDependencyResolverAdder,
            ITestProbeCreator testProbeCreator,
            IResolvedTestProbeStore resolvedProbeStore,
            ITestProbeActorCreator testProbeActorCreator, 
            ITestProbeHandlersMapper handlersMapper, 
            ImmutableDictionary<(Type, Type), Func<object, object>> handlers,
            TestKitBase testKit, 
            Props props,
            int expectedChildrenCount)
        {
            _sutCreator = sutCreator;
            _childTeller = childTeller;
            _childWaiter = childWaiter;
            _resolvedProbeStore = resolvedProbeStore;
            _testKit = testKit;
            
            testProbeDependencyResolverAdder.Add(
                resolverAdder,
                testProbeActorCreator,
                testProbeCreator,
                resolvedProbeStore,
                childWaiter,
                testKit,
                handlersMapper.Map(handlers));

            Supervisor = testProbeCreator.Create(testKit);

            Sut = _sutCreator.Create<TActor>(
                _childWaiter,
                _testKit,
                props,
                expectedChildrenCount,
                Supervisor);
        }

        /// <summary>
        /// The TestProbe that is the parent/superivsor for all actors created using the CreateSut method.
        /// </summary>
        public TestProbe Supervisor { get; }

        /// <summary>
        /// The Actor that is the subject of your tests.
        /// </summary>
        public TestActorRef<TActor> Sut { get; }

        /// <summary>
        /// Finds the test probe created by the given actor ref with the given child name.
        /// </summary>
        /// <param name="parentActor">The actor this is parent to the child</param>
        /// <param name="childName">The name of the child</param>
        /// <returns></returns>
        public TestProbe ResolvedTestProbe(IActorRef parentActor, string childName) =>
            _resolvedProbeStore.FindResolvedTestProbe(parentActor, childName);

        /// <summary>
        /// Finds the Type of actor the given actor ref requested for the given child name.
        /// </summary>
        /// <param name="parentActor">The actor this is parent to the child</param>
        /// <param name="childName">The name of the child</param>
        /// <returns></returns>
        public Type ResolvedType(IActorRef parentActor, string childName) =>
            _resolvedProbeStore.FindResolvedType(parentActor, childName);

        /// <summary>
        /// Creates an actor whilst waiting for the expected number of children to be resolved before returning.
        /// </summary>
        /// <typeparam name="TActor">The type of actor to create</typeparam>
        /// <param name="props">Props object used to create the actor</param>
        /// <param name="expectedChildrenCount">The number child actors to wait for</param>
        /// <returns>The created actor</returns>
        public TestActorRef<TActor> CreateSut<TActor>(Props props, int expectedChildrenCount) where TActor : ActorBase =>
            _sutCreator.Create<TActor>(
                _childWaiter,
                _testKit,
                props,
                expectedChildrenCount,
                Supervisor);

        /// <summary>
        /// Creates an actor whilst waiting for the expected number of children to be resolved before returning.
        /// </summary>
        /// <typeparam name="TActor">The type of actor to create</typeparam>
        /// <param name="expectedChildrenCount">The number child actors to wait for</param>
        /// <returns>The created actor</returns>
        public TestActorRef<TActor> CreateSut<TActor>(int expectedChildrenCount) where TActor : ActorBase, new() =>
            _sutCreator.Create<TActor>(
                _childWaiter,
                _testKit,
                Props.Create<TActor>(),
                expectedChildrenCount,
                Supervisor);

        /// <summary>
        /// Sends an actor a message whilst waiting for the expected number of children to be resolved before returning.
        /// </summary>
        /// <typeparam name="TMessage">The type of message to send</typeparam>
        /// <param name="recipient">The actor to send a message to</param>
        /// <param name="message">The message to send</param>
        /// <param name="waitForChildrenCount">The number child actors to wait for</param>
        public void TellMessage<TMessage>(IActorRef recipient, TMessage message, int waitForChildrenCount) =>
            _childTeller.TellMessage(
                _childWaiter,
                _testKit,
                recipient,
                message,
                waitForChildrenCount);

        /// <summary>
        /// Sends an actor a message whilst waiting for the expected number of children to be resolved before returning.
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="recipient">The actor to send a message to</param>
        /// <param name="message">The message to send</param>
        /// <param name="sender">The actor to send the message from</param>
        /// <param name="waitForChildrenCount">The number child actors to wait for</param>
        public void TellMessage<TMessage>(IActorRef recipient, TMessage message, IActorRef sender, int waitForChildrenCount) =>
            _childTeller.TellMessage(
                _childWaiter,
                _testKit,
                recipient,
                message,
                waitForChildrenCount,
                sender);

        public SupervisorStrategy ResolvedSupervisorStratergy<TActor>(TestActorRef<TActor> parentActor, string childName)
            where TActor : ActorBase
        {
            SupervisorStrategy supervisorStrategy = _resolvedProbeStore.FindSupervisorStratergy(parentActor, childName);
            if (supervisorStrategy == null)
            {
                
            }
        }
            

    }
}