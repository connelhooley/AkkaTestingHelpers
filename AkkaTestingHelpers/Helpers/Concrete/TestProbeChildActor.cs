using System;
using System.Collections.Generic;
using Akka.Actor;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.Helpers.Abstract;
using NullGuard;

namespace ConnelHooley.AkkaTestingHelpers.Helpers.Concrete
{
    internal sealed class TestProbeChildActor : ReceiveActor, ITestProbeChildActor
    {
        public TestProbeChildActor(ITestProbeCreator testProbeCreator, TestKitBase testKit, IReadOnlyDictionary<Type, Func<object, object>> handlers = null)
        {
            ActorPath = Context.Self.Path;
            TestProbe = testProbeCreator.Create(testKit);
            PropsSupervisorStrategy = Context.Props.SupervisorStrategy;
            ReceiveAny(o => TestProbe.Forward(o));
            if (handlers != null)
            {
                TestProbe.SetAutoPilot(new DelegateAutoPilot((sender, message) =>
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
        }

        public ActorPath ActorPath { get; }

        public TestProbe TestProbe { get; }

        [AllowNull]
        public SupervisorStrategy PropsSupervisorStrategy { get; }

        public ActorBase Actor => this;
    }
}