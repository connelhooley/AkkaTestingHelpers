using Akka.TestKit;
using FluentAssertions;
using NUnit.Framework;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.TestProbeResolverTests
{
    internal class Supervisor : TestBase
    {
        [Test]
        public void TestProbeResolver_Supervisor_ReturnsSupervisor()
        {
            //arrange
            TestProbeResolver sut = CreateTestProbeResolver(TestProbeResolverSettings.Empty);

            //act
            TestProbe result = sut.Supervisor;

            //assert
            result.Should().BeSameAs(Supervisor);
        }
    }
}