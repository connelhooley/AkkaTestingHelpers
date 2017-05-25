using System;
using Akka.Actor;
using Akka.DI.Core;
using Akka.TestKit;
using Akka.TestKit.TestActors;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;
using FluentAssertions;
using NUnit.Framework;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.DependencyResolverAdderTests
{
    internal class Add : TestBase
    {
        [Test]
        public void DependencyResolverAdder_AddWithNullTestKitBase_ThrowsArgumentNullException()
        {
            //arrange
            DependencyResolverAdder sut = CreateDependencyResolverAdder();

            //act
            Action act = () => sut.Add(null, type => new BlackHoleActor());

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void DependencyResolverAdder_AddWithNullActorFactory_ThrowsArgumentNullException()
        {
            //arrange
            DependencyResolverAdder sut = CreateDependencyResolverAdder();

            //act
            Action act = () => sut.Add(this, null);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void DependencyResolverAdder_Add_ResultOfFactoryIsUsedToCreateActors()
        {
            //arrange
            DependencyResolverAdder sut = CreateDependencyResolverAdder();
            DummyActor actor = new DummyActor();
            
            //act
            sut.Add(this, type => actor);

            //assert
            TestActorRef<ActorBase> result = ActorOfAsTestActorRef<ActorBase>(Sys.DI().Props<ActorBase>());
            result.UnderlyingActor.Should().BeSameAs(actor);
        }

        [Test]
        public void DependencyResolverAdder_Add_FactoryIsGivenCorrectType()
        {
            //arrange
            DependencyResolverAdder sut = CreateDependencyResolverAdder();
            DummyActor actor = new DummyActor();
            Type actual = null;

            //act
            sut.Add(this, type =>
            {
                actual = type;
                return actor;
            });

            //assert
            ActorOfAsTestActorRef<ActorBase>(Sys.DI().Props<DummyActor>());
            actual.Should().Be<DummyActor>();
        }
    }
}