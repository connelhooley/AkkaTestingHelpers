using System;
using System.Collections.Generic;
using Akka.TestKit;

namespace ConnelHooley.AkkaTestingHelpers.Helpers.Abstract
{
    internal interface ITestProbeChildActorCreator
    {
        ITestProbeChildActor Create(
            ITestProbeCreator testProbeCreator,
            TestKitBase testKit,
            IReadOnlyDictionary<Type, Func<object, object>> handlers = null);
    }
}