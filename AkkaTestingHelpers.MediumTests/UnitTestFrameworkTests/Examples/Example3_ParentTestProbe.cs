using Akka.Actor;
using Akka.TestKit.Xunit2;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.MediumTests.UnitTestFrameworkTests.Examples
{
    public class Example3_ParentTestProbe : TestKit
    {
        public Example3_ParentTestProbe() : base(AkkaConfig.Config) { }

        public class SutActor : ReceiveActor
        {
            public SutActor()
            {
                Receive<string>(s =>
                {
                    Context.Parent.Tell(s.ToUpper());
                });
            }
        }

        [Fact]
        public void SutActor_ReceiveStringMessage_SendsUpperCaseStringMessageToParent()
        {
            //arrange
            UnitTestFramework<SutActor> framework = UnitTestFrameworkSettings
                .Empty
                .CreateFramework<SutActor>(this);

            //act
            framework.Sut.Tell("hello world");

            //assert
            framework.Parent.ExpectMsg("HELLO WORLD");
        }
    }
}