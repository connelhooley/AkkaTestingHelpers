using System;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.NUnit3;
using NUnit.Framework;

namespace ConnelHooley.AkkaTestingHelpers.DI.MediumTests.TestProbeResolverTests
{
    internal class Supervisor : TestKit
    {
        public Supervisor() : base(AkkaConfig.Config) { }

        [Test]
        public void TestProbeResolver_SupervisorTestProbeReceivesMessagesSentToParent()
        {
            //arrange
            const int initialChildCount = 0;
            Guid message = Guid.NewGuid();
            TestProbeResolver sut = TestProbeResolverSettings
                .Empty
                .CreateResolver(this);
            TestActorRef<ParentActor> actor = sut.CreateSut<ParentActor>(Props.Create(() => new ParentActor()), initialChildCount);

            //act
            actor.Tell(new TellParent(message));

            //assert
            sut.Supervisor.ExpectMsg(message);
        }
    }
}