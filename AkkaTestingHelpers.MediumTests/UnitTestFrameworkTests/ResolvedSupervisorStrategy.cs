using System;
using Akka.Actor;
using Akka.TestKit.Xunit2;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.MediumTests.UnitTestFrameworkTests
{
    public class ResolvedSupervisorStrategy : TestKit
    {
        public ResolvedSupervisorStrategy() : base(AkkaConfig.Config) { }

        [Fact]
        public void UnitTestFramework_ResolvedSupervisorStrategiesAreStored()
        {
            //arrange
            SupervisorStrategy parentSupervisorStrategy = new AllForOneStrategy(exception => Directive.Restart);
            SupervisorStrategy childSupervisorStrategy = new OneForOneStrategy(exception => Directive.Restart);
            UnitTestFramework<ParentActorWithSupervisorStratery> sut = UnitTestFrameworkSettings
                .Empty
                .CreateFramework<ParentActorWithSupervisorStratery>(
                    this, 
                    Props.Create(() => new ParentActorWithSupervisorStratery(parentSupervisorStrategy, childSupervisorStrategy)), 
                    2);

            //act
            SupervisorStrategy result = sut.ResolvedSupervisorStrategy("ChildWithParentSupervisorStrategy");

            //assert
            result.Should().BeSameAs(parentSupervisorStrategy);
        }
        
        [Fact]
        public void UnitTestFramework_ParentSupervisorStrategiesAreReturnedWhenChildDoesNotHaveOneSetInItsProps()
        {
            //arrange
            SupervisorStrategy parentSupervisorStrategy = new AllForOneStrategy(exception => Directive.Restart);
            SupervisorStrategy childSupervisorStrategy = new OneForOneStrategy(exception => Directive.Restart);

            UnitTestFramework<ParentActorWithSupervisorStratery> sut = UnitTestFrameworkSettings
                .Empty
                .CreateFramework<ParentActorWithSupervisorStratery>(
                    this,
                    Props.Create(() => new ParentActorWithSupervisorStratery(parentSupervisorStrategy, childSupervisorStrategy)), 
                    2);

            //act
            SupervisorStrategy result = sut.ResolvedSupervisorStrategy("ChildWithChildSupervisorStrategy");

            //assert
            result.Should().BeSameAs(childSupervisorStrategy);
        }

        [Fact]
        public void UnitTestFramework_ThrownsWhenChildHasNotBeenResolved()
        {
            //arrange
            SupervisorStrategy parentSupervisorStrategy = new AllForOneStrategy(exception => Directive.Restart);
            SupervisorStrategy childSupervisorStrategy = new OneForOneStrategy(exception => Directive.Restart);

            UnitTestFramework<ParentActorWithSupervisorStratery> sut = UnitTestFrameworkSettings
                .Empty
                .CreateFramework<ParentActorWithSupervisorStratery>(
                    this,
                    Props.Create(() => new ParentActorWithSupervisorStratery(parentSupervisorStrategy, childSupervisorStrategy)), 
                    2);

            //act
            Action act = () => sut.ResolvedSupervisorStrategy(Guid.NewGuid().ToString());

            //assert
            act.Should().Throw<ActorNotFoundException>();
        }
    }
}