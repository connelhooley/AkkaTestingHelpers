using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Akka.Actor;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract;

namespace ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete
{
    internal sealed class ResolvedTestProbeStore : IResolvedTestProbeStore
    {
        private readonly IDictionary<ActorPath, (Type, TestProbe)> _resolved;

        public ResolvedTestProbeStore() => 
            _resolved = new ConcurrentDictionary<ActorPath, (Type, TestProbe)>();

        public void ResolveProbe(ActorPath actorPath, Type actorType, TestProbe testProbe) => 
            _resolved[actorPath] = (actorType, testProbe);

        public TestProbe FindResolvedTestProbe(IActorRef parentActor, string childName) => 
            FindResolved(parentActor, childName).ResolvedTestProbe;

        public Type FindResolvedType(IActorRef parentActor, string childName) =>
            FindResolved(parentActor, childName).ResolvedType;
        
        private (Type ResolvedType, TestProbe ResolvedTestProbe) FindResolved(IActorRef parentActor, string childName)
        {
            ActorPath childPath = parentActor.Path.Child(childName);
            if (_resolved.TryGetValue(childPath, out (Type, TestProbe) result))
            {
                return result;
            }
            throw new ActorNotFoundException($"No child has been resolved for the path '{childPath}'");
        }
    }
}