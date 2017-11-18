using System;
using Akka.Actor;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.ResolvedTestProbeStoreTests
{
    public class FindResolvedSupervisorStrategy : TestBase
    {
        #region Null tests
        [Fact]
        public void ResolvedTestProbeStore_FindResolvedSupervisorStrategyWithNullParentRef_ThrowsArgumentNullException()
        {
            //arrange
            ResolvedTestProbeStore sut = CreateResolvedTestProbeStore();

            //act
            Action act = () => sut.FindResolvedSupervisorStrategy(null, TestUtils.Create<string>());

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void ResolvedTestProbeStore_FindResolvedSupervisorStrategyWithNullChildName_ThrowsArgumentNullException()
        {
            //arrange
            ResolvedTestProbeStore sut = CreateResolvedTestProbeStore();

            //act
            Action act = () => sut.FindResolvedSupervisorStrategy(TestActor, null);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void ResolvedTestProbeStore_FindResolvedSupervisorStrategyWithNullParentRefAndChildName_ThrowsArgumentNullException()
        {
            //arrange
            ResolvedTestProbeStore sut = CreateResolvedTestProbeStore();

            //act
            Action act = () => sut.FindResolvedSupervisorStrategy(null,  null);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }
        #endregion

        [Fact]
        public void ResolvedTestProbeStoreWithNoActorsResolved_FindResolvedSupervisorStrategy_ThrowsActorNotFoundException()
        {
            //arrange
            (ActorPath path, _, _, _, string name) = CreateChildVariables();
            ResolvedTestProbeStore sut = CreateResolvedTestProbeStore();

            //act
            Action act = () => sut.FindResolvedSupervisorStrategy(TestActor, name);

            //assert
            act
                .ShouldThrow<ActorNotFoundException>()
                .WithMessage($"No child has been resolved for the path '{path}'");
        }

        [Fact]
        public void ResolvedTestProbeStoreWithASingleActorResolved_FindResolvedSupervisorStrategyWithNameThatHasBeenResolvedWithASupervisorStrategy_ReturnsCorrectSupervisorStrategy()
        {
            //arrange
            (ActorPath path, Type type, TestProbe probe, SupervisorStrategy supervisorStrategy, string name) = CreateChildVariables();
            ResolvedTestProbeStore sut = CreateResolvedTestProbeStore();
            sut.ResolveProbe(path, type, probe, supervisorStrategy);

            //act
            SupervisorStrategy result = sut.FindResolvedSupervisorStrategy(TestActor, name);

            //assert
            result.Should().Be(supervisorStrategy);
        }
        
        [Fact]
        public void ResolvedTestProbeStoreWithASingleActorResolved_FindResolvedSupervisorStrategyWithNameThatHasBeenResolvedWithoutASupervisorStrategy_ReturnsNull()
        {
            //arrange
            (ActorPath path, Type type, TestProbe probe, _, string name) = CreateChildVariables();
            ResolvedTestProbeStore sut = CreateResolvedTestProbeStore();
            sut.ResolveProbe(path, type, probe, null);

            //act
            SupervisorStrategy result = sut.FindResolvedSupervisorStrategy(TestActor, name);

            //assert
            result.Should().BeNull();
        }

        [Fact]
        public void ResolvedTestProbeStoreWithASingleActorResolved_FindResolvedSupervisorStrategyWithNameThatHasNotBeenResolved_ThrowsActorNotFoundException()
        {
            //arrange
            (ActorPath path1, Type type1, TestProbe probe1, SupervisorStrategy supervisorStrategy1, _) = CreateChildVariables();
            (ActorPath path2, _, _, _, string name2) = CreateChildVariables();
            ResolvedTestProbeStore sut = CreateResolvedTestProbeStore();
            sut.ResolveProbe(path1, type1, probe1, supervisorStrategy1);

            //act
            Action act = () => sut.FindResolvedSupervisorStrategy(TestActor, name2);

            //assert
            act
                .ShouldThrow<ActorNotFoundException>()
                .WithMessage($"No child has been resolved for the path '{path2}'");
        }

        [Fact]
        public void ResolvedTestProbeStoreWithMultipleActorsResolved_FindResolvedSupervisorStrategyWithNameThatHasBeenResolvedWithASupervisorStrategy_ReturnsCorrectSupervisorStrategy()
        {
            //arrange
            ResolvedTestProbeStore sut = CreateResolvedTestProbeStore();
            (ActorPath path1, Type type1, TestProbe probe1, SupervisorStrategy supervisorStrategy1, _) = CreateChildVariables();
            (ActorPath path2, Type type2, TestProbe probe2, SupervisorStrategy supervisorStrategy2, string name2) = CreateChildVariables();
            (ActorPath path3, Type type3, TestProbe probe3, SupervisorStrategy supervisorStrategy3, _) = CreateChildVariables();
            sut.ResolveProbe(path1, type1, probe1, supervisorStrategy1);
            sut.ResolveProbe(path2, type2, probe2, supervisorStrategy2);
            sut.ResolveProbe(path3, type3, probe3, supervisorStrategy3);

            //act
            SupervisorStrategy result = sut.FindResolvedSupervisorStrategy(TestActor, name2);

            //assert
            result.Should().Be(supervisorStrategy2);
        }
        
        [Fact]
        public void ResolvedTestProbeStoreWithMultipleActorsResolved_FindResolvedSupervisorStrategyWithNameThatHasBeenResolvedWithoutASupervisorStrategy_ReturnsNull()
        {
            //arrange
            ResolvedTestProbeStore sut = CreateResolvedTestProbeStore();
            (ActorPath path1, Type type1, TestProbe probe1, SupervisorStrategy supervisorStrategy1, _) = CreateChildVariables();
            (ActorPath path2, Type type2, TestProbe probe2, _, string name2) = CreateChildVariables();
            (ActorPath path3, Type type3, TestProbe probe3, SupervisorStrategy supervisorStrategy3, _) = CreateChildVariables();
            sut.ResolveProbe(path1, type1, probe1, supervisorStrategy1);
            sut.ResolveProbe(path2, type2, probe2, null);
            sut.ResolveProbe(path3, type3, probe3, supervisorStrategy3);

            //act
            SupervisorStrategy result = sut.FindResolvedSupervisorStrategy(TestActor, name2);

            //assert
            result.Should().BeNull();
        }

        [Fact]
        public void ResolvedTestProbeStoreWithMultipleActorsResolved_FindResolvedSupervisorStrategyWithNameThatHasNotBeenResolved_ThrowsActorNotFoundException()
        {
            //arrange
            ResolvedTestProbeStore sut = CreateResolvedTestProbeStore();
            (ActorPath path1, Type type1, TestProbe probe1, SupervisorStrategy supervisorStrategy1, _) = CreateChildVariables();
            (ActorPath path2, Type type2, TestProbe probe2, SupervisorStrategy supervisorStrategy2, _) = CreateChildVariables();
            (ActorPath path3, Type type3, TestProbe probe3, SupervisorStrategy supervisorStrategy3, _) = CreateChildVariables();
            (ActorPath path4, _, _, _, string name4) = CreateChildVariables();
            sut.ResolveProbe(path1, type1, probe1, supervisorStrategy1);
            sut.ResolveProbe(path2, type2, probe2, supervisorStrategy2);
            sut.ResolveProbe(path3, type3, probe3, supervisorStrategy3);

            //act
            Action act = () => sut.FindResolvedSupervisorStrategy(TestActor, name4);

            //assert
            act
                .ShouldThrow<ActorNotFoundException>()
                .WithMessage($"No child has been resolved for the path '{path4}'");
        }
    }
}