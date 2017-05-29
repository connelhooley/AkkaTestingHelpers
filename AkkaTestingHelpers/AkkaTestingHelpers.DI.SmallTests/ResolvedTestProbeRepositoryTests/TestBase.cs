using System;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.NUnit3;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;
using NUnit.Framework;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.ResolvedTestProbeRepositoryTests
{
    internal class TestBase : TestKit
    {
        protected ActorPath ActorPath;
        protected Type ActorType;
        protected TestProbe TestProbe;

        [SetUp]
        public void SetUp()
        {
            // Create objects passed into sut
            ActorPath = ActorPath.Parse($"akka://user/{Guid.NewGuid()}");
            ActorType = TestUtils.Create<Type>();
            TestProbe = CreateTestProbe();
        }

        [TearDown]
        public void TearDown()
        {
            ActorPath = null;
            ActorType = null;
            TestProbe = null;
        }

        public ResolvedTestProbeStore CreateResolvedTestProbeStore() => new ResolvedTestProbeStore();
    }
}