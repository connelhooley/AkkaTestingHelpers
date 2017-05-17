using System;
using Akka.Actor;
using Akka.DI.Core;

namespace ConnelHooley.AkkaTestingHelpers.DI
{
    internal class DependencyResolver : IDependencyResolver
    {
        private readonly Func<Type, ActorBase> _actorFactory;

        public DependencyResolver(Func<Type, ActorBase> actorFactory)
        {
            _actorFactory = actorFactory;
        }

        Func<ActorBase> IDependencyResolver.CreateActorFactory(Type actorType) =>
            () => _actorFactory(actorType);

        Type IDependencyResolver.GetType(string actorName) =>
            throw new NotImplementedException();

        Props IDependencyResolver.Create<TActor>() =>
            throw new NotImplementedException();

        Props IDependencyResolver.Create(Type actorType) =>
            throw new NotImplementedException();

        void IDependencyResolver.Release(ActorBase actor)
        {
            //No implementation needed
        }
    }
}