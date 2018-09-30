using Akka.Actor;
using Akka.TestKit;
using System;
using System.Collections.Generic;

namespace ConnelHooley.AkkaTestingHelpers.Helpers.Abstract
{
    internal interface ITestProbeParentActor
    {
        TestProbe TestProbe { get; }

        IEnumerable<Exception> UnhandledExceptions { get; }

        IActorRef Ref { get; }
    }
}
