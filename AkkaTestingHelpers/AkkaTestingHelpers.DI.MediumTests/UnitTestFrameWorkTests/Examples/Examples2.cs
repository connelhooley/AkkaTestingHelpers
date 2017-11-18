using Akka.Actor;
using Akka.DI.Core;
using Akka.TestKit.Xunit2;
using Moq;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.MediumTests.UnitTestFrameworkTests.Examples
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
            Mock<IRepository> repoMock = new Mock<IRepository>();
            UnitTestFramework<ParentActor> framework = UnitTestFrameworkSettings
                .Empty
                .RegisterHandler<ChildActor, ParentActor.Save>(s => new ChildActor.ModifiedSave(s.Value.ToUpper()))
                .CreateFramework<ParentActor>(this, Props.Create(() => new ParentActor(repoMock.Object)), 1);

            //act
            framework.Sut.Tell(new ParentActor.Save("hello world"));

            //assert
            AwaitAssert(() => repoMock.Verify(repo => repo.Save("HELLO WORLD"), Times.Once));
        }
    }
}