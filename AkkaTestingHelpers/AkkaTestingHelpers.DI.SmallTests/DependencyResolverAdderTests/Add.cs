using System;
using System.Collections.Generic;
using System.Linq;
using Akka.Actor;
using Akka.DI.Core;
using Akka.TestKit;
using Akka.TestKit.TestActors;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.DependencyResolverAdderTests
{
    internal class Add : TestBase
    {
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

        [Fact]
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

        [Fact]
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

        [Fact]
        public void DependencyResolverAdder_Add_LatestFactoryIsUsed()
        {
            //arrange
            DependencyResolverAdder sut = CreateDependencyResolverAdder();
            List<DummyActor> actors = TestUtils.CreateMany<DummyActor>();
            foreach (DummyActor actor in actors.Take(actors.Count))
            {
                sut.Add(this, type => actor);
            }
            DummyActor expected = new DummyActor();

            //act
            sut.Add(this, type => expected);

            //assert
            TestActorRef<ActorBase> result = ActorOfAsTestActorRef<ActorBase>(Sys.DI().Props<DummyActor>());
            result.UnderlyingActor.Should().BeSameAs(expected);
        }
    }
}