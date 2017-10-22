using System;
using System.Collections.Generic;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.DI.Actors.Abstract;
using ConnelHooley.AkkaTestingHelpers.DI.Actors.Concrete;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract;
using NullGuard;

namespace ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete
{
    internal sealed class TestProbeActorCreator : ITestProbeActorCreator
    {
        public ITestProbeActor Create(ITestProbeCreator testProbeCreator, TestKitBase testKit, [AllowNull]IReadOnlyDictionary<Type, Func<object, object>> handlers) =>
            new TestProbeActor(testProbeCreator, testKit, handlers);
    }
}