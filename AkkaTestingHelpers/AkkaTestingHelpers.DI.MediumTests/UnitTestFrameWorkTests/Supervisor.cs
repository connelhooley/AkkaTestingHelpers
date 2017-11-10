using System;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.Xunit2;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.MediumTests.TestProbeResolverTests
{
    public class Supervisor : TestKit
    {
        public Supervisor() : base(AkkaConfig.Config) { }

        [Fact]
        public void TestProbeResolver_SupervisorTestProbeReceivesMessagesSentToParent()
        {
            //arrange
            const int initialChildCount = 0;
            Guid message = Guid.NewGuid();
            UnitTestFramework<> sut = TestProbeResolverSettings
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