using System;
using System.Collections.Generic;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.Helpers.Abstract;
using NullGuard;

namespace ConnelHooley.AkkaTestingHelpers.Helpers.Concrete
{
    internal sealed class TestProbeChildActorCreator : ITestProbeChildActorCreator
    {
        public ITestProbeChildActor Create(ITestProbeCreator testProbeCreator, TestKitBase testKit, [AllowNull]IReadOnlyDictionary<Type, Func<object, object>> handlers) =>
            new TestProbeChildActor(testProbeCreator, testKit, handlers);
    }
}