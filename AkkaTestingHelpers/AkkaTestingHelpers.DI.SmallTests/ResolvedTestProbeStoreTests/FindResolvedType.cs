using System;
using Akka.Actor;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.ResolvedTestProbeStoreTests
{
    public class FindResolvedType : TestBase
    {
        #region Null tests
        [Fact]
        public void ResolvedTestProbeStore_FindResolvedTypeWithNullParentRef_ThrowsArgumentNullException()
        {
            //arrange
            ResolvedTestProbeStore sut = CreateResolvedTestProbeStore();

            //act
            Action act = () => sut.FindResolvedType(null, TestUtils.Create<string>());

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void ResolvedTestProbeStore_FindResolvedTypeWithNullChildName_ThrowsArgumentNullException()
        {
            //arrange
            ResolvedTestProbeStore sut = CreateResolvedTestProbeStore();

            //act
            Action act = () => sut.FindResolvedType(TestActor, null);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void ResolvedTestProbeStore_FindResolvedTypeWithNullParentRefAndChildName_ThrowsArgumentNullException()
        {
            //arrange
            ResolvedTestProbeStore sut = CreateResolvedTestProbeStore();

            //act
            Action act = () => sut.FindResolvedType(null, null);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }
        #endregion

        [Fact]
        public void ResolvedTestProbeStore_NoActorsAreResolved_FindResolvedType_ThrowsActorNotFoundException()
        {
            //arrange
            (ActorPath path, _, _, _, string name) = CreateChildVariables();
            ResolvedTestProbeStore sut = CreateResolvedTestProbeStore();

            //act
            Action act = () => sut.FindResolvedType(TestActor, name);

            //assert
            act
                .ShouldThrow<ActorNotFoundException>()
                .WithMessage($"No child has been resolved for the path '{path}'");
        }

        [Fact]
        public void ResolvedTestProbeStore_SingleActorIsResolved_FindResolvedTypeWithNameThatHasNotBeenResolved_ThrowsActorNotFoundException()
        {
            //arrange
            (ActorPath path1, Type type1, TestProbe probe1, SupervisorStrategy supervisorStrategy1, _) = CreateChildVariables();
            (ActorPath path2, _, _, _, string name2) = CreateChildVariables();
            ResolvedTestProbeStore sut = CreateResolvedTestProbeStore();
            sut.ResolveProbe(path1, type1, probe1, supervisorStrategy1);

            //act
            Action act = () => sut.FindResolvedType(TestActor, name2);

            //assert
            act
                .ShouldThrow<ActorNotFoundException>()
                .WithMessage($"No child has been resolved for the path '{path2}'");
        }

        [Fact]
        public void ResolvedTestProbeStore_SingleActorIsResolved_FindResolvedTypeWithNameThatHasBeenResolved_ReturnsCorrectTestProbe()
        {
            //arrange
            (ActorPath path, Type type, TestProbe probe, SupervisorStrategy supervisorStrategy, string name) = CreateChildVariables();
            ResolvedTestProbeStore sut = CreateResolvedTestProbeStore();
            sut.ResolveProbe(path, type, probe, supervisorStrategy);

            //act
            Type result = sut.FindResolvedType(TestActor, name);

            //assert
            result.Should().Be(type);
        }

        [Fact]
        public void ResolvedTestProbeStore_MultipleActorsAreResolved_FindResolvedTypeWithNameThatHasBeenResolved_ReturnsCorrectTestProbe()
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
            Type result = sut.FindResolvedType(TestActor, name2);

            //assert
            result.Should().Be(type2);
        }

        [Fact]
        public void ResolvedTestProbeStore_MultipleActorsAreResolved_FindResolvedTypeWithNameThatHasNotBeenResolved_ThrowsActorNotFoundException()
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
            Action act = () => sut.FindResolvedType(TestActor, name4);

            //assert
            act
                .ShouldThrow<ActorNotFoundException>()
                .WithMessage($"No child has been resolved for the path '{path4}'");
        }
    }
}