using System;
using System.Collections.Immutable;
using Akka.Actor;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;

namespace ConnelHooley.AkkaTestingHelpers.DI
{
    public class TestProbeResolverSettings
    {
        internal readonly ImmutableDictionary<(Type, Type), Func<object, object>> Handlers;

        private TestProbeResolverSettings(ImmutableDictionary<(Type, Type), Func<object, object>> handlers)
        {
            Handlers = handlers;
        }

        public TestProbeResolver CreateResolver(TestKitBase testKit) => 
            new TestProbeResolver(
                new DependencyResolverAdder(), 
                new SutCreator(), 
                new ChildTeller(), 
                new ChildWaiter(), 
                new ResolvedTestProbeStore(),
                new TestProbeCreator(),
                new TestProbeActorFactory(), 
                new TestProbeHandlersMapper(), 
                testKit,
                this);

        public static TestProbeResolverSettings Empty =>
            new TestProbeResolverSettings(ImmutableDictionary<(Type, Type), Func<object, object>>.Empty);

        public TestProbeResolverSettings RegisterHandler<TActor, TMessage>(Func<TMessage, object> messageHandler) where TActor : ActorBase => 
            new TestProbeResolverSettings(Handlers.SetItem(
                (typeof(TActor), typeof(TMessage)),
                o => messageHandler((TMessage)o)));
    }
}