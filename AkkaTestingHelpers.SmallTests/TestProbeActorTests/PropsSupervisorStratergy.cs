using Akka.Actor;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;
using ConnelHooley.TestHelpers;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.TestProbeActorTests
{
    public class PropsSupervisorStrategy : TestBase
    {
        [Fact]
        public void TestProbeActorCreatedWithoutSupervisorStrategy_PropsSupervisorStrategy_ReturnsNull()
        {
            //arrange
            TestActorRef<TestProbeActor> sut = CreateTestProbeActorWithoutSupervisorStrategy();

            //act
            SupervisorStrategy result = sut.UnderlyingActor.PropsSupervisorStrategy;

            //assert
            result.Should().BeNull();
        }

        [Fact]
        public void TestProbeActorWithSupervisorStrategy_PropsSupervisorStrategy__ReturnsSameResultOnEveryCall()
        {
            //arrange
            AllForOneStrategy exptected = new AllForOneStrategy(
                TestHelper.GenerateNumber(), 
                TestHelper.GenerateNumber(),
                exception => TestHelper.Generate<Directive>());
            TestProbeActor sut = CreateTestProbeActorWithSupervisorStrategy(exptected).UnderlyingActor;

            //act
            SupervisorStrategy result = sut.PropsSupervisorStrategy;

            //assert
            result.Should().BeSameAs(exptected);
        }
    }
}