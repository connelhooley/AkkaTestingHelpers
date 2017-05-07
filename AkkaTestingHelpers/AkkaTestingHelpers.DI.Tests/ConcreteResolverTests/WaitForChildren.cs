using System;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using static ConnelHooley.AkkaTestingHelpers.DI.Tests.ParentThatCreatesManyChildren;

namespace ConnelHooley.AkkaTestingHelpers.DI.Tests.ConcreteResolverTests
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
                .Register<ChildActor>()
                .CreateResolver(this);
            TestActorRef<ParentActor> rootActor = sut.CreateSut<ParentActor>(Props.Create(() => new ParentActor(0)), 0);

            //act
            sut.WaitForChildren(() => rootActor.Tell(3), 3);

            //assert
            foreach (IActorRef child in rootActor.UnderlyingActor.Children)
            {
                child.Tell(new { });
            }
            ReceiveN(3);
        }

        [Test]
        public void ConcreteResolverTests_WaitForChildren_ExpectedChildCountIsTooHigh_ThrowsTimeoutException()
        {
            //arrange
            ConcreteResolver sut = ConcreteResolverSettings
                .Empty
                .Register<ChildActor>()
                .CreateResolver(this);
            TestActorRef<ParentActor> rootActor = sut.CreateSut<ParentActor>(Props.Create(() => new ParentActor(0)), 0);

            //act
            Action act = () => sut.WaitForChildren(() => rootActor.Tell(2), 3);

            //assert
            act.ShouldThrow<TimeoutException>();
        }
    }
}