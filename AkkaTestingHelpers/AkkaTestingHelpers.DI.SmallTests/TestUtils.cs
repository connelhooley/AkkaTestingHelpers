using System;
using System.Collections.Generic;
using System.Linq;
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
            Random = new Random();
        }

        public static T Create<T>() => Fixture.Create<T>();

        public static List<T> CreateMany<T>() => Fixture.CreateMany<T>().ToList();

        public static List<T> CreateMany<T>(int count) => Fixture.CreateMany<T>(count).ToList();

        public static int RandomBetween(int min, int max) => Random.Next(min, max + 1);
    }
}