using System;
using Akka.Actor;
using Akka.DI.Core;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract;

namespace ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete
{
    internal class DependencyResolverAdder : IDependencyResolverAdder
    {
        public void Add(TestKitBase testKit, Func<Type, ActorBase> actorFactory)
        {
            testKit.Sys.AddDependencyResolver(new DependencyResolver(actorFactory));
        }
    }
}