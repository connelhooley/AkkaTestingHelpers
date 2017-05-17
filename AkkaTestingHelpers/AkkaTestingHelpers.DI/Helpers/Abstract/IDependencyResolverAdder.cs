using System;
using Akka.Actor;
using Akka.TestKit;

namespace ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract
{
    internal interface IDependencyResolverAdder
    {
        void Add(TestKitBase testKit, Func<Type, ActorBase> actorFactory);
    }
}