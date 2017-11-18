using System;
using System.Linq;
using Akka.Actor;
using Akka.TestKit.Xunit2;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.MediumTests.UnitTestFrameworkTests
{
    public class Sut : TestKit
    {
        public Sut() : base(AkkaConfig.Config) { }

        [Fact]
        public void TestProbeResolver_CreatesChildrenWithNoReplies()
        {
            //arrange
            const int childCount = 5;
            Type childType = typeof(ReplyChildActor1);

            //act
            UnitTestFramework<ParentActor> sut = UnitTestFrameworkSettings
                .Empty
                .RegisterHandler<ReplyChildActor2, Guid>(guid => Guid.Empty)
                .CreateFramework<ParentActor>(this, Props.Create(() => new ParentActor(childType, childCount)), childCount);

            //assert
            sut.Sut.Tell(new TellAllChildren(Guid.NewGuid()));
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

            //act
            UnitTestFramework<ParentActor> sut = UnitTestFrameworkSettings
                .Empty
                .RegisterHandler<ReplyChildActor2, Guid>(guid => (default(Guid), default(int)))
                .RegisterHandler<ReplyChildActor1, Guid>(guid => (guid, ++replyCount))
                .CreateFramework<ParentActor>(this, Props.Create(() => new ParentActor(childType, childCount)), childCount);
            
            //assert
            sut.Sut.Tell(new TellAllChildren(message));
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

            //act
            Action act = () =>
            {
                UnitTestFramework<ParentActor> sut = UnitTestFrameworkSettings
                    .Empty
                    .CreateFramework<ParentActor>(this, Props.Create(() => new ParentActor(childType, childCount)), childCount+1);
            };
            
            //assert
            act.ShouldThrow<TimeoutException>();
        }

        [Fact]
        public void TestProbeResolver_UsesLatestHandler()
        {
            //arrange
            const int childCount = 5;
            Type childType = typeof(ReplyChildActor1);
            Guid message = Guid.NewGuid();
            int replyCount = 0;

            //act
            UnitTestFramework<ParentActor> sut = UnitTestFrameworkSettings
                .Empty
                .RegisterHandler<ReplyChildActor1, Guid>(guid => (default(Guid), default(int)))
                .RegisterHandler<ReplyChildActor1, Guid>(guid => (guid, ++replyCount))
                .CreateFramework<ParentActor>(this, Props.Create(() => new ParentActor(childType, childCount)), childCount);
            
            //assert
            sut.Sut.Tell(new TellAllChildren(message));
            ExpectMsgAllOf(Enumerable
                .Range(1, childCount)
                .Select(i => (message, i))
                .ToArray()
            );
        }
    }
}