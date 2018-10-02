using System;
using System.Linq;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.TestActors;
using Akka.TestKit.Xunit2;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.MediumTests.UnitTestFrameworkTests
{
    public class ResolvedTestProbe : TestKit
    {
        public ResolvedTestProbe() : base(AkkaConfig.Config) { }

        [Fact]
        public void UnitTestFramework_ThrowsWhenChildHasNotBeenResolved()
        {
            //arrange
            UnitTestFramework<ParentActor> sut = UnitTestFrameworkSettings
                .Empty
                .CreateFramework<ParentActor>(this, Props.Create(() => new ParentActor()));

            //act
            sut.TellMessageAndWaitForChildren(new CreateChild(Guid.NewGuid().ToString(), typeof(EchoActor)), 1);
            Action act = () => sut.ResolvedTestProbe(Guid.NewGuid().ToString());

            //assert
            act.Should().Throw<ActorNotFoundException>();
        }

        [Fact]
        public void UnitTestFramework_ResolvedChildTestProbesHaveCorrectMessageHandlers()
        {
            //arrange
            const int initialChildCount = 0;
            const string createdChildName = "child-name";
            Guid message = Guid.NewGuid();
            Guid reply = Guid.NewGuid();

            UnitTestFramework<ParentActor> sut = UnitTestFrameworkSettings
                .Empty
                .RegisterChildHandler<ReplyChildActor1, Guid>(mess => reply)
                .CreateFramework<ParentActor>(
                    this,
                    Props.Create(() => new ParentActor()),
                    initialChildCount);

            sut.TellMessageAndWaitForChildren(new CreateChild(createdChildName, typeof(ReplyChildActor1)), 1);

            //act
            TestProbe childProbe = sut.ResolvedTestProbe(createdChildName);

            //assert
            childProbe.Ref.Tell(message);
            ExpectMsg(reply);
        }

        [Fact]
        public void UnitTestFramework_ResolvedChildTestProbesDoNotReplyWhenTheyHaveNoMessageHandlers()
        {
            //arrange
            const int childCount = 5;
            Type childType = typeof(ReplyChildActor1);

            //act
            UnitTestFramework<ParentActor> sut = UnitTestFrameworkSettings
                .Empty
                .RegisterChildHandler<ReplyChildActor2, Guid>(guid => Guid.Empty)
                .CreateFramework<ParentActor>(this, Props.Create(() => new ParentActor(childType, childCount)), childCount);

            //assert
            sut.Sut.Tell(new TellAllChildren(Guid.NewGuid()));
            ExpectNoMsg();
        }

        [Fact]
        public void UnitTestFramework_ResolvedChildTestProbesUsesLatestHandler()
        {
            //arrange
            const int childCount = 5;
            Type childType = typeof(ReplyChildActor1);
            Guid message = Guid.NewGuid();
            int replyCount = 0;

            //act
            UnitTestFramework<ParentActor> sut = UnitTestFrameworkSettings
                .Empty
                .RegisterChildHandler<ReplyChildActor1, Guid>(guid => (default(Guid), default(int)))
                .RegisterChildHandler<ReplyChildActor1, Guid>(guid => (guid, ++replyCount))
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