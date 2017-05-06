using System;
using Akka.TestKit;
using Akka.TestKit.NUnit3;
using ConnelHooley.AkkaTestingHelpers.DI.ConcreteResolver;
using FluentAssertions;
using NUnit.Framework;

namespace AkkaTestingHelpers.DI.Tests.ConcreteResolverTests
{
    public class WaitForChildren : TestKit
    {
        public WaitForChildren() : base(@"akka.test.timefactor = 0.6") { }
        
        [Test]
        public void ConcreteResolverTests_WaitForChildren_ExpectedChildCountIsCorrect_MethodOnlyReturnsWhenChildrenHaveBeenCreated()
        {
            //arrange
            ConcreteResolver sut = ConcreteResolverSettings
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
        public void ConcreteResolverTests_WaitForChildren_ExpectedChildCountIsTooLow_ThrowsTimeoutException()
        {
            //arrange
            ConcreteResolver sut = ConcreteResolverSettings
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