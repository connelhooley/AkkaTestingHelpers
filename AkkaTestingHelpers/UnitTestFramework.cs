﻿using System;
using System.Collections.Immutable;
using Akka.Actor;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.Helpers.Abstract;

namespace ConnelHooley.AkkaTestingHelpers
{
    public sealed class UnitTestFramework<TActor> where TActor : ActorBase
    {
        private readonly ITellChildWaiter _tellChildWaiter;
        private readonly IChildWaiter _childWaiter;
        private readonly IResolvedTestProbeStore _resolvedProbeStore;
        private readonly TestKitBase _testKit;
        private readonly SupervisorStrategy _sutSupervisorStrategy;

        internal UnitTestFramework(
            ISutCreator sutCreator, 
            ITellChildWaiter childTeller, 
            IChildWaiter childWaiter, 
            IDependencyResolverAdder resolverAdder, 
            ITestProbeDependencyResolverAdder testProbeDependencyResolverAdder,
            ITestProbeCreator testProbeCreator,
            IResolvedTestProbeStore resolvedProbeStore,
            ITestProbeActorCreator testProbeActorCreator, 
            ITestProbeHandlersMapper handlersMapper, 
            ISutSupervisorStrategyGetter sutSupervisorStrategyGetter,
            ImmutableDictionary<(Type, Type), Func<object, object>> handlers,
            TestKitBase testKit, 
            Props props,
            int expectedChildrenCount)
        {
            if (props.SupervisorStrategy != null)
            {
                throw new InvalidOperationException("Do not use Prop objects with supervisor stratergies to create your SUT actor as you cannot garentee your actor will be created with this stratergy in production.");
            }

            _tellChildWaiter = childTeller;
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

            Sut = sutCreator.Create<TActor>(
                _childWaiter,
                _testKit,
                props,
                expectedChildrenCount,
                Supervisor);

            _sutSupervisorStrategy = sutSupervisorStrategyGetter.Get(Sut.UnderlyingActor);
        }

        /// <summary>
        /// The TestProbe that is the parent/superivsor of the sut actor
        /// </summary>
        public TestProbe Supervisor { get; }

        /// <summary>
        /// The Actor that is the subject of your tests.
        /// </summary>
        public TestActorRef<TActor> Sut { get; }

        /// <summary>
        /// Finds the test probe created by the SUT actor for the given child name.
        /// </summary>
        /// <param name="childName">The name of the child</param>
        /// <returns>The test probe that was resolved</returns>
        public TestProbe ResolvedTestProbe(string childName) =>
            _resolvedProbeStore.FindResolvedTestProbe(Sut, childName);

        /// <summary>
        /// Finds the type of actor the SUT actor requested for the given child name.
        /// </summary>
        /// <param name="childName">The name of the child</param>
        /// <returns>The type of the actor requested</returns>
        public Type ResolvedType(string childName) =>
            _resolvedProbeStore.FindResolvedType(Sut, childName);

        /// <summary>
        /// Finds the SupervisorStrategy the SUT actor uses to supervise the given child name.
        /// </summary>
        /// <param name="childName">The name of the child</param>
        /// <returns>The SupervisorStrategy used</returns>
        public SupervisorStrategy ResolvedSupervisorStrategy(string childName) =>
            _resolvedProbeStore.FindResolvedSupervisorStrategy(Sut, childName) ?? 
            _sutSupervisorStrategy;

        /// <summary>
        /// Sends the SUT actor a message whilst waiting for the expected number of children to be resolved before continuing.
        /// </summary>
        /// <typeparam name="TMessage">The type of message to send</typeparam>
        /// <param name="message">The message to send</param>
        /// <param name="waitForChildrenCount">The number child actors to wait for</param>
        public void TellMessageAndWaitForChildren<TMessage>(TMessage message, int waitForChildrenCount) =>
            _tellChildWaiter.TellMessage(
                _childWaiter,
                _testKit,
                Sut,
                message,
                waitForChildrenCount);

        /// <summary>
        /// Sends the SUT actor a message whilst waiting for the expected number of children to be resolved before continuing.
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="message">The message to send</param>
        /// <param name="sender">The actor to send the message from</param>
        /// <param name="waitForChildrenCount">The number child actors to wait for</param>
        public void TellMessageAndWaitForChildren<TMessage>(TMessage message, IActorRef sender, int waitForChildrenCount) =>
            _tellChildWaiter.TellMessage(
                _childWaiter,
                _testKit,
                Sut,
                message,
                waitForChildrenCount,
                sender);
    }
}