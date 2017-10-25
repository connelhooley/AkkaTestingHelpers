﻿using System;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.TestProbeHandlersMapperTests
{
    public class Constructor : TestBase
    {
        [Fact]
        public void TestProbeHandlersMapper_Constructor_DoesNotThrow()
        {
            //act
            Action act = () => new TestProbeHandlersMapper();

            //assert
            act.ShouldNotThrow();
        }
    }
}