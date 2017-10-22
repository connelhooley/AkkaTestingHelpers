using System;
using System.Collections.Immutable;
using Akka.Actor;
using Akka.TestKit;

namespace ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract
{
    internal interface IConcreteDependencyResolverAdder
    {
        void Add(
            IDependencyResolverAdder dependencyResolverAdder,
            IChildWaiter childWaiter, 
            TestKitBase testKit,
            ImmutableDictionary<Type, Func<ActorBase>> factories);
    }
}