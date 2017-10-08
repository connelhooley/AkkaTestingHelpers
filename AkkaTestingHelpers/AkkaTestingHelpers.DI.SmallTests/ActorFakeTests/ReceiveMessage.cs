//using System;
//using System.Collections.Immutable;
//using Akka.Actor;
//using Akka.TestKit;
//using ConnelHooley.AkkaTestingHelpers.DI.Actors.Abstract;
//using ConnelHooley.AkkaTestingHelpers.DI.Actors.Concrete;
//using Xunit;

//// ReSharper disable SuspiciousTypeConversion.Global

//namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.FakeActorTests
//{
//    public class ReceiveMessage : TestBase
//    {
//        [Fact]
//        public void FakeActor_ReceiveMessage_ForwardsMessagesToProbe()
//        {
//            //arrange
//            TestActorRef<FakeActor> sut = ActorOfAsTestActorRef<FakeActor>(
//                Props.Create(() => new FakeActor(ImmutableDictionary<Type, Action<object>>.Empty)));
//            ((IRegisterableFakeActor) sut.UnderlyingActor).RegisterActor(this);
//            Akka.TestKit.TestProbe sender = CreateTestProbe();
//            object message = TestUtils.Create<object>();

//            //act
//            sut.Tell(message, sender);

//            //assert
//            sut.UnderlyingActor.TestProbe.ExpectMsgFrom(sender, message);
//        }

//        [Fact]
//        public void FakeActor_ReceiveMessage_CallsUnregisteredHandler()
//        {
//            //arrange
//            TestActorRef<FakeActor> sut = ActorOfAsTestActorRef<FakeActor>(
//                Props.Create(() => new FakeActor(ImmutableDictionary<Type, Action<object>>.Empty)));
//            IRegisterableFakeActor s = sut.UnderlyingActor;
//            s.RegisterActor(this);
//            Akka.TestKit.TestProbe sender = CreateTestProbe();
//            object message = TestUtils.Create<object>();

//            //act
//            sut.Tell(message, sender);

//            //assert
//            sut.UnderlyingActor.TestProbe.ExpectMsgFrom(sender, message);
//        }
//    }
//}