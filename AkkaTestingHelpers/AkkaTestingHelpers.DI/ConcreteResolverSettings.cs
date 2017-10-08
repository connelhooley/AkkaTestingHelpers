using System;
using System.Collections.Immutable;
using Akka.Actor;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;
using EitherFactory = Akka.Util.Either<System.Func<Akka.Actor.ActorBase>, ConnelHooley.AkkaTestingHelpers.DI.IRegisterableActorFake>;
using ConcreteFactory = Akka.Util.Left<System.Func<Akka.Actor.ActorBase>, ConnelHooley.AkkaTestingHelpers.DI.IRegisterableActorFake>;
using ActorFakeFactory = Akka.Util.Right<System.Func<Akka.Actor.ActorBase>, ConnelHooley.AkkaTestingHelpers.DI.IRegisterableActorFake>;

namespace ConnelHooley.AkkaTestingHelpers.DI
{
    public sealed class ConcreteResolverSettings
    {
        internal readonly ImmutableDictionary<Type, EitherFactory> Factories;
        
        internal ConcreteResolverSettings(ImmutableDictionary<Type, EitherFactory> factories)
        {
            Factories = factories;
        }

        public ConcreteResolver CreateResolver(TestKitBase testKit) => 
            new ConcreteResolver(
                new DependencyResolverAdder(), 
                new SutCreator(), 
                new ChildTeller(), 
                new ChildWaiter(), 
                new TestProbeActorCreator(),
                testKit, 
                Factories);

        public static ConcreteResolverSettings Empty => 
            new ConcreteResolverSettings(
                ImmutableDictionary<Type, EitherFactory>.Empty);

        public ConcreteResolverSettings Register<T>(Func<T> factory) where T : ActorBase => 
            new ConcreteResolverSettings(
                Factories.SetItem(typeof(T), 
                new ConcreteFactory(factory)));

        public ConcreteResolverSettings Register<T>() where T : ActorBase, new() =>
            new ConcreteResolverSettings(
                Factories.SetItem(typeof(T), 
                new ConcreteFactory(() => new T())));
        
        public ConcreteResolverSettings RegisterFake<T>(ActorFake actor) where T : ActorBase =>
            new ConcreteResolverSettings(
                Factories.SetItem(typeof(T), 
                new ActorFakeFactory(actor)));
    }
}