using System;
using System.Collections.Generic;
using Akka.TestKit;

namespace ConnelHooley.AkkaTestingHelpers.Helpers.Abstract
{
    internal interface ITestProbeActorCreator
    {
        ITestProbeActor Create(
            ITestProbeCreator testProbeCreator,
            TestKitBase testKit,
            IReadOnlyDictionary<Type, Func<object, object>> handlers = null);
    }
}