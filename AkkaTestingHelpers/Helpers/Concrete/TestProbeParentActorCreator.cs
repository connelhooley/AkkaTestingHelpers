using System;
using System.Collections.Generic;
using Akka.Actor;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.Helpers.Abstract;

namespace ConnelHooley.AkkaTestingHelpers.Helpers.Concrete
{
    internal sealed class TestProbeParentActorCreator : ITestProbeParentActorCreator
    {
        public ITestProbeParentActor Create(
            ITestProbeCreator testProbeCreator,
            IWaiter exceptionWaiter,
            TestKitBase testKit,
            Func<Exception, Directive> decider,
            IReadOnlyDictionary<Type, Func<object, object>> handlers) =>
            new TestProbeParentActor(testProbeCreator, exceptionWaiter, testKit, decider, handlers);
    }
}