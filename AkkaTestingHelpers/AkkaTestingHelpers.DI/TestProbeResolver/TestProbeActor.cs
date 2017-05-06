using System;
using Akka.Actor;
using Akka.TestKit;

namespace ConnelHooley.AkkaTestingHelpers.DI.TestProbeResolver
{
    internal class TestProbeActor : ReceiveActor
    {
        public ActorPath ActorPath { get; }

        public TestProbe TestProbe { get; }

        public TestProbeActor(TestKitBase testKit, Func<object, object> reply = null)
        {
            ActorPath = Context.Self.Path;
            TestProbe = testKit.CreateTestProbe();
            if (reply != null)
            {
                TestProbe.SetAutoPilot(new DelegateAutoPilot((sender, message) =>
                {
                    Context.Sender.Tell(reply(message));
                    return AutoPilot.KeepRunning;
                }));
            }
            ReceiveAny(o => TestProbe.Forward(o));
        }
    }
}