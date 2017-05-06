using System;
using System.Threading;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.NUnit3;
using ConnelHooley.AkkaTestingHelpers.DI.ConcreteResolver;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace AkkaTestingHelpers.DI.Tests.ConcreteResolverTests
{
    public class CreateSut : TestKit
    {
        public CreateSut() : base(@"akka.test.timefactor = 0.6") { }

        [Test]
        public void ConcreteResolverTests_CreateSut_SettingsWithFactoryFunc_ChildIsInjectedSuccessfully()
        {
            //arrange
            int count = 0;
            Mock<ICounter> counterMock = new Mock<ICounter>();
            counterMock
                .Setup(counter => counter.Count())
                .Callback(() => count++);
            ConcreteResolver sut = ConcreteResolverSettings
                .Empty
                .Register(() => new ChildActor1(counterMock.Object))
                .CreateResolver(this);

            //act
            TestActorRef<ParentActor1> rootActor = sut.CreateSut<ParentActor1>(
                Props.Create(() => new ParentActor1()));

            //assert
            rootActor.Tell(Guid.NewGuid());
            rootActor.Tell(Guid.NewGuid());
            Thread.Sleep(200); //evil
            count.Should().Be(2);
        }

        [Test]
        public void ConcreteResolverTests_CreateSut_SettingsWithGeneric_ChildIsInjectedSuccessfully()
        {
            //arrange
            ConcreteResolver sut = ConcreteResolverSettings
                .Empty
                .Register<ChildActor2>()
                .CreateResolver(this);

            //act
            TestActorRef<ParentActor2> rootActor = sut.CreateSut<ParentActor2>(
                Props.Create<ParentActor2>());

            //assert
            rootActor.Tell(Guid.NewGuid());
            rootActor.Tell(Guid.NewGuid());
            Thread.Sleep(200); //evil
            ChildActor2.Count.Should().Be(2);
        }

        [Test]
        public void ConcreteResolverTests_CreateSut_ParentThatCreatesMultipleChildren_MethodOnlyReturnsWhenAllChildrenAreCreated()
        {
            //arrange
            ConcreteResolver sut = ConcreteResolverSettings
                .Empty
                .Register<ChildActor3>()
                .CreateResolver(this);

            //act
            TestActorRef<ParentActor3> rootActor = sut.CreateSut<ParentActor3>(
                Props.Create<ParentActor3>(),
                3);

            //assert
            rootActor.Tell(Guid.NewGuid());
            Thread.Sleep(200); //evil
            ChildActor3.Count.Should().Be(1);
        }

        [Test]
        public void ConcreteResolverTests_CreateSut_ExpectedParentCountIsTooLow_ThrowsTimeoutException()
        {
            //arrange
            ConcreteResolver sut = ConcreteResolverSettings
                .Empty
                .Register<ChildActor3>()
                .CreateResolver(this);

            //act
            Action act = () =>
                sut.CreateSut<ParentActor3>(
                    Props.Create<ParentActor3>(),
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
            Action act = () => sut.CreateSut<ParentActor2>(Props.Create<ParentActor2>());

            //assert
            act.ShouldThrow<TimeoutException>();
            //Akka 'hides' the InvalidOperationException that is thrown from the Resolver
            //But the latch is not counted down so a timeout occurs
        }
    }
}