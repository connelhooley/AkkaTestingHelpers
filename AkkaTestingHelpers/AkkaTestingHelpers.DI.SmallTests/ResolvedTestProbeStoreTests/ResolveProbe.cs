using System;
using Akka.Actor;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.ResolvedTestProbeStoreTests
{
    public class ResolveProbe : TestBase
    {
        [Fact]
        public void ResolvedTestProbeRepository_ResolveProbeWithNullActorPath_ThrowsArgumentNullException()
        {
            //arrange
            ResolvedTestProbeStore sut = CreateResolvedTestProbeStore();

            //act
            Action act = () => sut.ResolveProbe(
                null,
                TestUtils.Create<Type>(),
                CreateTestProbe());

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void ResolvedTestProbeRepository_ResolveProbeWithNullType_ThrowsArgumentNullException()
        {
            //arrange
            ResolvedTestProbeStore sut = CreateResolvedTestProbeStore();

            //act
            Action act = () => sut.ResolveProbe(
                TestUtils.Create<ActorPath>(),
                null,
                CreateTestProbe());

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void ResolvedTestProbeRepository_ResolveProbeWithNullTestProbe_ThrowsArgumentNullException()
        {
            //arrange
            ResolvedTestProbeStore sut = CreateResolvedTestProbeStore();

            //act
            Action act = () => sut.ResolveProbe(
                TestUtils.Create<ActorPath>(),
                TestUtils.Create<Type>(),
                null);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void ResolvedTestProbeRepository_ResolveProbeWithNullActorPathAndTypeAndTestProbe_ThrowsArgumentNullException()
        {
            //arrange
            ResolvedTestProbeStore sut = CreateResolvedTestProbeStore();

            //act
            Action act = () => sut.ResolveProbe(null, null, null);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }


        [Fact]
        public void ResolvedTestProbeRepository_ResolveProbe_DoesNotThrowException()
        {
            //arrange
            ResolvedTestProbeStore sut = CreateResolvedTestProbeStore();

            //act
            Action act = () => sut.ResolveProbe(
                TestUtils.Create<ActorPath>(), 
                TestUtils.Create<Type>(), 
                CreateTestProbe());

            //assert
            act.ShouldNotThrow();
        }
    }
}