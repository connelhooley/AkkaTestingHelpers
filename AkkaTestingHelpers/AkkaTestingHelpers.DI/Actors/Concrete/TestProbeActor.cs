using System;
using System.Collections.Generic;
using Akka.Actor;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.DI.Actors.Abstract;
namespace ConnelHooley.AkkaTestingHelpers.DI.Actors.Concrete
{
    internal sealed class TestProbeActor : ReceiveActor, ITestProbeActor
    {
        public TestProbeActor(TestKitBase testKit)
        {
            ActorPath = Context.Self.Path;
            TestProbe = testKit.CreateTestProbe();
            ReceiveAny(o => TestProbe.Forward(o));
        }

        public ActorPath ActorPath { get; }

        public TestProbe TestProbe { get; }

        public ActorBase Actor => this;

        public void SetHandlers(IReadOnlyDictionary<Type, Func<object, object>> handlers) =>
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