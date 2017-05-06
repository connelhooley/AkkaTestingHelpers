using System;
using System.Collections.Immutable;
using Akka.Actor;
using Akka.TestKit;

namespace ConnelHooley.AkkaTestingHelpers.DI.DependancyResolver
{
    public class Settings
    {
        internal readonly IImmutableDictionary<Type, Func<ActorBase>> Factories;
        
        private Settings(IImmutableDictionary<Type, Func<ActorBase>> factories)
        {
            Factories = factories;
        }

        public Resolver CreateResolver(TestKitBase testKit) => new Resolver(testKit, this);

        public static Settings Empty => 
            new Settings(ImmutableDictionary<Type, Func<ActorBase>>.Empty);

        public Settings Register<T>(Func<T> factory) where T : ActorBase => 
            new Settings(Factories.SetItem(typeof(T), factory));
        
        public Settings Register<T>() where T : ActorBase, new() =>
            new Settings(Factories.SetItem(typeof(T), () => new T()));
    }
}