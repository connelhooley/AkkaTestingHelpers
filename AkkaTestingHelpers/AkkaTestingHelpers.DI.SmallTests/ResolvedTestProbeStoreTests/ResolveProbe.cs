using System;
using Akka.Actor;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.ResolvedTestProbeStoreTests
{
    public class ResolveProbe : TestBase
    {
        #region Null tests
        [Fact]
        public void ResolvedTestProbeStore_ResolveProbeWithNullActorPath_ThrowsArgumentNullException()
        {
            //arrange
            var child = CreateChildVariables();
            ResolvedTestProbeStore sut = CreateResolvedTestProbeStore();

            //act
            Action act = () => sut.ResolveProbe(
                null,
                child.Type,
                child.TestProbe,
                child.SupervisorStrategy);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void ResolvedTestProbeStore_ResolveProbeWithNullType_ThrowsArgumentNullException()
        {
            //arrange
            var child = CreateChildVariables();
            ResolvedTestProbeStore sut = CreateResolvedTestProbeStore();

            //act
            Action act = () => sut.ResolveProbe(
                child.ActorPath,
                null,
                child.TestProbe,
                child.SupervisorStrategy);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void ResolvedTestProbeStore_ResolveProbeWithNullTestProbe_ThrowsArgumentNullException()
        {
            //arrange
            var child = CreateChildVariables();
            ResolvedTestProbeStore sut = CreateResolvedTestProbeStore();

            //act
            Action act = () => sut.ResolveProbe(
                child.ActorPath,
                child.Type,
                null,
                child.SupervisorStrategy);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }
        
        [Fact]
        public void ResolvedTestProbeStore_ResolveProbeWithNullTestProbe_DoesNotThrow()
        {
            //arrange
            var child = CreateChildVariables();
            ResolvedTestProbeStore sut = CreateResolvedTestProbeStore();

            //act
            Action act = () => sut.ResolveProbe(
                child.ActorPath,
                child.Type,
                child.TestProbe,
                null);

            //assert
            act.ShouldNotThrow();
        }

        [Fact]
        public void ResolvedTestProbeStore_ResolveProbeWithNullActorPathAndTypeAndTestProbe_ThrowsArgumentNullException()
        {
            //arrange
            var child = CreateChildVariables();
            ResolvedTestProbeStore sut = CreateResolvedTestProbeStore();

            //act
            Action act = () => sut.ResolveProbe(
                null, 
                null, 
                null,
                child.SupervisorStrategy);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }
        #endregion

        [Fact]
        public void ResolvedTestProbeStore_ResolveProbe_DoesNotThrowException()
        {
            //arrange
            ResolvedTestProbeStore sut = CreateResolvedTestProbeStore();

            //act
            Action act = () => sut.ResolveProbe(
                TestUtils.Create<ActorPath>(), 
                TestUtils.Create<Type>(),
                CreateTestProbe(),
                new AllForOneStrategy(
                    TestUtils.Create<int>(),
                    TestUtils.Create<int>(),
                    exception => TestUtils.Create<Directive>()));

            //assert
            act.ShouldNotThrow();
        }
    }
}