using Akka.Actor;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.Helpers.Abstract;
using System;
using System.Collections.Generic;

namespace ConnelHooley.AkkaTestingHelpers.Helpers.Concrete
{
    internal sealed class TestProbeParentActor : ReceiveActor, ITestProbeParentActor
    {
        private readonly Directive _directive;
        private readonly List<Exception> _unhandledExceptions;

        public TestProbeParentActor(ITestProbeCreator testProbeCreator, TestKitBase testKit, Directive directive, IReadOnlyDictionary<Type, Func<object, object>> handlers)
        {
            _directive = directive;
            _unhandledExceptions = new List<Exception>();

            TestProbe = testProbeCreator.Create(testKit);
            ReceiveAny(o => TestProbe.Forward(o));

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
        }

        public TestProbe TestProbe { get; }

        public IEnumerable<Exception> UnhandledExceptions => _unhandledExceptions;

        public IActorRef Ref => TestProbe.Ref;

        protected override SupervisorStrategy SupervisorStrategy() =>
                new AllForOneStrategy(exception =>
                {
                    _unhandledExceptions.Add(exception);
                    return _directive;
                });
    }
}