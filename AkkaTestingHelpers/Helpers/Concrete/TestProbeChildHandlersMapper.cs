using System;
using System.Collections.Immutable;
using System.Linq;
using ConnelHooley.AkkaTestingHelpers.Helpers.Abstract;

namespace ConnelHooley.AkkaTestingHelpers.Helpers.Concrete
{
    internal sealed class TestProbeChildHandlersMapper : ITestProbeChildHandlersMapper
    {
        public ImmutableDictionary<Type, ImmutableDictionary<Type, Func<object, object>>> Map(ImmutableDictionary<(Type, Type), Func<object, object>> settingsHandlers) =>
            settingsHandlers
                .ToLookup(
                    pair => pair.Key.Item1,
                    pair => new
                    {
                        messageType = pair.Key.Item2,
                        messageHandler = pair.Value
                    })
                .ToImmutableDictionary(
                    grouping => grouping.Key,
                    grouping => grouping.ToImmutableDictionary(
                        item => item.messageType,
                        item => item.messageHandler));
    }
}