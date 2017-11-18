using Akka.Actor;
using Akka.TestKit.Xunit2;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.MediumTests.UnitTestFrameworkTests.Examples
{
    public class Examples3 : TestKit
    {
        public Examples3() : base(AkkaConfig.Config) { }

        public class DummmyActor : ReceiveActor
        {
            public DummmyActor()
            {
                Receive<string>(s =>
                {
                    Context.Parent.Tell(s.ToUpper());
                });
            }
        }

        [Fact]
        public void DummyActor_ReceiveStringMessage_SendsUppercaseStringMessageToSupervisor()
        {
            //arrange
            UnitTestFramework<DummmyActor> framework = UnitTestFrameworkSettings
                .Empty
                .CreateFramework<DummmyActor>(this);

            //act
            framework.Sut.Tell("hello world");

            //assert
            framework.Supervisor.ExpectMsg("HELLO WORLD");
        }
    }
}