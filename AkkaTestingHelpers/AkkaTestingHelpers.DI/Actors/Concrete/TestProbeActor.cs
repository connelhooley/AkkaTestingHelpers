﻿using System;
using System.Collections.Generic;
using Akka.Actor;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.DI.Actors.Abstract;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract;

namespace ConnelHooley.AkkaTestingHelpers.DI.Actors.Concrete
{
    internal sealed class TestProbeActor : ReceiveActor, ITestProbeActor
    {
        public TestProbeActor(ITestProbeCreator testProbeCreator, TestKitBase testKit, IReadOnlyDictionary<Type, Func<object, object>> handlers = null)
        {
            ActorPath = Context.Self.Path;
            TestProbe = testProbeCreator.Create(testKit);
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

        public ActorBase Actor => this;
    }
}