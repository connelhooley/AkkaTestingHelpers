﻿using System;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.TellWaiterTests
{
    public class Constructor : TestBase
    {
        [Fact]
        public void ChildTeller_Constructor_DoesNotThrow()
        {
            //act
            Action act = () => new TellWaiter();

            //assert
            act.Should().NotThrow();
        }
    }
}