using Akka.TestKit;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.UnitTestFrameworkTests
{
    public class Supervisor : TestBase
    {
        [Fact]
        public void UnitTestFramework_Supervisor_ReturnsSupervisor()
        {
            //arrange
            UnitTestFramework<DummyActor> sut = CreateUnitTestFramework();

            //act
            TestProbe result = sut.Parent;

            //assert
            result.Should().BeSameAs(Supervisor);
        }
    }
}