using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.Xunit2;
using Moq;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.MediumTests.BasicResolverTests
{
    public class ResolveActor : TestKit
    {
        public ResolveActor() : base(AkkaConfig.Config) { }

        [Fact]
        public void BasicResolverSettings_CreatesChildrenWithoutDependancies()
        {
            //arrange
            BasicResolverSettings
                .Empty
                .RegisterActor<EmptyChildActor>()
                .RegisterActor<ChildActor>()
                .RegisterResolver(this);

            //act
            TestActorRef<ParentActor> actor = ActorOfAsTestActorRef<ParentActor>(Props.Create<ParentActor>());

            //assert
            actor.Tell(new object());
            ExpectMsg(ChildActor.Token);
        }

        [Fact]
        public void BasicResolverSettings_CreatesChildrenWithDependancies()
        {
            //arrange
            Mock<IDependancy> dependancyMock = new Mock<IDependancy>();
            IDependancy dependancyMockInstance = dependancyMock.Object;
            BasicResolverSettings
                .Empty
                .RegisterActor<EmptyChildActor>()
                .RegisterActor(() => new ChildActor(dependancyMockInstance))
                .RegisterResolver(this);

            //act
            TestActorRef<ParentActor> actor = ActorOfAsTestActorRef<ParentActor>(Props.Create<ParentActor>());

            //assert
            actor.Tell(new object());
            AwaitAssert(() =>
                dependancyMock.Verify(
                    dependancy => dependancy.SetResut(ChildActor.Token),
                    Times.Once));
        }
        
        [Fact]
        public void BasicResolverSettings_UsesLatestFactory()
        {
            //arrange
            Mock<IDependancy> dependancyMock = new Mock<IDependancy>();
            IDependancy dependancyMockInstance = dependancyMock.Object;
            BasicResolverSettings
                .Empty
                .RegisterActor<EmptyChildActor>()
                .RegisterActor<ChildActor>()
                .RegisterActor(() => new ChildActor(dependancyMockInstance))
                .RegisterResolver(this);

            //act
            TestActorRef<ParentActor> actor = ActorOfAsTestActorRef<ParentActor>(Props.Create<ParentActor>());

            //assert
            actor.Tell(new object());
            AwaitAssert(() =>
                dependancyMock.Verify(
                    dependancy => dependancy.SetResut(ChildActor.Token),
                    Times.Once));
        }
    }
}