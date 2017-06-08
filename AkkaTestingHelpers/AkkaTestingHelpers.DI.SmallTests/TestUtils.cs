using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Akka.Actor;
using Ploeh.AutoFixture;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests
{
    internal static class TestUtils
    {
        private static readonly Fixture Fixture;
        private static readonly Random Random;

        static TestUtils()
        {
            Fixture = new Fixture();
            // ActorPath
            Fixture.Register(() => ActorPath.Parse($"akka://user/{Guid.NewGuid()}"));
            
            // Unmapped handlers
            Fixture.Register(() => Fixture
                .Create<Dictionary<(Type, Type), Func<object, object>>>()
                .ToImmutableDictionary());

            //Mapped handlers
            Fixture.Register(() => Fixture
                .Create<Dictionary<Type, Dictionary<Type, Func<object, object>>>>()
                .Select(pair => new KeyValuePair<Type, ImmutableDictionary<Type, Func<object, object>>>(
                    pair.Key, 
                    pair.Value.ToImmutableDictionary()))
                .ToImmutableDictionary());
            Random = new Random();

            //Just actor handlers
            Fixture.Register(() => Fixture
                .Create<Dictionary<Type, Func<object, object>>>()
                .ToImmutableDictionary());

            Random = new Random();
        }

        public static T Create<T>() => Fixture.Create<T>();

        public static List<T> CreateMany<T>() => Fixture.CreateMany<T>().ToList();

        public static List<T> CreateMany<T>(int count) => Fixture.CreateMany<T>(count).ToList();

        public static int RandomBetween(int min, int max) => Random.Next(min, max + 1);

        public static T RandomlyPickItem<T>(this List<T> items) => items.ElementAt(Random.Next(0, items.Count));

        public static T RandomlyTakeItem<T>(this List<T> items)
        {
            int randomIndex = Random.Next(0, items.Count);
            T item = items.ElementAt(randomIndex);
            items.RemoveAt(randomIndex);
            return item;
        }

        public static Func<Type> RandomTypeGenerator()
        {
            List<Type> types = Assembly
                .GetAssembly(typeof(Type))
                .GetExportedTypes()
                .ToList();
            return () => types.RandomlyPickItem();
        }
    }
}