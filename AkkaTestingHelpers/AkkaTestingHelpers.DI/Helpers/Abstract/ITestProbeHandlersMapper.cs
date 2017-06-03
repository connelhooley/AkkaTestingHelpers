using System;
using System.Collections.Immutable;

namespace ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract
{
    internal interface ITestProbeHandlersMapper
    {
        ImmutableDictionary<Type, ImmutableDictionary<Type, Func<object, object>>> Map(ImmutableDictionary<(Type, Type), Func<object, object>> settingsHandlers);
    }
}