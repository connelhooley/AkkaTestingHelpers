using System;
using System.Linq;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.Xunit2;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.MediumTests.TestProbeResolverTests
{
    public class WaitForChildren : TestKit
    {
        public WaitForChildren() : base(AkkaConfig.Config) { }

        [Fact]
        public void TestProbeResolver_WaitsForChildrenCreatedWhenProcessingMessages()
        {
            //arrange
            const int initialChildCount = 2;
            const int additionalChildCount = 5;
            Type childType = typeof(ReplyChildActor1);
            Guid message = Guid.NewGuid();
            UnitTestFramework<> sut = TestProbeResolverSettings
                .Empty
                .RegisterHandler<ReplyChildActor1, Guid>(guid => guid)
                .CreateResolver(this);
            TestActorRef<ParentActor> actor = sut.CreateSut<ParentActor>(Props.Create(() => new ParentActor(childType, initialChildCount)), initialChildCount);

            //act
            sut.TellMessage(actor, new CreateChildren(childType, additionalChildCount), additionalChildCount);

            //assert
            actor.Tell(new TellAllChildren(message));
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
            UnitTestFramework<> sut = TestProbeResolverSettings
                .Empty
                .CreateResolver(this);
            TestActorRef<ParentActor> actor = sut.CreateSut<ParentActor>(Props.Create(() => new ParentActor(childType, initialChildCount)), initialChildCount);

            //act
            Action act = () => sut.TellMessage(actor, new CreateChildren(childType, moreChildCount), moreChildCount + 1);

            //assert
            act.ShouldThrow<TimeoutException>();
        }
    }
}