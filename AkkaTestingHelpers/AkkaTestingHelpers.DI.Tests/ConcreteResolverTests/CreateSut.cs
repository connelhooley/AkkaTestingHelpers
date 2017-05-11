using System;
using System.Linq;
using Akka.Actor;
using Akka.TestKit;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace ConnelHooley.AkkaTestingHelpers.DI.Tests.ConcreteResolverTests
{
    public class CreateSut : TestBase
    {
        [Test]
        public void ConcreteResolver_CreateSut_SettingsWithFactoryFunc_ChildIsInjected()
        {
            //arrange
            Mock<IDependancy> dependancyMock = new Mock<IDependancy>();
            ConcreteResolver sut = ConcreteResolverSettings
                .Empty
                .Register(() => new DependancyChildActor(dependancyMock.Object))
                .CreateResolver(this);
            object message = Fixture.Create<object>();

            //act
            TestActorRef <ParentActor<DependancyChildActor>> rootActor = 
                sut.CreateSut<ParentActor<DependancyChildActor>>(
                    Props.Create(() => new ParentActor<DependancyChildActor>(Fixture.Create<string>())));

            //assert
            rootActor.Tell(message);
            dependancyMock.Verify(dependancy => dependancy.SetResut(It.Is<object>(o => o == message)));
        }

        [Test]
        public void ConcreteResolver_CreateSut_SettingsWithGeneric_ChildIsInjected()
        {
            //arrange
            ConcreteResolver sut = ConcreteResolverSettings
                .Empty
                .Register<ReplyChildActor>()
                .CreateResolver(this);
            object message = Fixture.Create<object>();

            //act
            TestActorRef<ParentActor<ReplyChildActor>> rootActor = 
                sut.CreateSut<ParentActor<ReplyChildActor>>(
                    Props.Create(() => new ParentActor<ReplyChildActor>(Fixture.Create<string>())));

            //assert
            rootActor.Tell(message);
            ExpectMsg<object>().Should().BeSameAs(message);
        }

        [Test]
        public void ConcreteResolver_CreateSut_NoProps_ParentIsCreated()
        {
            //arrange
            ConcreteResolver sut = ConcreteResolverSettings
                .Empty
                .Register<ReplyChildActor>()
                .CreateResolver(this);

            //act
            TestActorRef<ParentActor<ReplyChildActor>> rootActor = 
                sut.CreateSut<ParentActor<ReplyChildActor>>(0);

            //assert
            rootActor.UnderlyingActor.Should().BeOfType<ParentActor<ReplyChildActor>>();
        }

        [Test]
        public void ConcreteResolver_CreateSut_ParentThatCreatesMultipleChildren_AllChildrenAreInjected()
        {
            //arrange
            ConcreteResolver sut = ConcreteResolverSettings
                .Empty
                .Register<ReplyChildActor>()
                .CreateResolver(this);
            const int childCount = 5;

            //act
            TestActorRef<ParentActor<ReplyChildActor>> rootActor =
                sut.CreateSut<ParentActor<ReplyChildActor>>(
                    Props.Create(() => new ParentActor<ReplyChildActor>(Fixture.CreateMany<string>(childCount).ToArray())),
                    childCount);

            //assert
            rootActor.Tell(Fixture.Create<object>());
            ReceiveN(childCount);
        }

        [Test]
        public void ConcreteResolver_CreateSut_ExpectedParentCountIsTooHigh_ThrowsTimeoutException()
        {
            //arrange
            ConcreteResolver sut = ConcreteResolverSettings
                .Empty
                .Register<ReplyChildActor>()
                .CreateResolver(this);
            const int childCount = 5;

            //act
            Action act = () => 
                sut.CreateSut<ParentActor<ReplyChildActor>>(
                    Props.Create(() => new ParentActor<ReplyChildActor>(Fixture.CreateMany<string>(childCount).ToArray())),
                    childCount+1);

            //assert
            act.ShouldThrow<TimeoutException>();
        }

        [Test]
        public void ConcreteResolver_CreateSut_ChildIsNotRegisteredInSettings_ThrowsTimeoutException()
        {
            //arrange
            ConcreteResolver sut = ConcreteResolverSettings
                .Empty
                .CreateResolver(this);

            //act
            Action act = () => sut.CreateSut<ParentActor<ReplyChildActor>>(
                Props.Create(() => new ParentActor<ReplyChildActor>()));

            //assert
            act.ShouldThrow<TimeoutException>();
            //Akka 'hides' the InvalidOperationException that is thrown from the Resolver
            //But the latch is not counted down so a timeout occurs
        }
    }
}