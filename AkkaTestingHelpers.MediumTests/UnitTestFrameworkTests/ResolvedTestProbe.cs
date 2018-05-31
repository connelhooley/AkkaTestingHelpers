using System;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.TestActors;
using Akka.TestKit.Xunit2;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.MediumTests.UnitTestFrameworkTests
{
    public class ResolvedTestProbe : TestKit
    {
        public ResolvedTestProbe() : base(AkkaConfig.Config) { }

        [Fact]
        public void TestProbeResolver_ResolvedTestProbesAreStored()
        {
            //arrange
            Type childType = typeof(BlackHoleActor);
            string childName = Guid.NewGuid().ToString();
            Guid message = Guid.NewGuid();
            UnitTestFramework<ParentActor> sut = UnitTestFrameworkSettings
                .Empty
                .CreateFramework<ParentActor>(this, Props.Create(() => new ParentActor()));

            //act
            sut.TellMessageAndWaitForChildren(new CreateChild(childName, childType), 1);
            TestProbe result = sut.ResolvedTestProbe(childName);

            //assert
            sut.Sut.Tell(new TellChild(childName, message));
            result.ExpectMsg(message);
        }

        [Fact]
        public void TestProbeResolver_ThrownsWhenChildHasNotBeenResolved()
        {
            //arrange
            UnitTestFramework<ParentActor> sut = UnitTestFrameworkSettings
                .Empty
                .CreateFramework<ParentActor>(this, Props.Create(() => new ParentActor()));

            //act
            sut.TellMessageAndWaitForChildren(new CreateChild(Guid.NewGuid().ToString(), typeof(EchoActor)), 1);
            Action act = () => sut.ResolvedTestProbe(Guid.NewGuid().ToString());

            //assert
            act.Should().Throw<ActorNotFoundException>();
        }
    }
}