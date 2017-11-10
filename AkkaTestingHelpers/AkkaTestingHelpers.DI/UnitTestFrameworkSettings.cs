using System;
using System.Collections.Immutable;
using Akka.Actor;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;

namespace ConnelHooley.AkkaTestingHelpers.DI
{
    public sealed class UnitTestFrameworkSettings
    {
        internal readonly ImmutableDictionary<(Type, Type), Func<object, object>> Handlers;

        private UnitTestFrameworkSettings(ImmutableDictionary<(Type, Type), Func<object, object>> handlers) => 
            Handlers = handlers;
        
        public UnitTestFramework<TActor> CreateFramework<TActor>(TestKitBase testKit) where TActor : ActorBase, new() =>
            new UnitTestFrameworkCreator()
                .Create<TActor>(
                    ImmutableDictionary<(Type, Type), Func<object, object>>.Empty, 
                    testKit, 
                    Props.Create<TActor>(), 
                    0);

        public UnitTestFramework<TActor> CreateFramework<TActor>(TestKitBase testKit, int expectedChildrenCount) where TActor : ActorBase, new() =>
            new UnitTestFrameworkCreator()
                .Create<TActor>(
                    ImmutableDictionary<(Type, Type), Func<object, object>>.Empty,
                    testKit,
                    Props.Create<TActor>(),
                    expectedChildrenCount);

        public UnitTestFramework<TActor> CreateFramework<TActor>(TestKitBase testKit, ImmutableDictionary<(Type, Type), Func<object, object>> handlers, int expectedChildrenCount) where TActor : ActorBase, new() =>
            new UnitTestFrameworkCreator()
                .Create<TActor>(
                    handlers,
                    testKit,
                    Props.Create<TActor>(),
                    expectedChildrenCount);
        
        public UnitTestFramework<TActor> CreateFramework<TActor>(TestKitBase testKit, Props props) where TActor : ActorBase =>
            new UnitTestFrameworkCreator()
                .Create<TActor>(
                    ImmutableDictionary<(Type, Type), Func<object, object>>.Empty,
                    testKit,
                    props,
                    0);

        public UnitTestFramework<TActor> CreateFramework<TActor>(TestKitBase testKit, Props props, int expectedChildrenCount) where TActor : ActorBase =>
            new UnitTestFrameworkCreator()
                .Create<TActor>(
                    ImmutableDictionary<(Type, Type), Func<object, object>>.Empty,
                    testKit,
                    props,
                    expectedChildrenCount);

        public UnitTestFramework<TActor> CreateFramework<TActor>(TestKitBase testKit, ImmutableDictionary<(Type, Type), Func<object, object>> handlers, Props props, int expectedChildrenCount) where TActor : ActorBase =>
            new UnitTestFrameworkCreator()
                .Create<TActor>(
                    handlers,
                    testKit,
                    props,
                    expectedChildrenCount);
        
        /// <summary>
        /// Creates a UnitTestFramework instance with no handlers configured
        /// </summary>
        public static UnitTestFrameworkSettings Empty =>
            new UnitTestFrameworkSettings(ImmutableDictionary<(Type, Type), Func<object, object>>.Empty);

        /// <summary>
        /// Registers a new message handler
        /// </summary>
        /// <typeparam name="TActor">The type of actor that will run the handler</typeparam>
        /// <typeparam name="TMessage">The type of message that will trigger the handler</typeparam>
        /// <param name="messageHandler">The method that will be ran when when the TActor receives TMessage, the returned value from this method is sent back to the sending actor</param>
        /// <returns>A new UnitTestFramework instance with the additional handler configured</returns>
        public UnitTestFrameworkSettings RegisterHandler<TActor, TMessage>(Func<TMessage, object> messageHandler) where TActor : ActorBase => 
            new UnitTestFrameworkSettings(Handlers.SetItem(
                (typeof(TActor), typeof(TMessage)),
                o => messageHandler((TMessage)o)));
    }
}