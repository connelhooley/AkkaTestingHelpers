﻿using System;
using Akka.Actor;
using Akka.DI.Core;
using Akka.TestKit;
using Akka.TestKit.TestActors;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;
using FluentAssertions;
using Moq;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.DependencyResolverAdderTests
{
    public class Add : TestBase
    {
        #region Null tests
        [Fact]
        public void DependencyResolverAdder_AddWithNullTestKitBase_ThrowsArgumentNullException()
        {
            //arrange
            DependencyResolverAdder sut = CreateDependencyResolverAdder();

            //act
            Action act = () => sut.Add(null, type => new BlackHoleActor());

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void DependencyResolverAdder_AddWithNullActorFactory_ThrowsArgumentNullException()
        {
            //arrange
            DependencyResolverAdder sut = CreateDependencyResolverAdder();

            //act
            Action act = () => sut.Add(this, null);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void DependencyResolverAdder_AddWithNullTestKitBaseAndActorFactory_ThrowsArgumentNullException()
        {
            //arrange
            DependencyResolverAdder sut = CreateDependencyResolverAdder();

            //act
            Action act = () => sut.Add(null, null);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }
        #endregion

        [Fact]
        public void DependencyResolverAdder_Add_ResultOfFactoryIsUsedToCreateActors()
        {
            //arrange
            DependencyResolverAdder sut = CreateDependencyResolverAdder();
            
            //act
            sut.Add(this, ActorFactory);

            //assert
            TestActorRef<ActorBase> result = ActorOfAsTestActorRef<ActorBase>(Sys.DI().Props<ActorBase>());
            result.UnderlyingActor.Should().BeSameAs(ReturnedActor);
        }

        [Fact]
        public void DependencyResolverAdder_Add_FactoryIsGivenCorrectType()
        {
            //arrange
            DependencyResolverAdder sut = CreateDependencyResolverAdder();

            //act
            sut.Add(this, ActorFactory);

            //assert
            ActorOfAsTestActorRef<ActorBase>(Sys.DI().Props<DummyActor>());
            TypeGivenToFactory.Should().Be<DummyActor>();
        }

        [Fact]
        public void DependencyResolverAdder_Add_LatestFactoryIsUsed()
        {
            //arrange
            DependencyResolverAdder sut = CreateDependencyResolverAdder();
            foreach (int i in TestUtils.CreateMany<int>())
            {
                sut.Add(this, type => new Mock<ActorBase>().Object);
            }

            //act
            sut.Add(this, ActorFactory);

            //assert
            TestActorRef<ActorBase> result = ActorOfAsTestActorRef<ActorBase>(Sys.DI().Props<DummyActor>());
            result.UnderlyingActor.Should().BeSameAs(ReturnedActor);
        }
    }
}