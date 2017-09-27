using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.Xunit2;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.MediumTests.TestProbeResolverTests.Examples
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
            TestProbeResolver resolver = TestProbeResolverSettings
                .Empty
                .CreateResolver(this);
            TestActorRef<DummmyActor> sut = resolver.CreateSut<DummmyActor>(0);

            //act
            sut.Tell("hello world");

            //assert
            resolver.Supervisor.ExpectMsg("HELLO WORLD");
        }
    }
}