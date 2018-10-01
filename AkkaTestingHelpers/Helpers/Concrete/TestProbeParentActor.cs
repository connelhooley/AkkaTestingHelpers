using Akka.Actor;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.Helpers.Abstract;
using System;
using System.Collections.Generic;

namespace ConnelHooley.AkkaTestingHelpers.Helpers.Concrete
{
    internal sealed class TestProbeParentActor : ReceiveActor, ITestProbeParentActor
    {
        private readonly IWaiter _exceptionWaiter;
        private readonly Func<Exception, Directive> _decider;
        private readonly List<Exception> _unhandledExceptions;

        public TestProbeParentActor(
            ITestProbeCreator testProbeCreator,
            IWaiter exceptionWaiter,
            TestKitBase testKit,
            Func<Exception, Directive> decider,
            IReadOnlyDictionary<Type, Func<object, object>> handlers)
        {
            _exceptionWaiter = exceptionWaiter;
            _decider = decider;
            _unhandledExceptions = new List<Exception>();

            TestProbe = testProbeCreator.Create(testKit);
            TestProbe.SetAutoPilot(new DelegateAutoPilot((_, message) =>
            {
                Type messageType = message.GetType();
                if (handlers.TryGetValue(messageType, out Func<object, object> handler))
                {
                    object reply = handler(message);
                    Context.Sender.Tell(reply);
                }
                return AutoPilot.KeepRunning;
            }));
            ReceiveAny(o => TestProbe.Forward(o));
            Ref = Self;
        }

        public TestProbe TestProbe { get; }

        public IActorRef Ref { get; }

        public IEnumerable<Exception> UnhandledExceptions => _unhandledExceptions;

        protected override SupervisorStrategy SupervisorStrategy() =>
                new AllForOneStrategy(exception =>
                {
                    _unhandledExceptions.Add(exception);
                    _exceptionWaiter.ResolveEvent();
                    return _decider(exception);
                });
    }
}