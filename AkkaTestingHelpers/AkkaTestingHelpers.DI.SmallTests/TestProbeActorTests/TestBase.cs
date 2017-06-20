using System;
using System.Collections.Generic;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.Xunit2;
using ConnelHooley.AkkaTestingHelpers.DI.Actors.Concrete;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.TestProbeActorTests
{
    public class TestBase : TestKit
    {
        internal object Message1;
        internal Type Type1;
        internal object Reply1;
        internal object Message2;
        internal Type Type2;
        internal object Reply2;
        internal Dictionary<Type, Func<object, object>> Handlers;

        public TestBase() : base(AkkaConfig.Config)
        {
            
            Message1 = new ExampleMessage1();
            Type1 = typeof(ExampleMessage1);
            Reply1 = TestUtils.Create<object>();
            Message2 = new ExampleMessage2();
            Type2 = typeof(ExampleMessage2);
            Reply2 = TestUtils.Create<object>();
            Handlers = new Dictionary<Type, Func<object, object>>
            {
                { Type1, o => Reply1 },
                { Type2, o => Reply2 }
            };
        }
        
        internal TestActorRef<TestProbeActor> CreateTestProbeActor() => 
            ActorOfAsTestActorRef<TestProbeActor>(
                Props.Create(() => new TestProbeActor(this)),
                TestUtils.Create<string>());

        private class ExampleMessage1 { }
        private class ExampleMessage2 { }
    }
}