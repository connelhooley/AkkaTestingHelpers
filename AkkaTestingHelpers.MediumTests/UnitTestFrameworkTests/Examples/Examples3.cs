using Akka.Actor;
using Akka.TestKit.Xunit2;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.MediumTests.UnitTestFrameworkTests.Examples
{
    public class Examples3 : TestKit
    {
        public Examples3() : base(AkkaConfig.Config) { }

        public class DummyActor : ReceiveActor
        {
            public DummyActor()
            {
                Receive<string>(s =>
                {
                    Context.Parent.Tell(s.ToUpper());
                });
            }
        }

        [Fact]
        public void DummyActor_ReceiveStringMessage_SendsUpperCaseStringMessageToSupervisor()
        {
            //arrange
            UnitTestFramework<DummyActor> framework = UnitTestFrameworkSettings
                .Empty
                .CreateFramework<DummyActor>(this);

            //act
            framework.Sut.Tell("hello world");

            //assert
            framework.Parent.ExpectMsg("HELLO WORLD");
        }
    }
}