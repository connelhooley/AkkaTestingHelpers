using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Akka.Actor;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract;
using NullGuard;

namespace ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete
{
    internal sealed class ResolvedTestProbeStore : IResolvedTestProbeStore
    {
        private readonly IDictionary<ActorPath, (Type, TestProbe, SupervisorStrategy)> _resolved;

        public ResolvedTestProbeStore() => 
            _resolved = new ConcurrentDictionary<ActorPath, (Type, TestProbe, SupervisorStrategy)>();

        public void ResolveProbe(ActorPath actorPath, Type actorType, TestProbe testProbe, [AllowNull] SupervisorStrategy supervisorStrategy) => 
            _resolved[actorPath] = (actorType, testProbe, supervisorStrategy);

        public TestProbe FindResolvedTestProbe(IActorRef parentActor, string childName) => 
            FindResolved(parentActor, childName).ResolvedTestProbe;

        public Type FindResolvedType(IActorRef parentActor, string childName) =>
            FindResolved(parentActor, childName).ResolvedType;
        
        [return: AllowNull]
        public SupervisorStrategy FindResolvedSupervisorStrategy(IActorRef parentActor, string childName) =>
            FindResolved(parentActor, childName).ResolvedSupervisorStrategy;

        private (Type ResolvedType, TestProbe ResolvedTestProbe, SupervisorStrategy ResolvedSupervisorStrategy) FindResolved(IActorRef parentActor, string childName)
        {
            ActorPath childPath = parentActor.Path.Child(childName);
            if (_resolved.TryGetValue(childPath, out (Type, TestProbe, SupervisorStrategy) result))
            {
                return result;
            }
            throw new ActorNotFoundException($"No child has been resolved for the path '{childPath}'");
        }
    }
}