using System;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.NUnit3;
using Akka.TestKit.TestActors;
using FluentAssertions;
using NUnit.Framework;

namespace ConnelHooley.AkkaTestingHelpers.DI.MediumTests.TestProbeResolverTests
{
    public class ResolvedTestProbe : TestKit
    {
        [Test]
        public void TestProbeResolver_ResolvedTestProbesAreStored()
        {
            //arrange
            Type childType = typeof(BlackHoleActor);
            string childName = Guid.NewGuid().ToString();
            Guid message = Guid.NewGuid();
            TestProbeResolver sut = TestProbeResolverSettings
                .Empty
                .CreateResolver(this);

            //act
            TestActorRef<ParentActor> actor = sut.CreateSut<ParentActor>(Props.Create(() => new ParentActor()), 0);
            sut.TellMessage(actor, new CreateChild(childName, childType), 1);
            TestProbe result = sut.ResolvedTestProbe(actor, childName);

            //assert
            actor.Tell(new TellChild(childName, message));
            result.ExpectMsg(message);
        }

        [Test]
        public void TestProbeResolver_ThrownsWhenChildHasNotBeenResolved()
        {
            //arrange
            TestProbeResolver sut = TestProbeResolverSettings
                .Empty
                .CreateResolver(this);

            //act
            TestActorRef<ParentActor> actor = sut.CreateSut<ParentActor>(Props.Create(() => new ParentActor()), 0);
            sut.TellMessage(actor, new CreateChild(Guid.NewGuid().ToString(), typeof(EchoActor)), 1);
            Action act = () => sut.ResolvedTestProbe(actor, Guid.NewGuid().ToString());

            //assert
            act.ShouldThrow<ActorNotFoundException>();
        }
    }
}