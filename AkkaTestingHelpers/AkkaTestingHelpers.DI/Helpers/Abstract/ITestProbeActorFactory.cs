using System;
using System.Collections.Immutable;
using Akka.Actor;
using Akka.TestKit;

namespace ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract
{
    public interface ITestProbeActorFactory
    {
        (ActorBase, ActorPath, TestProbe) Create(TestKitBase testKit, Type actorType, ImmutableDictionary<Type, ImmutableDictionary<Type, Func<object, object>>> handlers);
    }
}