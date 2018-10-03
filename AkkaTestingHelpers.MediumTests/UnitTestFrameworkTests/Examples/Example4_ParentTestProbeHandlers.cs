using Akka.Actor;
using Akka.TestKit.Xunit2;
using Moq;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.MediumTests.UnitTestFrameworkTests.Examples
{
    public class Example4_ParentTestProbeHandlers : TestKit
    {
        public Example4_ParentTestProbeHandlers() : base(AkkaConfig.Config) { }

        public class ParentActor : ReceiveActor
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
                Receive<Save>(s => Context.Parent.Tell(s));
                Receive<ParentActor.ModifiedSave>(s => repo.Save(s.Value));
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
        public void SutActor_ReceiveSaveMessage_StoresModifiedSaveMessageFromParentInRepo()
        {
            //arrange
            Mock<IRepository> repoMock = new Mock<IRepository>();
            UnitTestFramework<SutActor> framework = UnitTestFrameworkSettings
                .Empty
                .RegisterParentHandler<SutActor.Save>(s => new ParentActor.ModifiedSave(s.Value.ToUpper()))
                .CreateFramework<SutActor>(this, Props.Create(() => new SutActor(repoMock.Object)));

            //act
            framework.Sut.Tell(new SutActor.Save("hello world"));

            //assert
            AwaitAssert(() => repoMock.Verify(repo => repo.Save("HELLO WORLD"), Times.Once));
        }
    }
}