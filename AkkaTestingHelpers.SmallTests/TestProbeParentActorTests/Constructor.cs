using System;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;
using FluentAssertions;
using Xunit;

// ReSharper disable ObjectCreationAsStatement

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.TestProbeParentActorTests
{
    public class Constructor : TestBase
    {
        #region Null tests
        [Fact]
        public void TestProbeParentActor_ConstructorWithNullTestProbeCreator_ThrowsArgumentNullException()
        {
            //act
            Action act = () => new TestProbeParentActor(
                null,
                ExceptionWaiter,
                this,
                Decider,
                Handlers);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeParentActor_ConstructorWithNullExceptionWaiter_ThrowsArgumentNullException()
        {
            //act
            Action act = () => new TestProbeParentActor(
                TestProbeCreator,
                null,
                this,
                Decider,
                Handlers);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeParentActor_ConstructorWithNullTestKit_ThrowsArgumentNullException()
        {
            //act
            Action act = () => new TestProbeParentActor(
                TestProbeCreator,
                ExceptionWaiter,
                null,
                Decider,
                Handlers);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeParentActor_ConstructorWithNullDecider_ThrowsArgumentNullException()
        {
            //act
            Action act = () => new TestProbeParentActor(
                TestProbeCreator,
                ExceptionWaiter,
                this,
                null,
                Handlers);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeParentActor_ConstructorWithNullHandlers_ThrowsArgumentNullException()
        {
            //act
            Action act = () => new TestProbeParentActor(
                TestProbeCreator,
                ExceptionWaiter,
                this,
                Decider,
                null);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeParentActor_ConstructorWithAllNulls_ThrowsArgumentNullException()
        {
            //act
            Action act = () => new TestProbeParentActor(
                null,
                null,
                null,
                null,
                null);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }
        #endregion

        [Fact]
        public void TestProbeParentActor_Constructor_DoesNotThrow()
        {
            //act
            Action act = () => new TestProbeParentActor(
                TestProbeCreator,
                ExceptionWaiter,
                this,
                Decider,
                Handlers);

            //assert
            act.Should().NotThrow();
        }
    }
}