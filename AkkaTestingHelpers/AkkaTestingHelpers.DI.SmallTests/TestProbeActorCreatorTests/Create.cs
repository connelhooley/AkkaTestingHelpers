using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.TestProbeActorCreatorTests
{
    public class Create : TestBase
    {
        [Fact]
        public void TestProbeActorCreator_Create_ReturnsTestProbeActor()
        {
            TestProbeActorCreator sut = CreateTestProbeCreator();
            
            //ITestProbeActor result = sut.Create();

            //todo shims
        }
    }
}