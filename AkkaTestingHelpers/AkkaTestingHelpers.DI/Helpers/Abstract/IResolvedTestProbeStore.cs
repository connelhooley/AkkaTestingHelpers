using System;
using Akka.Actor;
using Akka.TestKit;

namespace ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract
{
    internal interface IResolvedTestProbeStore
    {
        void ResolveProbe(ActorPath actorPath, Type actorType, TestProbe testProbe, SupervisorStrategy supervisorStrategy);

        TestProbe FindResolvedTestProbe(IActorRef parentActor, string childName);

        Type FindResolvedType(IActorRef parentActor, string childName);

        SupervisorStrategy FindResolvedSupervisorStrategy(IActorRef parentActor, string childName);
    }
}