using System;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Configuration;
using Akka.TestKit.Xunit2;
using Moq;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.MediumTests.UnitTestFrameworkTests.Examples
{
    public class Example7_Delayer : TestKit
    {
        public Example7_Delayer() : base(ConfigurationFactory
            .ParseString("akka.test.timefactor = 2")
            .WithFallback(AkkaConfig.Config)) { }

        public interface IRepository
        {
            void Save(string value);
        }

        public class SutActor : ReceiveActor
        {
            public SutActor(IRepository repository)
            {
                ReceiveAsync<string>(async message => {
                    await Task.Delay(1800).ConfigureAwait(false);
                    repository.Save(message);
                });
            }
        }

        [Fact]
        public void ParentActor_ReceiveStringMessage_CallsRepoAfterDuration()
        {
            //arrange
            Mock<IRepository> repoMock = new Mock<IRepository>();
            string message = Guid.NewGuid().ToString();
            UnitTestFramework<SutActor> framework = UnitTestFrameworkSettings
                .Empty
                .CreateFramework<SutActor>(this, Props.Create(() => new SutActor(repoMock.Object)));

            //act
            framework.Sut.Tell(message);

            //assert
            framework.Delay(TimeSpan.FromSeconds(1)); // 1 second is multipled by timefactor to make 2 seconds
            repoMock.Verify(
                repo => repo.Save(message),
                Times.Once);
        }
    }
}