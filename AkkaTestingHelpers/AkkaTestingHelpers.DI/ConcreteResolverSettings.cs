using System;
using System.Collections.Immutable;
using Akka.Actor;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;

namespace ConnelHooley.AkkaTestingHelpers.DI
{
    public sealed class ConcreteResolverSettings
    {
        internal readonly ImmutableDictionary<Type, Func<ActorBase>> Factories;
        
        internal ConcreteResolverSettings(ImmutableDictionary<Type, Func<ActorBase>> factories) => Factories = factories;
        
        /// <summary>
        /// Registers a resolver that resolves children using the registered factories.
        /// </summary>
        /// <param name="testKit">The testkit used to add the resolver to</param>
        public void RegisterResolver(TestKitBase testKit) =>
            new ConcreteDependencyResolverAdderCreator()
                    .Create()
                    .Add(testKit, Factories);

        /// <summary>
        /// Creates an instance of ConcreteResolverSettings with no factories registered.
        /// </summary>
        public static ConcreteResolverSettings Empty => 
            new ConcreteResolverSettings(
                ImmutableDictionary<Type, Func<ActorBase>>.Empty);

        /// <summary>
        /// Creates a new instance of ConcreteResolverSettings with the given actor type registered. The actor will be constructed using the given factory.
        /// </summary>
        /// <typeparam name="T">The type of actor to register</typeparam>
        /// <param name="factory">A method the creates the actor type</param>
        /// <returns>A new instance of ConcreteResolverSettings with the actor type registered</returns>
        public ConcreteResolverSettings RegisterActor<T>(Func<T> factory) where T : ActorBase => 
            new ConcreteResolverSettings(
                Factories.SetItem(typeof(T), 
                factory));

        /// <summary>
        /// Creates a new instance of ConcreteResolverSettings with the given actor type registered. The actor will be constructed using its default constructor.
        /// </summary>
        /// <typeparam name="T">The type of actor to register</typeparam>
        /// <returns>A new instance of ConcreteResolverSettings with the actor type registered</returns>
        public ConcreteResolverSettings RegisterActor<T>() where T : ActorBase, new() =>
            new ConcreteResolverSettings(
                Factories.SetItem(typeof(T), 
                () => new T()));
    }
}