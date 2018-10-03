using Akka.Actor;
using Akka.DI.Core;
using Akka.TestKit.Xunit2;
using Moq;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.MediumTests.UnitTestFrameworkTests.Examples
{
    public class Example2_ChildTestProbeHandlers : TestKit
    {
        public Example2_ChildTestProbeHandlers() : base(AkkaConfig.Config) { }

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

        public class SutActor : ReceiveActor
        {
            public SutActor(IRepository repo)
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
        public void SutActor_ReceiveSaveMessage_StoresModifiedSaveMessageFromChildInRepo()
        {
            //arrange
            Mock<IRepository> repoMock = new Mock<IRepository>();
            UnitTestFramework<SutActor> framework = UnitTestFrameworkSettings
                .Empty
                .RegisterChildHandler<ChildActor, SutActor.Save>(s => new ChildActor.ModifiedSave(s.Value.ToUpper()))
                .CreateFramework<SutActor>(this, Props.Create(() => new SutActor(repoMock.Object)), 1);

            //act
            framework.Sut.Tell(new SutActor.Save("hello world"));

            //assert
            AwaitAssert(() => repoMock.Verify(repo => repo.Save("HELLO WORLD"), Times.Once));
        }
    }
}