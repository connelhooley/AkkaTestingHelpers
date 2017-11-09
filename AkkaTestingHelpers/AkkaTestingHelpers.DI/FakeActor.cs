using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Akka.Actor;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;

namespace ConnelHooley.AkkaTestingHelpers.DI
{
    public class FakeActor
    {
        private readonly ITestProbeCreator _testProbeCreator;
        private readonly TestKitBase _testKit;
        private readonly IReadOnlyDictionary<Type, Func<object, object>> _handlers;
        private readonly ConcurrentDictionary<ActorPath, int> _numberOfStarts;
        private readonly ConcurrentDictionary<ActorPath, TestProbe> _testProbes;

        internal FakeActor(ITestProbeCreator testProbeCreator, TestKitBase testKit, IReadOnlyDictionary<Type, Func<object, object>> handlers = null)
        {
            _testProbeCreator = testProbeCreator;
            _testKit = testKit;
            _handlers = handlers;
            _numberOfStarts = new ConcurrentDictionary<ActorPath, int>();
            _testProbes = new ConcurrentDictionary<ActorPath, TestProbe>();
        }

        public TestProbe GetTestProbeProbe(ActorPath actorPath) => _testProbes[actorPath];

        public int GetNumberOfStarts(ActorPath actorPath) => _numberOfStarts[actorPath];

        internal ActorBase CreateActor() => 
            new FakeActorInstance((path, testProbe) =>
            {
                _numberOfStarts[path] = _numberOfStarts[path] + 1;
                _testProbes[path] = testProbe;
            }, _testProbeCreator, _testKit, _handlers);

        private class FakeActorInstance : ReceiveActor
        {
            private readonly Action<ActorPath, TestProbe> _onStart;
            private readonly TestProbe _testProbe;

            public FakeActorInstance(Action<ActorPath, TestProbe> onStart, ITestProbeCreator testProbeCreator, TestKitBase testKit, IReadOnlyDictionary<Type, Func<object, object>> handlers = null)
            {
                _onStart = onStart;
                _testProbe = testProbeCreator.Create(testKit);
                if (handlers != null)
                {
                    _testProbe.SetAutoPilot(new DelegateAutoPilot((sender, message) =>
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
                ReceiveAny(o => _testProbe.Forward(o));
            }

            protected override void PostRestart(Exception reason)
            {

                base.PostRestart(reason);
            }
        }
    }

    public class FakeActorSettings
    {
        public FakeActor Create(TestKitBase testKit, IReadOnlyDictionary<Type, Func<object, object>> handlers) => new FakeActor(new TestProbeCreator(), testKit, handlers);
    }
}