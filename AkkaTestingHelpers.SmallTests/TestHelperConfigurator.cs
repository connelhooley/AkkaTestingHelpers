using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using ConnelHooley.TestHelpers.Abstractions;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests
{
    public class TestHelperConfigurator : ITestHelperConfigurator
    {
        public void Configure(ITestHelperContext x)
        {
            // Unmapped handlers
            x.Register(() => x
                .Generate<Dictionary<(Type, Type), Func<object, object>>>()
                .ToImmutableDictionary());

            //Mapped handlers
            x.Register(() => x
                .Generate<Dictionary<Type, Dictionary<Type, Func<object, object>>>>()
                .Select(pair => new KeyValuePair<Type, ImmutableDictionary<Type, Func<object, object>>>(
                    pair.Key,
                    pair.Value.ToImmutableDictionary()))
                .ToImmutableDictionary());

            //Just actor handlers
            x.Register(() => x
                .Generate<Dictionary<Type, Func<object, object>>>()
                .ToImmutableDictionary());
        }
    }
}