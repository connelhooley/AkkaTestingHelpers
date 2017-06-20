using Akka.TestKit;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.TestProbeResolverTests
{
    internal class Supervisor : TestBase
    {
        [Fact]
        public void TestProbeResolver_Supervisor_ReturnsSupervisor()
        {
            //arrange
            TestProbeResolver sut = CreateTestProbeResolver();

            //act
            TestProbe result = sut.Supervisor;

            //assert
            result.Should().BeSameAs(Supervisor);
        }
    }
}