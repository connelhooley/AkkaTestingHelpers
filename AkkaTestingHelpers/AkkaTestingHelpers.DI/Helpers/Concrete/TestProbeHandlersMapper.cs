using System;
using System.Collections.Immutable;
using System.Linq;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract;

namespace ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete
{
    internal class TestProbeHandlersMapper : ITestProbeHandlersMapper
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