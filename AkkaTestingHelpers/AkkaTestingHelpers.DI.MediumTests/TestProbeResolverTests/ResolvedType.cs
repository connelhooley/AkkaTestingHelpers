﻿using System;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.NUnit3;
using Akka.TestKit.TestActors;
using FluentAssertions;
using NUnit.Framework;

namespace ConnelHooley.AkkaTestingHelpers.DI.MediumTests.TestProbeResolverTests
{
    public class ResolvedType : TestKit
    {
        [Test]
        public void TestProbeResolver_ResolvedTypesAreStored()
        {
            //arrange
            Type childType = typeof(BlackHoleActor);
            string childName = Guid.NewGuid().ToString();
            TestProbeResolver sut = TestProbeResolverSettings
                .Empty
                .CreateResolver(this);

            //act
            TestActorRef<ParentActor> actor = sut.CreateSut<ParentActor>(Props.Create(() => new ParentActor()), 0);
            sut.TellMessage(actor, new CreateChild(childName, childType), 1);

            //assert
            sut.ResolvedType(actor, childName).Should().Be(childType);
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
            sut.TellMessage(actor, new CreateChild(Guid.NewGuid().ToString(), typeof(BlackHoleActor)), 1);
            Action act = () => sut.ResolvedType(actor, Guid.NewGuid().ToString());

            //assert
            act.ShouldThrow<ActorNotFoundException>();
        }
    }
}