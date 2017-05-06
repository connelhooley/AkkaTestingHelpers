using System;
using System.Collections.Immutable;
using Akka.Actor;

namespace ConnelHooley.AkkaTestingHelpers.DI.TestProbeDependancyResolver
{
    public class Settings
    {
        internal readonly IImmutableDictionary<Type, Func<object, object>> Handlers;

        private Settings(IImmutableDictionary<Type, Func<object, object>> handlers)
        {
            Handlers = handlers;
        }

        public static Settings Empty =>
            new Settings(ImmutableDictionary<Type, Func<object, object>>.Empty);

        public Settings RegisterHandler<T>(Func<object, object> messageHandler) where T : ActorBase =>
            new Settings(Handlers.SetItem(typeof(T), messageHandler));
    }
}