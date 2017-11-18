using System;
using System.Collections.Generic;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.Helpers.Abstract;
using NullGuard;

namespace ConnelHooley.AkkaTestingHelpers.Helpers.Concrete
{
    internal sealed class TestProbeActorCreator : ITestProbeActorCreator
    {
        public ITestProbeActor Create(ITestProbeCreator testProbeCreator, TestKitBase testKit, [AllowNull]IReadOnlyDictionary<Type, Func<object, object>> handlers) =>
            new TestProbeActor(testProbeCreator, testKit, handlers);
    }
}