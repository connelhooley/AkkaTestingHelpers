using System;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;
using FluentAssertions;
using Xunit;

// ReSharper disable ObjectCreationAsStatement

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.TestProbeChildActorTests
{
    public class Constructor : TestBase
    {
        #region Null tests
        [Fact]
        public void TestProbeChildActor_ConstructorWithNullTestKit_ThrowsArgumentNullException()
        {
            //act
            Action act = () => new TestProbeChildActor(
                TestProbeCreator,
                null,
                Handlers);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeChildActor_ConstructorWithNullTestProbeCreator_ThrowsArgumentNullException()
        {
            //act
            Action act = () => new TestProbeChildActor(
                TestProbeCreator,
                null,
                Handlers);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeChildActor_ConstructorWithNullHandlers_DoesNotThrow()
        {
            //act
            Action act = () => new TestProbeChildActor(
                TestProbeCreator,
                this,
                null);

            //assert
            act.Should().NotThrow();
        }

        [Fact]
        public void TestProbeChildActor_ConstructorWithAllNulls_ThrowsArgumentNullException()
        {
            //act
            Action act = () => new TestProbeChildActor(
                null,
                null);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }
        #endregion

        [Fact]
        public void TestProbeChildActor_Constructor_DoesNotThrow()
        {
            //act
            Action act = () => new TestProbeChildActor(
                TestProbeCreator,
                this,
                Handlers);

            //assert
            act.Should().NotThrow();
        }
    }
}