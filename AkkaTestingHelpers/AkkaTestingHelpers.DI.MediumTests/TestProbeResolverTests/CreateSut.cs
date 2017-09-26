    using System;
using System.Linq;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.Xunit2;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.MediumTests.TestProbeResolverTests
{
    public class CreateSut : TestKit
    {
        public CreateSut() : base(AkkaConfig.Config) { }
        
        [Fact]
        public void TestProbeResolver_CreatesChildrenWithNoReplies()
        {
            //arrange
            const int childCount = 5;
            Type childType = typeof(ReplyChildActor1);
            TestProbeResolver sut = TestProbeResolverSettings
                .Empty
                .RegisterHandler<ReplyChildActor2, Guid>(guid => Guid.Empty)
                .CreateResolver(this);

            //act
            TestActorRef<ParentActor> actor = sut.CreateSut<ParentActor>(Props.Create(() => new ParentActor(childType, childCount)), childCount);

            //assert
            actor.Tell(new TellAllChildren(Guid.NewGuid()));
            ExpectNoMsg();
        }

        [Fact]
        public void TestProbeResolver_CreatesChildrenWithReplies()
        {
            //arrange
            const int childCount = 5;
            Type childType = typeof(ReplyChildActor1);
            Guid message = Guid.NewGuid();
            int replyCount = 0;
            TestProbeResolver sut = TestProbeResolverSettings
                .Empty
                .RegisterHandler<ReplyChildActor2, Guid>(guid => (default(Guid), default(int)))
                .RegisterHandler<ReplyChildActor1, Guid>(guid => (guid, ++replyCount))
                .CreateResolver(this);

            //act
            TestActorRef<ParentActor> actor = sut.CreateSut<ParentActor>(Props.Create(() => new ParentActor(childType, childCount)), childCount);

            //assert
            actor.Tell(new TellAllChildren(message));
            ExpectMsgAllOf(Enumerable
                .Range(1, childCount)
                .Select(i => (message, i))
                .ToArray()
            );
        }

        [Fact]
        public void TestProbeResolver_TimesOutWhenChildrenCountIsTooHigh()
        {
            //arrange
            const int childCount = 5;
            Type childType = typeof(ReplyChildActor1);
            TestProbeResolver sut = TestProbeResolverSettings
                .Empty
                .CreateResolver(this);

            //act
            Action act = () => sut.CreateSut<ParentActor>(Props.Create(() => new ParentActor(childType, childCount)), childCount + 1);

            //assert
            act.ShouldThrow<TimeoutException>();
        }

        [Fact]//failed in build
        public void TestProbeResolver_UsesLatestHandler()
        {
            //arrange
            const int childCount = 5;
            Type childType = typeof(ReplyChildActor1);
            Guid message = Guid.NewGuid();
            int replyCount = 0;
            TestProbeResolver sut = TestProbeResolverSettings
                .Empty
                .RegisterHandler<ReplyChildActor1, Guid>(guid => (default(Guid), default(int)))
                .RegisterHandler<ReplyChildActor1, Guid>(guid => (guid, ++replyCount))
                .CreateResolver(this);
            
            //act
            TestActorRef<ParentActor> actor = sut.CreateSut<ParentActor>(Props.Create(() => new ParentActor(childType, childCount)), childCount);

            //assert
            actor.Tell(new TellAllChildren(message));
            ExpectMsgAllOf(Enumerable
                .Range(1, childCount)
                .Select(i => (message, i))
                .ToArray()
            );
        }
    }
}