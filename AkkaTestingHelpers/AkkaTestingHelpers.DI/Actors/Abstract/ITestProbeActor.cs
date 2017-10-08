using System;
using System.Collections.Generic;
using Akka.Actor;
using Akka.TestKit;
using Akka.Util;

namespace ConnelHooley.AkkaTestingHelpers.DI.Actors.Abstract
{
    internal interface ITestProbeActor
    {
        ActorPath ActorPath { get; }
        TestProbe TestProbe { get; }
        ActorBase Actor { get; }
        void SetHandlers(IReadOnlyDictionary<Type, Func<object, object>> handlers);
        void SetHandlers(IReadOnlyDictionary<Type, Either<Action<object>, Func<object, object>>> handlers);
    }
}