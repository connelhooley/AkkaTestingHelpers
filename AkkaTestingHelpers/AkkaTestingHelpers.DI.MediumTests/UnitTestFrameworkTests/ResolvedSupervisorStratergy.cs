using System;
using Akka.Actor;
using Akka.TestKit.Xunit2;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.MediumTests.UnitTestFrameworkTests
{
    public class ResolvedSupervisorStratergy : TestKit
    {
        public ResolvedSupervisorStratergy() : base(AkkaConfig.Config) { }

        [Fact]
        public void TestProbeResolver_ResolvedSupervisorStratergiesAreStored()
        {
            //arrange
            SupervisorStrategy parentSupervisorStrategy = new AllForOneStrategy(exception => Directive.Restart);
            SupervisorStrategy childSupervisorStrategy = new OneForOneStrategy(exception => Directive.Restart);
            UnitTestFramework<ParentActorWithSupervisorStratery> sut = UnitTestFrameworkSettings
                .Empty
                .CreateFramework<ParentActorWithSupervisorStratery>(this,
                    Props.Create(() =>
                        new ParentActorWithSupervisorStratery(parentSupervisorStrategy, childSupervisorStrategy)), 2);

            //act
            SupervisorStrategy result = sut.ResolvedSupervisorStratergy("ChildWithParentSupervisorStrategy");

            //assert
            result.Should().BeSameAs(parentSupervisorStrategy);
        }
        
        [Fact]
        public void TestProbeResolver_ParentSupervisorStratergiesAreReturnedWhenChildDoesNotHaveOneSetInItsProps()
        {
            //arrange
            SupervisorStrategy parentSupervisorStrategy = new AllForOneStrategy(exception => Directive.Restart);
            SupervisorStrategy childSupervisorStrategy = new OneForOneStrategy(exception => Directive.Restart);

            UnitTestFramework<ParentActorWithSupervisorStratery> sut = UnitTestFrameworkSettings
                .Empty
                .CreateFramework<ParentActorWithSupervisorStratery>(this,
                    Props.Create(() =>
                        new ParentActorWithSupervisorStratery(parentSupervisorStrategy, childSupervisorStrategy)), 2);

            //act
            SupervisorStrategy result = sut.ResolvedSupervisorStratergy("ChildWithChildSupervisorStrategy");

            //assert
            result.Should().BeSameAs(childSupervisorStrategy);
        }

        [Fact]
        public void TestProbeResolver_ThrownsWhenChildHasNotBeenResolved()
        {
            //arrange
            SupervisorStrategy parentSupervisorStrategy = new AllForOneStrategy(exception => Directive.Restart);
            SupervisorStrategy childSupervisorStrategy = new OneForOneStrategy(exception => Directive.Restart);

            UnitTestFramework<ParentActorWithSupervisorStratery> sut = UnitTestFrameworkSettings
                .Empty
                .CreateFramework<ParentActorWithSupervisorStratery>(this,
                    Props.Create(() =>
                        new ParentActorWithSupervisorStratery(parentSupervisorStrategy, childSupervisorStrategy)), 2);

            //act
            Action act = () => sut.ResolvedSupervisorStratergy(Guid.NewGuid().ToString());

            //assert
            act.ShouldThrow<ActorNotFoundException>();
        }
    }
}