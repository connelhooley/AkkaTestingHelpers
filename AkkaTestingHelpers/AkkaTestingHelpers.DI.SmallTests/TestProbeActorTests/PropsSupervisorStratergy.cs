using Akka.Actor;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.TestProbeActorTests
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
                TestUtils.Create<int>(), 
                TestUtils.Create<int>(),
                exception => TestUtils.Create<Directive>());
            TestProbeActor sut = CreateTestProbeActorWithSupervisorStrategy(exptected).UnderlyingActor;

            //act
            SupervisorStrategy result = sut.PropsSupervisorStrategy;

            //assert
            result.Should().BeSameAs(exptected);
        }
    }
}