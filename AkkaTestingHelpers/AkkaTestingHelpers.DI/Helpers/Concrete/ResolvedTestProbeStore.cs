using System;
using Akka.Actor;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract;

namespace ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete
{
    internal class ResolvedTestProbeStore : IResolvedTestProbeStore
    {
        public void ResolveProbe(ActorPath actorPath, Type actorType, TestProbe testProbe)
        {

        }

        public TestProbe FindResolvedTestProbe(IActorRef parentActor, string childName)
        {
            throw new NotImplementedException();
        }

        public Type FindResolvedType(IActorRef parentActor, string childName)
        {
            throw new NotImplementedException();
        }
    }
}