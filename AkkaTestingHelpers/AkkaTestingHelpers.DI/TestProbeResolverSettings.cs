using System;
using System.Collections.Immutable;
using Akka.Actor;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;

namespace ConnelHooley.AkkaTestingHelpers.DI
{
    public sealed class TestProbeResolverSettings
    {
        internal readonly ImmutableDictionary<(Type, Type), Func<object, object>> Handlers;

        private TestProbeResolverSettings(ImmutableDictionary<(Type, Type), Func<object, object>> handlers)
        {
            Handlers = handlers;
        }
        
        /// <summary>
        /// Creates a resolver instance
        /// </summary>
        /// <typeparam name="TActor">The type of actor to be created</typeparam>
        /// <param name="testKit">TestKit to add resolver to</param>
        /// <param name="numberOfChildren">Number of children you expect to be created by the SUT actor</param>
        /// <returns></returns>
        public TestProbeResolver<TActor> CreateResolver<TActor>(TestKitBase testKit, int numberOfChildren) where TActor : ActorBase, new() => 
            new TestProbeResolver<TActor>(
                new SutCreator(), 
                new ChildTeller(), 
                new ChildWaiter(), 
                new DependencyResolverAdder(), 
                new TestProbeDependencyResolverAdder(),
                new TestProbeCreator(),
                new ResolvedTestProbeStore(),
                new TestProbeActorCreator(), 
                new TestProbeHandlersMapper(), 
                Handlers,
                testKit,
                Props.Create<TActor>(),
                numberOfChildren);
        
        /// <summary>
        /// Creates an instance with no handlers
        /// </summary>
        public static TestProbeResolverSettings Empty =>
            new TestProbeResolverSettings(ImmutableDictionary<(Type, Type), Func<object, object>>.Empty);

        /// <summary>
        /// Registers a new handler in the settings
        /// </summary>
        /// <typeparam name="TActor">The type of actor that will run the handler</typeparam>
        /// <typeparam name="TMessage">The type of message that will trigger the handler</typeparam>
        /// <param name="messageHandler">The method that will be ran when when the TActor receives TMessage, the returned value from this method is sent back to the sending actor</param>
        /// <returns>Settings with the additional handler</returns>
        public TestProbeResolverSettings RegisterHandler<TActor, TMessage>(Func<TMessage, object> messageHandler) where TActor : ActorBase => 
            new TestProbeResolverSettings(Handlers.SetItem(
                (typeof(TActor), typeof(TMessage)),
                o => messageHandler((TMessage)o)));
    }
}