//using System;
//using System.Linq;
//using Akka.Actor;
//using Akka.TestKit;
//using Akka.TestKit.Xunit2;
//using FluentAssertions;
//using Xunit;

//namespace ConnelHooley.AkkaTestingHelpers.DI.MediumTests.ConcreteResolverTests
//{
//    public class WaitForChildren : TestKit
//    {
//        public WaitForChildren(): base(AkkaConfig.Config) { }

//        [Fact]
//        public void ConcreteResolver_WaitsForChildrenCreatedWhenProcessingMessages()
//        {
//            //arrange
//            const int initialChildCount = 2;
//            const int moreChildCount = 5;
//            ConcreteResolver sut = ConcreteResolverSettings
//                .Empty
//                .Register<EmptyChildActor>()
//                .Register<ChildActor>()
//                .CreateResolver(this);
//            TestActorRef<ParentActor> actor = sut.CreateSut<ParentActor>(Props.Create(() => new ParentActor(initialChildCount)), initialChildCount);

//            //act
//            sut.TellMessage(actor, moreChildCount, moreChildCount);

//            //assert
//            actor.Tell(new object());
//            ExpectMsgAllOf(Enumerable.Repeat(ChildActor.Token, initialChildCount + moreChildCount).ToArray());
//        }

//        [Fact]
//        public void ConcreteResolver_TimesoutWhenWaitingForChildrenWithAnExpectedChildCountThatIsTooHigh()
//        {
//            //arrange
//            const int initialChildCount = 2;
//            const int moreChildCount = 5;
//            ConcreteResolver sut = ConcreteResolverSettings
//                .Empty
//                .Register<EmptyChildActor>()
//                .Register<ChildActor>()
//                .CreateResolver(this);
//            TestActorRef<ParentActor> actor = sut.CreateSut<ParentActor>(Props.Create(() => new ParentActor(initialChildCount)), initialChildCount);

//            //act
//            Action act = () => sut.TellMessage(actor, moreChildCount, moreChildCount + 1);

//            //assert
//            act.ShouldThrow<TimeoutException>();
//        }
//    }
//}