using System;
using System.Threading;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.NUnit3;
using ConnelHooley.AkkaTestingHelpers.DI.DependancyResolver;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Settings = ConnelHooley.AkkaTestingHelpers.DI.DependancyResolver.Settings;

//These tests are horrible. Sorry.

namespace AkkaTestingHelpers.DI.Tests.DependancyResolverTests
{
    public class ResolverTests : TestKit {

        public ResolverTests() : base(@"akka.test.timefactor = 0.6") { }

        [Test]
        public void InjectChildWithOutParameterlessConstructor()
        {
            //arrange
            int count = 0;
            Mock<ICounter> counterMock = new Mock<ICounter>();
            counterMock
                .Setup(counter => counter.Count())
                .Callback(() => count++);
            Resolver sut = Settings
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
        public void InjectChildWithParameterlessConstructor()
        {
            //arrange
            Resolver sut = Settings
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
        public void WaitForMultipleChildren()
        {
            //arrange
            Resolver sut = Settings
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
        public void ThrowExceptionWhenTooFewChildrenAreCreated()
        {
            //arrange
            Resolver sut = Settings
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
        public void ThrowExceptionWhenChildTypeIsNotRegisted()
        {
            //arrange
            Resolver sut = Settings
                .Empty
                .CreateResolver(this);

            //act
            Action act = () => sut.CreateSut<ParentActor2>(Props.Create<ParentActor2>());

            //assert
            act.ShouldThrow<TimeoutException>();
            //Akka 'hides' the InvalidOperationException that is thrown from the Resolver
            //But the latch is not counted down so a timeout occurs
        }

        [Test]
        public void WaitForChildrenAfterConstruction()
        {
            //arrange
            Resolver sut = Settings
                .Empty
                .Register<ChildActor4>()
                .CreateResolver(this);
            TestActorRef<ParentActor4> actor = ActorOfAsTestActorRef<ParentActor4>();
            
            //act
            sut.WaitForChildren(() => actor.Tell(3), 3);
            
            //assert
            ChildActor4.Count.Should().Be(3);
        }

        [Test]
        public void WaitForChildrenAfterConstructionThrowsWhenTooFewChildrenAreCreated()
        {
            //arrange
            Resolver sut = Settings
                .Empty
                .Register<ChildActor4>()
                .CreateResolver(this);
            TestActorRef<ParentActor4> actor = ActorOfAsTestActorRef<ParentActor4>();

            //act
            Action act = () => sut.WaitForChildren(() => actor.Tell(2), 3);

            //assert
            act.ShouldThrow<TimeoutException>();
        }
    }
}
