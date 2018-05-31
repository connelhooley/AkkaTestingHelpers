using System;
using System.Linq;
using Akka.Actor;
using Akka.TestKit.Xunit2;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.MediumTests.UnitTestFrameworkTests
{
    public class TellMessageAndWaitForChildren : TestKit
    {
        public TellMessageAndWaitForChildren() : base(AkkaConfig.Config) { }

        [Fact]
        public void TestProbeResolver_WaitsForChildrenCreatedWhenProcessingMessages()
        {
            //arrange
            const int initialChildCount = 2;
            const int additionalChildCount = 5;
            Type childType = typeof(ReplyChildActor1);
            Guid message = Guid.NewGuid();
            UnitTestFramework<ParentActor> sut = UnitTestFrameworkSettings
                .Empty
                .RegisterHandler<ReplyChildActor1, Guid>(guid => guid)
                .CreateFramework<ParentActor>(this, Props.Create(() => new ParentActor(childType, initialChildCount)), initialChildCount);

            //act
            sut.TellMessageAndWaitForChildren(new CreateChildren(childType, additionalChildCount), additionalChildCount);

            //assert
            sut.Sut.Tell(new TellAllChildren(message));
            ExpectMsgAllOf(Enumerable
                .Repeat(message, initialChildCount + additionalChildCount)
                .ToArray());
        }

        [Fact]
        public void TestProbeResolver_TimesoutWhenWaitingForChildrenWithAnExpectedChildCountThatIsTooHigh()
        {
            //arrange
            const int initialChildCount = 2;
            const int moreChildCount = 5;
            Type childType = typeof(ReplyChildActor1);
            UnitTestFramework<ParentActor> sut = UnitTestFrameworkSettings
                .Empty
                .CreateFramework<ParentActor>(this, Props.Create(() => new ParentActor(childType, initialChildCount)), initialChildCount);
            
            //act
            Action act = () => sut.TellMessageAndWaitForChildren(new CreateChildren(childType, moreChildCount), moreChildCount + 1);

            //assert
            act.Should().Throw<TimeoutException>();
        }
    }
}