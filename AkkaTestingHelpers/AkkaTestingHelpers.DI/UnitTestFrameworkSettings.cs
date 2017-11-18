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

        /// <summary>
        /// Creates a UnitTestFramework using the given TestKit. Does not wait for any children to be created when the SUT actor is created.
        /// </summary>
        /// <typeparam name="TActor">The type of actor that will be created as the SUT actor</typeparam>
        /// <param name="testKit">The TestKit that will be used to resolve children as TestProbes</param>
        /// <returns>A UnitTestFramework that allows you to unit test the given TActor type</returns>
        public UnitTestFramework<TActor> CreateFramework<TActor>(TestKitBase testKit) where TActor : ActorBase, new() =>
            new UnitTestFrameworkCreator()
                .Create<TActor>(
                    Handlers, 
                    testKit, 
                    Props.Create<TActor>(), 
                    0);

        /// <summary>
        /// Creates a UnitTestFramework using the given TestKit. Waits for the given number of children to be created when the SUT actor is created.
        /// </summary>
        /// <typeparam name="TActor">The type of actor that will be created as the SUT actor</typeparam>
        /// <param name="testKit">The TestKit that will be used to resolve children as TestProbes</param>
        /// <param name="expectedChildrenCount">The number of children to wait for when the SUT actor is created</param>
        /// <returns>A UnitTestFramework that allows you to unit test the given actor type</returns>
        public UnitTestFramework<TActor> CreateFramework<TActor>(TestKitBase testKit, int expectedChildrenCount) where TActor : ActorBase, new() =>
            new UnitTestFrameworkCreator()
                .Create<TActor>(
                    Handlers,
                    testKit,
                    Props.Create<TActor>(),
                    expectedChildrenCount);

        /// <summary>
        /// Creates a UnitTestFramework using the given TestKit. Creates the SUT actor using the given Props. Does not wait for any children to be created when the SUT actor is created.
        /// </summary>
        /// <typeparam name="TActor">The type of actor that will be created as the SUT actor</typeparam>
        /// <param name="testKit">The TestKit that will be used to resolve children as TestProbes</param>
        /// <param name="props">The props used to create the SUT actor</param>
        /// <returns>A UnitTestFramework that allows you to unit test the given actor type</returns>
        public UnitTestFramework<TActor> CreateFramework<TActor>(TestKitBase testKit, Props props) where TActor : ActorBase =>
            new UnitTestFrameworkCreator()
                .Create<TActor>(
                    Handlers,
                    testKit,
                    props,
                    0);

        /// <summary>
        /// Creates a UnitTestFramework using the given TestKit. Creates the SUT actor using the given Props. Waits for the given number of children to be created when the SUT actor is created.
        /// </summary>
        /// <typeparam name="TActor">The type of actor that will be created as the SUT actor</typeparam>
        /// <param name="testKit">The TestKit that will be used to resolve children as TestProbes</param>
        /// <param name="props">The props used to create the SUT actor</param>
        /// <param name="expectedChildrenCount">The number of children to wait for when the SUT actor is created</param>
        /// <returns>A UnitTestFramework that allows you to unit test the given actor type</returns>
        public UnitTestFramework<TActor> CreateFramework<TActor>(TestKitBase testKit, Props props, int expectedChildrenCount) where TActor : ActorBase =>
            new UnitTestFrameworkCreator()
                .Create<TActor>(
                    Handlers,
                    testKit,
                    props,
                    expectedChildrenCount);

        /// <summary>
        /// Creates a UnitTestFrameworkSettings instance with no message handlers registered.
        /// </summary>
        public static UnitTestFrameworkSettings Empty =>
            new UnitTestFrameworkSettings(ImmutableDictionary<(Type, Type), Func<object, object>>.Empty);

        /// <summary>
        /// Creates a UnitTestFramework with the given message handler registered.
        /// </summary>
        /// <typeparam name="TActor">The type of actor that will run the handler</typeparam>
        /// <typeparam name="TMessage">The type of message that will trigger the handler</typeparam>
        /// <param name="messageHandler">The method that will be ran when when the TActor receives TMessage, the returned value from this method is sent back to the sending actor</param>
        /// <returns>A new UnitTestFrameworkSettings instance with the additional handler configured</returns>
        public UnitTestFrameworkSettings RegisterHandler<TActor, TMessage>(Func<TMessage, object> messageHandler) where TActor : ActorBase => 
            new UnitTestFrameworkSettings(Handlers.SetItem(
                (typeof(TActor), typeof(TMessage)),
                o => messageHandler((TMessage)o)));
    }
}