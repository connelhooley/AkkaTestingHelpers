using System;
using Akka.Actor;
using Akka.TestKit;

namespace ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract
{
    internal interface IResolvedTestProbeStore
    {
        void ResolveProbe(ActorPath actorPath, Type actorType, TestProbe testProbe);

        TestProbe FindResolvedTestProbe(IActorRef parentActor, string childName);

        Type FindResolvedType(IActorRef parentActor, string childName);
    }
}