using System;
using Akka.Actor;
using Akka.TestKit.Xunit2;
using Xunit;
using Akka.TestKit;

namespace ConnelHooley.AkkaTestingHelpers.MediumTests.UnitTestFrameworkTests
{
    public class Parent : TestKit
    {
        public Parent() : base(AkkaConfig.Config) { }

        [Fact]
        public void UnitTestFramework_ParentTestProbeReceivesMessagesSentToParent()
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

        [Fact]
        public void UnitTestFramework_ParentTestProbeHasCorrectMessageHandlers()
        {
            //arrange
            const int initialChildCount = 0;
            Guid message = Guid.NewGuid();
            Guid reply = Guid.NewGuid();

            UnitTestFramework<ParentActor> sut = UnitTestFrameworkSettings
                .Empty
                .RegisterParentHandler<Guid>(mess => reply)
                .CreateFramework<ParentActor>(this, Props.Create(() => new ParentActor()), initialChildCount);

            //act
            sut.Parent.Ref.Tell(message);

            //assert
            ExpectMsg(reply);
        }
    }
}