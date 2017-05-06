using System;
using System.Collections.Immutable;
using Akka.Actor;

namespace ConnelHooley.AkkaTestingHelpers.DI.DependancyResolver
{
    public class Settings
    {
        internal readonly IImmutableDictionary<Type, Func<ActorBase>> Factories;
        
        private Settings(IImmutableDictionary<Type, Func<ActorBase>> factories)
        {
            Factories = factories;
        }

        public static Settings Empty => 
            new Settings(ImmutableDictionary<Type, Func<ActorBase>>.Empty);

        public Settings Register<T>(Func<T> factory) where T : ActorBase => 
            new Settings(Factories.SetItem(typeof(T), factory));
    }
}