using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Akka.Actor;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract;

namespace ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete
{
    public class TestProbeActorFactory : ITestProbeActorFactory
    {
        public (ActorBase, ActorPath, TestProbe) Create(TestKitBase testKit, Type actorType, ImmutableDictionary<Type, ImmutableDictionary<Type, Func<object, object>>> handlers)
        {
            TestProbeActor actor = handlers.ContainsKey(actorType)
                ? new TestProbeActor(testKit, handlers[actorType])
                : new TestProbeActor(testKit);
            return (actor, actor.ActorPath, actor.TestProbe);
        }

        private class TestProbeActor : ReceiveActor
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
}