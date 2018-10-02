using System;
using System.Linq;
using Akka.Actor;
using Akka.TestKit.Xunit2;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.MediumTests.UnitTestFrameworkTests
{
    public class CreateFramework : TestKit
    {
        public CreateFramework() : base(AkkaConfig.Config) { }
        
        [Fact]
        public void UnitTestFramework_TimesOutWhenChildrenCountIsTooHigh()
        {
            //arrange
            const int childCount = 5;
            Type childType = typeof(ReplyChildActor1);

            //act
            Action act = () =>
            {
                UnitTestFramework<ParentActor> sut = UnitTestFrameworkSettings
                    .Empty
                    .CreateFramework<ParentActor>(this, Props.Create(() => new ParentActor(childType, childCount)), childCount+1);
            };
            
            //assert
            act.Should().Throw<TimeoutException>();
        }
    }
}