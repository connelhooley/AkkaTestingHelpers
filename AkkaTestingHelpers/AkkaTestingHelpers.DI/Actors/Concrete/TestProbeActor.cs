using System;
using System.Collections.Generic;
using Akka.Actor;
using Akka.TestKit;
using Akka.Util;
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
                if (handlers.ContainsKey(messageType))
                {
                    Func<object, object> handler = handlers[messageType];
                    object reply = handler(message);
                    Context.Sender.Tell(reply);
                }
                return AutoPilot.KeepRunning;
            }));

        //todo test
        public void SetHandlers(IReadOnlyDictionary<Type, Either<Action<object>, Func<object, object>>> handlers) => 
            TestProbe.SetAutoPilot(new DelegateAutoPilot((sender, message) =>
            {
                Type messageType = message.GetType();
                if (handlers.ContainsKey(messageType))
                {
                    Action<object> handler = handlers[messageType].Fold(
                        nonReplyingHandler => nonReplyingHandler,
                        replyingHandler => m =>
                        {
                            object reply = replyingHandler(m);
                            Context.Sender.Tell(reply);
                        });
                    handler(message);
                }
                return AutoPilot.KeepRunning;
            }));
    }
}