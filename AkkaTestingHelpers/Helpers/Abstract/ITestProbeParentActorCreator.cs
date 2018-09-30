using System;
using System.Collections.Generic;
using Akka.Actor;
using Akka.TestKit;

namespace ConnelHooley.AkkaTestingHelpers.Helpers.Abstract
{
    internal interface ITestProbeParentActorCreator
    {
        ITestProbeParentActor Create(
            ITestProbeCreator testProbeCreator,
            TestKitBase testKit,
            Func<Exception, Directive> decider,
            IReadOnlyDictionary<Type, Func<object, object>> handlers);
    }
}