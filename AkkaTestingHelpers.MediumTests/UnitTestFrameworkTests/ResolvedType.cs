using System;
using Akka.Actor;
using Akka.TestKit.TestActors;
using Akka.TestKit.Xunit2;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.MediumTests.UnitTestFrameworkTests
{
    public class ResolvedType : TestKit
    {
        public ResolvedType() : base(AkkaConfig.Config) { }

        [Fact]
        public void UnitTestFramework_ResolvedChildTypesAreStored()
        {
            //arrange
            Type childType = typeof(BlackHoleActor);
            string childName = Guid.NewGuid().ToString();
            UnitTestFramework<ParentActor> sut = UnitTestFrameworkSettings
                .Empty
                .CreateFramework<ParentActor>(this, Props.Create(() => new ParentActor()));

            //act
            sut.TellMessageAndWaitForChildren(new CreateChild(childName, childType), 1);

            //assert
            sut.ResolvedType(childName).Should().Be(childType);
        }

        [Fact]
        public void UnitTestFramework_ThrownsWhenChildHasNotBeenResolved()
        {
            //arrange
            UnitTestFramework<ParentActor> sut = UnitTestFrameworkSettings
                .Empty
                .CreateFramework<ParentActor>(this, Props.Create(() => new ParentActor()));

            //act
            sut.TellMessageAndWaitForChildren(new CreateChild(Guid.NewGuid().ToString(), typeof(BlackHoleActor)), 1);
            Action act = () => sut.ResolvedType(Guid.NewGuid().ToString());

            //assert
            act.Should().Throw<ActorNotFoundException>();
        }
    }
}