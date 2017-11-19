using Akka.Actor;
using Akka.DI.Core;
using Akka.TestKit.Xunit2;
using Moq;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.MediumTests.BasicResolverTests.Examples
{
    public class Examples1 : TestKit
    {
        public Examples1() : base(AkkaConfig.Config) { }
        
        public class ParentActor : ReceiveActor
        {
            public ParentActor()
            {
                var child = Context.ActorOf(Context.DI().Props<ChildActor>());
                child.Tell("hello");
            }
        }
        
        public class ChildActor : ReceiveActor
        {
            public ChildActor()
            {
                var grandChild = Context.ActorOf(Context.DI().Props<GrandChildActor>());
                ReceiveAny(message => grandChild.Forward(message));
            }
        }
        
        public class GrandChildActor : ReceiveActor
        {
            public GrandChildActor(IRepo repo)
            {
                ReceiveAny(message => repo.Save(message));
            }
        }

        public interface IRepo
        {
            void Save(object message);
        }

        [Fact]
        public void ParentActorConstructorSendsMessageToChild_ChildSendsMessageToGrandChild_GrandChildSavesMessageInRepo()
        {
            //act
            Mock<IRepo> repoMock = new Mock<IRepo>();
            BasicResolverSettings
                .Empty
                .RegisterActor<ChildActor>()
                .RegisterActor(() => new GrandChildActor(repoMock.Object))
                .RegisterResolver(this);

            //act
            ActorOfAsTestActorRef<ParentActor>();
            
            AwaitAssert(() =>
                repoMock.Verify(
                    repo => repo.Save("hello"),
                    Times.Once()));
        }
    }
}