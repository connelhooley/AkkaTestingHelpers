using System;
using System.Diagnostics.CodeAnalysis;
using Akka.Actor;
using Akka.DI.Core;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract;

namespace ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete
{
    internal sealed class DependencyResolverAdder : IDependencyResolverAdder
    {
        public void Add(TestKitBase testKit, Func<Type, ActorBase> actorFactory)
        {
            testKit.Sys.AddDependencyResolver(new DependencyResolver(actorFactory));
        }

        private class DependencyResolver : IDependencyResolver
        {
            private readonly Func<Type, ActorBase> _actorFactory;

            public DependencyResolver(Func<Type, ActorBase> actorFactory)
            {
                _actorFactory = actorFactory;
            }

            Func<ActorBase> IDependencyResolver.CreateActorFactory(Type actorType) =>
                () => _actorFactory(actorType);

            [ExcludeFromCodeCoverage]
            Type IDependencyResolver.GetType(string actorName) =>
                throw new NotImplementedException();

            [ExcludeFromCodeCoverage]
            Props IDependencyResolver.Create<TActor>() =>
                throw new NotImplementedException();

            [ExcludeFromCodeCoverage]
            Props IDependencyResolver.Create(Type actorType) =>
                throw new NotImplementedException();

            [ExcludeFromCodeCoverage]
            void IDependencyResolver.Release(ActorBase actor) { }
        }
    }
}