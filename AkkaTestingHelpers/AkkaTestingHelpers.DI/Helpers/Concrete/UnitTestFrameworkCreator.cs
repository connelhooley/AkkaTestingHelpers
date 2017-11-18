using System;
using System.Collections.Immutable;
using Akka.Actor;
using Akka.TestKit;

namespace ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete
{
    public class UnitTestFrameworkCreator
    {
        public UnitTestFramework<TActor> Create<TActor>(ImmutableDictionary<(Type, Type), Func<object, object>> handlers, TestKitBase testKit, Props props, int expectedChildrenCount) where TActor : ActorBase => 
            new UnitTestFramework<TActor>(
                new SutCreator(), 
                new TellChildWaiter(), 
                new ChildWaiter(), 
                new DependencyResolverAdder(), 
                new TestProbeDependencyResolverAdder(), 
                new TestProbeCreator(), 
                new ResolvedTestProbeStore(), 
                new TestProbeActorCreator(), 
                new TestProbeHandlersMapper(),
                new SutSupervisorStrategyGetter(), 
                handlers,
                testKit,
                props,
                expectedChildrenCount);
    }
}