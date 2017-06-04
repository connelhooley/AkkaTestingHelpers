﻿using System;
using Akka.TestKit;
using FluentAssertions;
using NUnit.Framework;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.TestProbeResolverTests
{
    internal class ResolvedTestProbe : TestBase
    {
        [Test]
        public void TestProbeResolver_ResolvedTestProbeWithNullParentActor_ThrowsArgumentNullException()
        {
            //arrange
            TestProbeResolver sut = CreateTestProbeResolver();

            //act
            Action act = () => sut.ResolvedTestProbe(null, ChildName);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void TestProbeResolver_ResolvedTestProbeWithNullChildName_ThrowsArgumentNullException()
        {
            //arrange
            TestProbeResolver sut = CreateTestProbeResolver();

            //act
            Action act = () => sut.ResolvedTestProbe(TestActor, null);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void TestProbeResolver_ResolvedTestProbeWithNullParentActorAndChildName_ThrowsArgumentNullException()
        {
            //arrange
            TestProbeResolver sut = CreateTestProbeResolver();

            //act
            Action act = () => sut.ResolvedTestProbe(null, null);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void TestProbeResolver_ResolvedTestProbe_ReturnsCorrectProbe()
        {
            //arrange
            TestProbeResolver sut = CreateTestProbeResolver();

            //act
            TestProbe result = sut.ResolvedTestProbe(TestActor, ChildName);

            //assert
            result.Should().BeSameAs(ResolvedTestProbe);
        }
    }
}