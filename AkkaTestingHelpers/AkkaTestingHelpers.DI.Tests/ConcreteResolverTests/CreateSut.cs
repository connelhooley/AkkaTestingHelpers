using System;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace ConnelHooley.AkkaTestingHelpers.DI.Tests.ConcreteResolverTests
{
    public class CreateSut : TestKit
    {
        public CreateSut() : base(@"akka.test.timefactor = 0.6") { }

        [Test]
        public void ConcreteResolverTests_CreateSut_SettingsWithFactoryFunc_ChildIsInjected()
        {
            //arrange
            Guid childId = Guid.NewGuid();
            Mock<ChildWithDependancy.IDependancy> dependancyMock = new Mock<ChildWithDependancy.IDependancy>();
            ConcreteResolver sut = ConcreteResolverSettings
                .Empty
                .Register(() => new ChildWithDependancy.ChildActor(childId, dependancyMock.Object))
                .CreateResolver(this);

            //act
            TestActorRef<ChildWithDependancy.ParentActor> rootActor = sut.CreateSut<ChildWithDependancy.ParentActor>(
                Props.Create(() => new ChildWithDependancy.ParentActor()), 1);

            //assert
            rootActor.UnderlyingActor.Child.Tell(new {});
            dependancyMock.Verify(dependancy => dependancy.SetResut(It.Is<Guid>(guid => guid == childId)));
        }

        [Test]
        public void ConcreteResolverTests_CreateSut_SettingsWithGeneric_ChildIsInjected()
        {
            //arrange
            ConcreteResolver sut = ConcreteResolverSettings
                .Empty
                .Register<ChildWithoutDependancy.ChildActor>()
                .CreateResolver(this);

            //act
            TestActorRef<ChildWithoutDependancy.ParentActor> rootActor = sut.CreateSut<ChildWithoutDependancy.ParentActor>(
                Props.Create<ChildWithoutDependancy.ParentActor>(), 1);

            //assert
            rootActor.UnderlyingActor.Child.Tell(new { });
            ExpectMsg<object>();
        }

        [Test]
        public void ConcreteResolverTests_CreateSut_ParentThatCreatesMultipleChildren_AllChildrenAreInjected()
        {
            //arrange
            ConcreteResolver sut = ConcreteResolverSettings
                .Empty
                .Register<ParentThatCreatesManyChildren.ChildActor>()
                .CreateResolver(this);

            //act
            TestActorRef<ParentThatCreatesManyChildren.ParentActor> rootActor = sut.CreateSut<ParentThatCreatesManyChildren.ParentActor>(
                Props.Create(() => new ParentThatCreatesManyChildren.ParentActor(3)),
                3);

            //assert
            foreach (IActorRef child in rootActor.UnderlyingActor.Children)
            {
                child.Tell(new {});
            }
            ReceiveN(3);
        }

        [Test]
        public void ConcreteResolverTests_CreateSut_NoProps_ParentIsCreated()
        {
            //arrange
            ConcreteResolver sut = ConcreteResolverSettings
                .Empty
                .Register<ChildWithoutDependancy.ChildActor>()
                .CreateResolver(this);

            //act
            TestActorRef<ChildWithoutDependancy.ParentActor> rootActor = sut.CreateSut<ChildWithoutDependancy.ParentActor>();

            //assert
            rootActor.UnderlyingActor.Should().BeOfType<ChildWithoutDependancy.ParentActor>();
        }

        [Test]
        public void ConcreteResolverTests_CreateSut_ExpectedParentCountIsTooHigh_ThrowsTimeoutException()
        {
            //arrange
            ConcreteResolver sut = ConcreteResolverSettings
                .Empty
                .Register<ParentThatCreatesManyChildren.ChildActor>()
                .CreateResolver(this);

            //act
            Action act = () => sut.CreateSut<ParentThatCreatesManyChildren.ParentActor>(
                Props.Create(() => new ParentThatCreatesManyChildren.ParentActor(3)),
                4);

            //assert
            act.ShouldThrow<TimeoutException>();
        }

        [Test]
        public void ConcreteResolverTests_CreateSut_ChildIsNotRegisteredInSettings_ThrowsTimeoutException()
        {
            //arrange
            ConcreteResolver sut = ConcreteResolverSettings
                .Empty
                .CreateResolver(this);

            //act
            Action act = () => sut.CreateSut<ChildWithoutDependancy.ParentActor>(Props.Create<ChildWithoutDependancy.ParentActor>());

            //assert
            act.ShouldThrow<TimeoutException>();
            //Akka 'hides' the InvalidOperationException that is thrown from the Resolver
            //But the latch is not counted down so a timeout occurs
        }
    }
}