using Akka.TestKit;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.UnitTestFrameworkTests
{
    public class Sut : TestBase
    {
        [Fact]
        public void UnitTestFramework_Supervisor_ReturnsSupervisor()
        {
            //arrange
            UnitTestFramework<DummyActor> sut = CreateUnitTestFramework();

            //act
            TestActorRef<DummyActor> result = sut.Sut;

            //assert
            result.Should().BeSameAs(SutActor);
        }
    }
}