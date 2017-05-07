using System;
using System.Collections.Immutable;
using Akka.Actor;

namespace ConnelHooley.AkkaTestingHelpers.DI
{
    public class TestProbeResolverSettings
    {
        internal readonly IImmutableDictionary<Type, Func<object, object>> Handlers;

        private TestProbeResolverSettings(IImmutableDictionary<Type, Func<object, object>> handlers)
        {
            Handlers = handlers;
        }

        public static TestProbeResolverSettings Empty =>
            new TestProbeResolverSettings(ImmutableDictionary<Type, Func<object, object>>.Empty);

        public TestProbeResolverSettings RegisterHandler<T>(Func<object, object> messageHandler) where T : ActorBase =>
            new TestProbeResolverSettings(Handlers.SetItem(typeof(T), messageHandler));
    }
}