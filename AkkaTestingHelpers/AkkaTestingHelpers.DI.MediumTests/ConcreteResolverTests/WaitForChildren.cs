using System;
using System.Linq;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.NUnit3;
using FluentAssertions;
using NUnit.Framework;

namespace ConnelHooley.AkkaTestingHelpers.DI.MediumTests.ConcreteResolverTests
{
    internal class WaitForChildren : TestKit
    {
        public WaitForChildren(): base(AkkaConfig.Config) { }

        [Test]
        public void ConcreteResolver_WaitsForChildrenCreatedWhenProcessingMessages()
        {
            //arrange
            const int initialChildCount = 2;
            const int moreChildCount = 5;
            ConcreteResolver sut = ConcreteResolverSettings
                .Empty
                .Register<EmptyChildActor>()
                .Register<ChildActor>()
                .CreateResolver(this);
            TestActorRef<ParentActor> actor = sut.CreateSut<ParentActor>(Props.Create(() => new ParentActor(initialChildCount)), initialChildCount);

            //act
            sut.TellMessage(actor, moreChildCount, moreChildCount);

            //assert
            actor.Tell(new object());
            ExpectMsgAllOf(Enumerable.Repeat(ChildActor.Token, initialChildCount + moreChildCount).ToArray());
        }

        [Test]
        public void ConcreteResolver_TimesoutWhenWaitingForChildrenWithAnExpectedChildCountThatIsTooHigh()
        {
            //arrange
            const int initialChildCount = 2;
            const int moreChildCount = 5;
            ConcreteResolver sut = ConcreteResolverSettings
                .Empty
                .Register<EmptyChildActor>()
                .Register<ChildActor>()
                .CreateResolver(this);
            TestActorRef<ParentActor> actor = sut.CreateSut<ParentActor>(Props.Create(() => new ParentActor(initialChildCount)), initialChildCount);

            //act
            Action act = () => sut.TellMessage(actor, moreChildCount, moreChildCount + 1);

            //assert
            act.ShouldThrow<TimeoutException>();
        }
    }
}