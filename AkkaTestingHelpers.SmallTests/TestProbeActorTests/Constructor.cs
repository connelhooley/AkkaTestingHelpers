﻿using System;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;
using FluentAssertions;
using Xunit;

// ReSharper disable ObjectCreationAsStatement

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.TestProbeActorTests
{
    public class Constructor : TestBase
    {
        #region Null tests
        [Fact]
        public void TestProbeActor_ConstructorWithNullTestKit_ThrowsArgumentNullException()
        {
            //act
            Action act = () => new TestProbeActor(
                TestProbeCreator,
                null,
                Handlers);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeActor_ConstructorWithNullTestProbeCreator_ThrowsArgumentNullException()
        {
            //act
            Action act = () => new TestProbeActor(
                TestProbeCreator,
                null,
                Handlers);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void TestProbeActor_ConstructorWithNullHandlers_DoesNotThrow()
        {
            //act
            Action act = () => new TestProbeActor(
                TestProbeCreator,
                this,
                null);

            //assert
            act.Should().NotThrow();
        }

        [Fact]
        public void TestProbeActor_ConstructorWithAllNulls_ThrowsArgumentNullException()
        {
            //act
            Action act = () => new TestProbeActor(
                null,
                null);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }
        #endregion

        [Fact]
        public void TestProbeActor_Constructor_DoesNotThrow()
        {
            //act
            Action act = () => new TestProbeActor(
                TestProbeCreator,
                this,
                Handlers);

            //assert
            act.Should().NotThrow();
        }
    }
}