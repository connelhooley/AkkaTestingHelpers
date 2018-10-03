using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Akka.Actor;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.Helpers.Abstract;

namespace ConnelHooley.AkkaTestingHelpers
{
    public sealed class UnitTestFramework<TActor> where TActor : ActorBase
    {
        private readonly ITellWaiter _tellWaiter;
        private readonly IWaiter _childWaiter;
        private readonly IWaiter _exceptionWaiter;
        private readonly IResolvedTestProbeStore _resolvedProbeStore;
        private readonly TestKitBase _testKit;
        private readonly SupervisorStrategy _sutSupervisorStrategy;

        internal UnitTestFramework(
            ISutCreator sutCreator,
            ITellWaiter tellWaiter,
            IWaiter childWaiter,
            IWaiter exceptionWaiter,
            IDependencyResolverAdder resolverAdder,
            ITestProbeDependencyResolverAdder testProbeDependencyResolverAdder,
            ITestProbeCreator testProbeCreator,
            IResolvedTestProbeStore resolvedProbeStore,
            ITestProbeChildActorCreator testProbeChildActorCreator,
            ITestProbeChildHandlersMapper testProbeChildHandlersMapper,
            ISutSupervisorStrategyGetter sutSupervisorStrategyGetter,
            ITestProbeParentActorCreator testProbeParentActorCreator,
            ImmutableDictionary<Type, Func<object, object>> parentHandlers,
            ImmutableDictionary<(Type, Type), Func<object, object>> childHandlers,
            TestKitBase testKit,
            Props sutProps,
            Func<Exception, Directive> supervisorDecider,
            int expectedChildrenCount)
        {
            if (sutProps.SupervisorStrategy != null)
            {
                throw new InvalidOperationException("Do not use Prop objects with supervisor stratergies to create your SUT actor as you cannot garentee your actor will be created with this stratergy in production.");
            }

            _tellWaiter = tellWaiter;
            _childWaiter = childWaiter;
            _exceptionWaiter = exceptionWaiter;
            _resolvedProbeStore = resolvedProbeStore;
            _testKit = testKit;

            testProbeDependencyResolverAdder.Add(
                resolverAdder,
                testProbeChildActorCreator,
                testProbeCreator,
                resolvedProbeStore,
                childWaiter,
                testKit,
                testProbeChildHandlersMapper.Map(childHandlers));

            ITestProbeParentActor testProbeParentActor = testProbeParentActorCreator.Create(
                testProbeCreator,
                exceptionWaiter,
                testKit,
                supervisorDecider,
                parentHandlers);

            Sut = sutCreator.Create<TActor>(
                childWaiter,
                testKit,
                sutProps,
                expectedChildrenCount,
                testProbeParentActor.Ref);

            Parent = testProbeParentActor.TestProbe;

            UnhandledExceptions = testProbeParentActor.UnhandledExceptions;

            _sutSupervisorStrategy = sutSupervisorStrategyGetter.Get(Sut.UnderlyingActor);
        }

        /// <summary>
        /// The TestProbe that is the parent/superivsor of the sut actor
        /// </summary>
        public TestProbe Parent { get; }

        /// <summary>
        /// The Actor that is the subject of your tests.
        /// </summary>
        public TestActorRef<TActor> Sut { get; }

        /// <summary>
        /// Contains all the unhandled exceptions thrown by the sut actor.
        /// </summary>
        public IEnumerable<Exception> UnhandledExceptions { get; }

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
            _tellWaiter.TellMessage(
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
            _tellWaiter.TellMessage(
                _childWaiter,
                _testKit,
                Sut,
                message,
                waitForChildrenCount,
                sender);

        /// <summary>
        /// Sends the SUT actor a message whilst waiting for the first exception to be thrown before continuing.
        /// </summary>
        /// <typeparam name="TMessage">The type of message to send</typeparam>
        /// <param name="message">The message to send</param>
        public void TellMessageAndWaitForException<TMessage>(TMessage message) =>
            _tellWaiter.TellMessage(
                _exceptionWaiter,
                _testKit,
                Sut,
                message,
                1);

        /// <summary>
        /// Sends the SUT actor a message whilst waiting for the first exception to be thrown before continuing.
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="message">The message to send</param>
        /// <param name="sender">The actor to send the message from</param>
        public void TellMessageAndWaitForException<TMessage>(TMessage message, IActorRef sender) =>
            _tellWaiter.TellMessage(
                _exceptionWaiter,
                _testKit,
                Sut,
                message,
                1,
                sender);

        /// <summary>
        /// Sends the SUT actor a message whilst waiting for the expected number of exceptions to be thrown before continuing.
        /// </summary>
        /// <typeparam name="TMessage">The type of message to send</typeparam>
        /// <param name="message">The message to send</param>
        /// <param name="waitForExceptionCount">The number exceptions to wait for</param>
        public void TellMessageAndWaitForExceptions<TMessage>(TMessage message, int waitForExceptionCount) =>
            _tellWaiter.TellMessage(
                _exceptionWaiter,
                _testKit,
                Sut,
                message,
                waitForExceptionCount);

        /// <summary>
        /// Sends the SUT actor a message whilst waiting for the expected number of exceptions to be thrown before continuing.
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="message">The message to send</param>
        /// <param name="sender">The actor to send the message from</param>
        /// <param name="waitForExceptionCount">The number exceptions to wait for</param>
        public void TellMessageAndWaitForExceptions<TMessage>(TMessage message, IActorRef sender, int waitForExceptionCount) =>
            _tellWaiter.TellMessage(
                _exceptionWaiter,
                _testKit,
                Sut,
                message,
                waitForExceptionCount,
                sender);
    }
}