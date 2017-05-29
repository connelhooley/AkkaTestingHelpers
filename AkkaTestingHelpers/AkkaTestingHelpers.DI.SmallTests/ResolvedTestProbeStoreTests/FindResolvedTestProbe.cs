using System;
using Akka.Actor;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;
using FluentAssertions;
using NUnit.Framework;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.ResolvedTestProbeStoreTests
{
    internal class FindResolvedTestProbe : TestBase
    {
        [Test]
        public void ResolvedTestProbeStore_FindResolvedTestProbeWithNullParentRef_ThrowsArgumentNullException()
        {
            //arrange
            (_, _, _, string name) = CreateChildVariables();
            ResolvedTestProbeStore sut = CreateResolvedTestProbeStore();

            //act
            Action act = () => sut.FindResolvedTestProbe(null, name);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void ResolvedTestProbeStore_FindResolvedTestProbeWithNullChildName_ThrowsArgumentNullException()
        {
            //arrange
            ResolvedTestProbeStore sut = CreateResolvedTestProbeStore();

            //act
            Action act = () => sut.FindResolvedTestProbe(TestActor, null);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void ResolvedTestProbeStore_FindResolvedTestProbeWithNullParentRefAndChildName_ThrowsArgumentNullException()
        {
            //arrange
            ResolvedTestProbeStore sut = CreateResolvedTestProbeStore();

            //act
            Action act = () => sut.FindResolvedTestProbe(null,  null);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void ResolvedTestProbeStore_NoActorsAreResolved_FindResolvedTestProbe_ThrowsActorNotFoundException()
        {
            //arrange
            (ActorPath path, _, _, string name) = CreateChildVariables();
            ResolvedTestProbeStore sut = CreateResolvedTestProbeStore();

            //act
            Action act = () => sut.FindResolvedTestProbe(TestActor, name);

            //assert
            act.ShouldThrow<ActorNotFoundException>().WithMessage($"No child has been resolved for the path '{path}'");
        }

        [Test]
        public void ResolvedTestProbeStore_SingleActorIsResolved_FindResolvedTestProbeWithNameThatHasNotBeenResolved_ThrowsActorNotFoundException()
        {
            //arrange
            (ActorPath path1, Type type1, TestProbe probe1, _) = CreateChildVariables();
            (ActorPath path2, _, _, string name2) = CreateChildVariables();
            ResolvedTestProbeStore sut = CreateResolvedTestProbeStore();
            sut.ResolveProbe(path1, type1, probe1);

            //act
            Action act = () => sut.FindResolvedTestProbe(TestActor, name2);

            //assert
            act.ShouldThrow<ActorNotFoundException>().WithMessage($"No child has been resolved for the path '{path2}'");
        }

        [Test]
        public void ResolvedTestProbeStore_SingleActorIsResolved_FindResolvedTestProbeWithNameThatHasBeenResolved_ReturnsCorrectTestProbe()
        {
            //arrange
            (ActorPath path, Type type, TestProbe probe, string name) = CreateChildVariables();
            ResolvedTestProbeStore sut = CreateResolvedTestProbeStore();
            sut.ResolveProbe(path, type, probe);

            //act
            TestProbe result = sut.FindResolvedTestProbe(TestActor, name);

            //assert
            result.Should().Be(probe);
        }
        
        [Test]
        public void ResolvedTestProbeStore_MultipleActorsAreResolved_FindResolvedTestProbeWithNameThatHasBeenResolved_ReturnsCorrectTestProbe()
        {
            //arrange
            ResolvedTestProbeStore sut = CreateResolvedTestProbeStore();
            (ActorPath path1, Type type1, TestProbe probe1, _) = CreateChildVariables();
            (ActorPath path2, Type type2, TestProbe probe2, string name2) = CreateChildVariables();
            (ActorPath path3, Type type3, TestProbe probe3, _) = CreateChildVariables();
            sut.ResolveProbe(path1, type1, probe1);
            sut.ResolveProbe(path2, type2, probe2);
            sut.ResolveProbe(path3, type3, probe3);

            //act
            TestProbe result = sut.FindResolvedTestProbe(TestActor, name2);

            //assert
            result.Should().Be(probe2);
        }

        [Test]
        public void ResolvedTestProbeStore_MultipleActorsAreResolved_FindResolvedTestProbeWithNameThatHasNotBeenResolved_ThrowsActorNotFoundException()
        {
            //arrange
            ResolvedTestProbeStore sut = CreateResolvedTestProbeStore();
            (ActorPath path1, Type type1, TestProbe probe1, _) = CreateChildVariables();
            (ActorPath path2, Type type2, TestProbe probe2, _) = CreateChildVariables();
            (ActorPath path3, Type type3, TestProbe probe3, _) = CreateChildVariables();
            (ActorPath path4, _, _, string name4) = CreateChildVariables();
            sut.ResolveProbe(path1, type1, probe1);
            sut.ResolveProbe(path2, type2, probe2);
            sut.ResolveProbe(path3, type3, probe3);

            //act
            Action act = () => sut.FindResolvedTestProbe(TestActor, name4);

            //assert
            act.ShouldThrow<ActorNotFoundException>().WithMessage($"No child has been resolved for the path '{path4}'");
        }
    }
}