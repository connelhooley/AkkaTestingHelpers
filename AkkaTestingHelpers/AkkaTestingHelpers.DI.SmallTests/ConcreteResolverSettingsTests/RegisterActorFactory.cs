﻿using System;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.ConcreteResolverSettingsTests
{
    public class Register : TestBase
    {
        #region Null tests
        [Fact]
        public void ConcreteResolverSettings_RegisterWithFactory_ThrowsArgumentNullException()
        {
            //arrange
            ConcreteResolverSettings sut = ConcreteResolverSettings.Empty;

            //act
            Action act = () => sut.RegisterActor<DummyActor1>(null);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }
        #endregion

        [Fact]
        public void ConcreteResolverSettings_RegisterWithFactory_ReturnsNewInstance()
        {
            //arrange
            ConcreteResolverSettings sut = ConcreteResolverSettings.Empty;

            //act
            ConcreteResolverSettings result = sut.RegisterActor(() => new DummyActor1());

            //assert
            result.Should().NotBe(sut);
        }
    }
}