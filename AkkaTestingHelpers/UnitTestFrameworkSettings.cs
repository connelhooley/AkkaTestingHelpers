using System;
using System.Collections.Immutable;
using Akka.Actor;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;

namespace ConnelHooley.AkkaTestingHelpers
{
    public sealed class UnitTestFrameworkSettings
    {
        internal readonly ImmutableDictionary<(Type, Type), Func<object, object>> ChildHandlers;
        internal readonly ImmutableDictionary<Type, Func<object, object>> ParentHandlers;

        private UnitTestFrameworkSettings(
            ImmutableDictionary<(Type, Type), Func<object, object>> childHandlers,
            ImmutableDictionary<Type, Func<object, object>> parentHandlers)
        {
            ChildHandlers = childHandlers;
            ParentHandlers = parentHandlers;
        }

        /// <summary>
        /// Creates a UnitTestFramework using the given TestKit. Does not wait for any children to be created when the SUT actor is created.
        /// </summary>
        /// <typeparam name="TActor">The type of actor that will be created as the SUT actor</typeparam>
        /// <param name="testKit">The TestKit that will be used to resolve children as TestProbes</param>
        /// <returns>A UnitTestFramework that allows you to unit test the given TActor type</returns>
        public UnitTestFramework<TActor> CreateFramework<TActor>(TestKitBase testKit) where TActor : ActorBase, new() =>
            new UnitTestFrameworkCreator()
                .Create<TActor>(
                    ParentHandlers,
                    ChildHandlers,
                    testKit,
                    Props.Create<TActor>(),
                    _ => Directive.Restart,
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
                    ParentHandlers,
                    ChildHandlers,
                    testKit,
                    Props.Create<TActor>(),
                    _ => Directive.Restart,
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
                    ParentHandlers,
                    ChildHandlers,
                    testKit,
                    props,
                    _ => Directive.Restart,
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
                    ParentHandlers,
                    ChildHandlers,
                    testKit,
                    props,
                    _ => Directive.Restart,
                    expectedChildrenCount);

        /// <summary>
        /// Creates a UnitTestFramework using the given TestKit. Does not wait for any children to be created when the SUT actor is created.
        /// </summary>
        /// <typeparam name="TActor">The type of actor that will be created as the SUT actor</typeparam>
        /// <param name="testKit">The TestKit that will be used to resolve children as TestProbes</param>
        /// <param name="decider">The decider that will be used to determine whether the parent actor should restart or stop the SUT actor when a certain type of unhandled exception is thrown</param>
        /// <returns>A UnitTestFramework that allows you to unit test the given TActor type</returns>
        public UnitTestFramework<TActor> CreateFramework<TActor>(TestKitBase testKit, Func<Exception, Directive> decider) where TActor : ActorBase, new() =>
            new UnitTestFrameworkCreator()
                .Create<TActor>(
                    ParentHandlers,
                    ChildHandlers,
                    testKit,
                    Props.Create<TActor>(),
                    decider,
                    0);

        /// <summary>
        /// Creates a UnitTestFramework using the given TestKit. Waits for the given number of children to be created when the SUT actor is created.
        /// </summary>
        /// <typeparam name="TActor">The type of actor that will be created as the SUT actor</typeparam>
        /// <param name="testKit">The TestKit that will be used to resolve children as TestProbes</param>
        /// <param name="decider">The decider that will be used to determine whether the parent actor should restart or stop the SUT actor when a certain type of unhandled exception is thrown</param>
        /// <param name="expectedChildrenCount">The number of children to wait for when the SUT actor is created</param>
        /// <returns>A UnitTestFramework that allows you to unit test the given actor type</returns>
        public UnitTestFramework<TActor> CreateFramework<TActor>(TestKitBase testKit, Func<Exception, Directive> decider, int expectedChildrenCount) where TActor : ActorBase, new() =>
            new UnitTestFrameworkCreator()
                .Create<TActor>(
                    ParentHandlers,
                    ChildHandlers,
                    testKit,
                    Props.Create<TActor>(),
                    decider,
                    expectedChildrenCount);

        /// <summary>
        /// Creates a UnitTestFramework using the given TestKit. Creates the SUT actor using the given Props. Does not wait for any children to be created when the SUT actor is created.
        /// </summary>
        /// <typeparam name="TActor">The type of actor that will be created as the SUT actor</typeparam>
        /// <param name="testKit">The TestKit that will be used to resolve children as TestProbes</param>
        /// <param name="props">The props used to create the SUT actor</param>
        /// <param name="decider">The decider that will be used to determine whether the parent actor should restart or stop the SUT actor when a certain type of unhandled exception is thrown</param>
        /// <returns>A UnitTestFramework that allows you to unit test the given actor type</returns>
        public UnitTestFramework<TActor> CreateFramework<TActor>(TestKitBase testKit, Props props, Func<Exception, Directive> decider) where TActor : ActorBase =>
            new UnitTestFrameworkCreator()
                .Create<TActor>(
                    ParentHandlers,
                    ChildHandlers,
                    testKit,
                    props,
                    decider,
                    0);

        /// <summary>
        /// Creates a UnitTestFramework using the given TestKit. Creates the SUT actor using the given Props. Waits for the given number of children to be created when the SUT actor is created.
        /// </summary>
        /// <typeparam name="TActor">The type of actor that will be created as the SUT actor</typeparam>
        /// <param name="testKit">The TestKit that will be used to resolve children as TestProbes</param>
        /// <param name="props">The props used to create the SUT actor</param>
        /// <param name="decider">The decider that will be used to determine whether the parent actor should restart or stop the SUT actor when a certain type of unhandled exception is thrown</param>
        /// <param name="expectedChildrenCount">The number of children to wait for when the SUT actor is created</param>
        /// <returns>A UnitTestFramework that allows you to unit test the given actor type</returns>
        public UnitTestFramework<TActor> CreateFramework<TActor>(TestKitBase testKit, Props props, Func<Exception, Directive> decider, int expectedChildrenCount) where TActor : ActorBase =>
            new UnitTestFrameworkCreator()
                .Create<TActor>(
                    ParentHandlers,
                    ChildHandlers,
                    testKit,
                    props,
                    decider,
                    expectedChildrenCount);

        /// <summary>
        /// Creates a UnitTestFrameworkSettings instance with no message handlers registered.
        /// </summary>
        public static UnitTestFrameworkSettings Empty =>
            new UnitTestFrameworkSettings(
                ImmutableDictionary<(Type, Type), Func<object, object>>.Empty,
                ImmutableDictionary<Type, Func<object, object>>.Empty);

        /// <summary>
        /// Creates a UnitTestFrameworkSettings instance with the given message handler registered to the specified child.
        /// </summary>
        /// <typeparam name="TChildActor">The type of actor that will run the handler</typeparam>
        /// <typeparam name="TMessage">The type of message that will trigger the handler</typeparam>
        /// <param name="messageHandler">The method that will be ran when when the TChildActor receives TMessage, the returned value from this method is sent back to the sending actor</param>
        /// <returns>A new UnitTestFrameworkSettings instance with the additional handler configured</returns>
        public UnitTestFrameworkSettings RegisterChildHandler<TChildActor, TMessage>(Func<TMessage, object> messageHandler) where TChildActor : ActorBase =>
            new UnitTestFrameworkSettings(
                ChildHandlers.SetItem(
                    (typeof(TChildActor), typeof(TMessage)),
                    o => messageHandler((TMessage)o)),
                ParentHandlers);

        /// <summary>
        /// Creates a UnitTestFrameworkSettings instance with the given message handler registered to the parent actor.
        /// </summary>
        /// <typeparam name="TMessage">The type of message that will trigger the handler</typeparam>
        /// <param name="messageHandler">The method that will be ran when when the parent receives TMessage, the returned value from this method is sent back to the sending actor</param>
        /// <returns>A new UnitTestFrameworkSettings instance with the additional handler configured</returns>
        public UnitTestFrameworkSettings RegisterParentHandler<TMessage>(Func<TMessage, object> messageHandler) =>
            new UnitTestFrameworkSettings(
                ChildHandlers,
                ParentHandlers.SetItem(
                    typeof(TMessage),
                    o => messageHandler((TMessage)o)));
    }
}