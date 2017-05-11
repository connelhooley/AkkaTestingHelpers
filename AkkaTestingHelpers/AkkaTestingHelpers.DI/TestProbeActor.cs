using System;
using System.Collections.Generic;
using Akka.Actor;
using Akka.TestKit;

namespace ConnelHooley.AkkaTestingHelpers.DI
{
    internal class TestProbeActor : ReceiveActor
    {
        public ActorPath ActorPath { get; }

        public TestProbe TestProbe { get; }

        public TestProbeActor(TestKitBase testKit, IReadOnlyDictionary<Type, Func<object, object>> handlers = null)
        {
            ActorPath = Context.Self.Path;
            TestProbe = testKit.CreateTestProbe();
            if (handlers != null)
            {
                TestProbe.SetAutoPilot(new DelegateAutoPilot((sender, message) =>
                {
                    Type messageType = message.GetType();
                    if (handlers.ContainsKey(messageType))
                    {
                        Func<object, object> handler = handlers[messageType];
                        object reply = handler(message);
                        Context.Sender.Tell(reply);
                    }
                    return AutoPilot.KeepRunning;
                }));
            }
            ReceiveAny(o => TestProbe.Forward(o));
        }
    }
}