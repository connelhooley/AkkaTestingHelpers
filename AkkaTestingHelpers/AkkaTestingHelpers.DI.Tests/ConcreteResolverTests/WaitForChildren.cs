using System;
using Akka.Actor;
using Akka.TestKit;
using FluentAssertions;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace ConnelHooley.AkkaTestingHelpers.DI.Tests.ConcreteResolverTests
{
    public class WaitForChildren : TestBase
    {
        [Test]
        public void ConcreteResolverTests_WaitForChildren_ExpectedChildCountIsCorrect_MethodOnlyReturnsWhenChildrenHaveBeenCreated()
        {
            //arrange
            ConcreteResolver sut = ConcreteResolverSettings
                .Empty
                .Register<ReplyChildActor>()
                .CreateResolver(this);
            TestActorRef<ParentActor<ReplyChildActor>> rootActor = 
                sut.CreateSut<ParentActor<ReplyChildActor>>(
                    Props.Create<ParentActor<ReplyChildActor>>(), 
                    0);
            const int childCount = 3;

            //act
            sut.WaitForChildren(
                () => rootActor.Tell(Fixture.CreateMany<string>(childCount)), 
                childCount);

            //assert
            rootActor.Tell(Fixture.Create<object>());
            ReceiveN(childCount);
        }

        [Test]
        public void ConcreteResolverTests_WaitForChildren_ExpectedChildCountIsTooHigh_ThrowsTimeoutException()
        {
            //arrange
            ConcreteResolver sut = ConcreteResolverSettings
                .Empty
                .Register<ReplyChildActor>()
                .CreateResolver(this);
            TestActorRef<ParentActor<ReplyChildActor>> rootActor =
                sut.CreateSut<ParentActor<ReplyChildActor>>(
                    Props.Create<ParentActor<ReplyChildActor>>(),
                    0);
            const int childCount = 3;

            //act
            Action act = () => sut.WaitForChildren(
                () => rootActor.Tell(Fixture.CreateMany<string>(childCount)),
                childCount+1);

            //assert
            act.ShouldThrow<TimeoutException>();
        }
    }
}