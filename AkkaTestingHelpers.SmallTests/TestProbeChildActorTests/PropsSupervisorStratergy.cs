using Akka.Actor;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;
using ConnelHooley.TestHelpers;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.TestProbeChildActorTests
{
    public class PropsSupervisorStrategy : TestBase
    {
        [Fact]
        public void TestProbeChildActorCreatedWithoutSupervisorStrategy_PropsSupervisorStrategy_ReturnsNull()
        {
            //arrange
            TestActorRef<TestProbeChildActor> sut = CreateTestProbeChildActorWithoutSupervisorStrategy();

            //act
            SupervisorStrategy result = sut.UnderlyingActor.PropsSupervisorStrategy;

            //assert
            result.Should().BeNull();
        }

        [Fact]
        public void TestProbeChildActorWithSupervisorStrategy_PropsSupervisorStrategy__ReturnsSameResultOnEveryCall()
        {
            //arrange
            AllForOneStrategy exptected = new AllForOneStrategy(
                TestHelper.GenerateNumber(), 
                TestHelper.GenerateNumber(),
                exception => TestHelper.Generate<Directive>());
            TestProbeChildActor sut = CreateTestProbeChildActorWithSupervisorStrategy(exptected).UnderlyingActor;

            //act
            SupervisorStrategy result = sut.PropsSupervisorStrategy;

            //assert
            result.Should().BeSameAs(exptected);
        }
    }
}