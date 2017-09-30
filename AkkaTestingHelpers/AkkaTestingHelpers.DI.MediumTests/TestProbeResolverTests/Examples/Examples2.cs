using Akka.Actor;
using Akka.DI.Core;
using Akka.TestKit;
using Akka.TestKit.Xunit2;
using Moq;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.MediumTests.TestProbeResolverTests.Examples
{
    public class Examples2 : TestKit
    {
        public Examples2() : base(AkkaConfig.Config) { }

        public class ChildActor : ReceiveActor
        {
            public class ModifiedSave
            {
                public string Value { get; }

                public ModifiedSave(string value)
                {
                    Value = value;
                }
            }
        }

        public interface IRepository
        {
            void Save(string value);
        }

        public class ParentActor : ReceiveActor
        {
            public ParentActor(IRepository repo)
            {
                var child = Context.ActorOf(Context.DI().Props<ChildActor>(), "child-actor-1");
                Receive<Save>(s => child.Tell(s));
                Receive<ChildActor.ModifiedSave>(s => repo.Save(s.Value));
            }

            public class Save
            {
                public string Value { get; }

                public Save(string value)
                {
                    Value = value;
                }
            }
        }
        
        [Fact]
        public void ParentActor_ReceiveSaveMessage_StoresModifiedSaveMessageFromChildInRepo()
        {
            //arrange
            TestProbeResolver resolver = TestProbeResolverSettings
                .Empty
                .RegisterHandler<ChildActor, ParentActor.Save>(s => new ChildActor.ModifiedSave(s.Value.ToUpper()))
                .CreateResolver(this);
            Mock<IRepository> repoMock = new Mock<IRepository>();
            Props props = Props.Create(() => new ParentActor(repoMock.Object));
            TestActorRef<ParentActor> sut = resolver.CreateSut<ParentActor>(props, 1);

            //act
            sut.Tell(new ParentActor.Save("hello world"));

            //assert
            AwaitAssert(() => repoMock.Verify(repo => repo.Save("HELLO WORLD"), Times.Once));
        }
    }
}