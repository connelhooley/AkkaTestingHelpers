using System;
#if NET45
using System.Diagnostics.CodeAnalysis;
#endif
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
#if NET45
            [ExcludeFromCodeCoverage]
#endif
            Type IDependencyResolver.GetType(string actorName) =>
                throw new NotImplementedException();

#if NET45
            [ExcludeFromCodeCoverage]
#endif
            Props IDependencyResolver.Create<TActor>() =>
                throw new NotImplementedException();

#if NET45
            [ExcludeFromCodeCoverage]
#endif
            Props IDependencyResolver.Create(Type actorType) =>
                throw new NotImplementedException();

#if NET45
            [ExcludeFromCodeCoverage]
#endif
            void IDependencyResolver.Release(ActorBase actor) { }
        }
    }
}