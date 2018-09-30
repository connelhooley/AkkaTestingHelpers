using System;
using System.Collections.Immutable;
using Akka.Actor;
using Akka.TestKit;

namespace ConnelHooley.AkkaTestingHelpers.Helpers.Concrete
{
    public class UnitTestFrameworkCreator
    {
        public UnitTestFramework<TActor> Create<TActor>(
            ImmutableDictionary<Type, Func<object, object>> parentHandlers,
            ImmutableDictionary<(Type, Type), Func<object, object>> childHandlers,
            TestKitBase testKit,
            Props props,
            Func<Exception, Directive> decider,
            int expectedChildrenCount) where TActor : ActorBase =>
            new UnitTestFramework<TActor>(
                new SutCreator(),
                new TellChildWaiter(),
                new ChildWaiter(),
                new DependencyResolverAdder(),
                new TestProbeDependencyResolverAdder(),
                new TestProbeCreator(),
                new ResolvedTestProbeStore(),
                new TestProbeChildActorCreator(),
                new TestProbeChildHandlersMapper(),
                new SutSupervisorStrategyGetter(),
                parentHandlers,
                childHandlers,
                testKit,
                props,
                decider,
                expectedChildrenCount);
    }
}