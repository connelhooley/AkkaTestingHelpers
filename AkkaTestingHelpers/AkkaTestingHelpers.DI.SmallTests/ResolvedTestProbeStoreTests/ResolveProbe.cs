﻿using System;
using Akka.Actor;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;
using FluentAssertions;
using NUnit.Framework;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.ResolvedTestProbeStoreTests
{
    internal class ResolveProbe : TestBase
    {
        [Test]
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