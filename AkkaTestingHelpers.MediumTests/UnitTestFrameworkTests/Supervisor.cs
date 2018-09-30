using System;
using Akka.Actor;
using Akka.TestKit.Xunit2;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.MediumTests.UnitTestFrameworkTests
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

            UnitTestFramework<ParentActor> sut = UnitTestFrameworkSettings
                .Empty
                .CreateFramework<ParentActor>(this, Props.Create(() => new ParentActor()), initialChildCount);
            
            //act
            sut.Sut.Tell(new TellParent(message));

            //assert
            sut.Parent.ExpectMsg(message);
        }
    }
}