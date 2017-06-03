using System;
using System.Collections.Immutable;
using Akka.Actor;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;

namespace ConnelHooley.AkkaTestingHelpers.DI
{
    public class ConcreteResolverSettings
    {
        internal readonly IImmutableDictionary<Type, Func<ActorBase>> Factories;
        
        private ConcreteResolverSettings(IImmutableDictionary<Type, Func<ActorBase>> factories)
        {
            Factories = factories;
        }

        public ConcreteResolver CreateResolver(TestKitBase testKit) => 
            new ConcreteResolver(
                new DependencyResolverAdder(), 
                new SutCreator(), 
                new ChildTeller(), 
                new ChildWaiter(), 
                testKit, 
                this);

        public static ConcreteResolverSettings Empty => 
            new ConcreteResolverSettings(
                ImmutableDictionary<Type, Func<ActorBase>>.Empty);

        public ConcreteResolverSettings Register<T>(Func<T> factory) where T : ActorBase => 
            new ConcreteResolverSettings(
                Factories.SetItem(typeof(T), factory));
        
        public ConcreteResolverSettings Register<T>() where T : ActorBase, new() =>
            new ConcreteResolverSettings(
                Factories.SetItem(typeof(T), () => new T()));
    }
}