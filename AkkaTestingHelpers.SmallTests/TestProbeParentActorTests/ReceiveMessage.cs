using System;
using System.Collections.Generic;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;
using ConnelHooley.TestHelpers;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.TestProbeParentActorTests
{
    public class ReceiveMessage : TestBase
    {
        [Fact]
        public void TestProbeParentActorWithHandlers_ReceivesMessageWithMatchingHandler_SendsCorrectReplies()
        {
            //arrange
            TestActorRef<TestProbeParentActor> sut = CreateTestProbeParentActor();

            //act
            sut.Tell(ParentMessage2);

            //assert
            ExpectMsg(ParentReply2);
        }

        [Fact]
        public void TestProbeParentActorWithHandlers_ReceivesMessageWithMatchingHandler_TestProbeIsForwardedMessages()
        {
            //arrange
            TestActorRef<TestProbeParentActor> sut = CreateTestProbeParentActor();

            //act
            sut.Tell(ParentMessage2);

            //assert
            sut.UnderlyingActor.TestProbe.ExpectMsg(ParentMessage2);
        }

        [Fact]
        public void TestProbeParentActorWitouthHandlers_ReceivesMessage_DoesNotReply()
        {
            //arrange
            Handlers = new Dictionary<Type, Func<object, object>>();
            TestActorRef<TestProbeParentActor> sut = CreateTestProbeParentActor();

            //act
            sut.Tell(ParentMessage2);

            //assert
            ExpectNoMsg();
        }
        
        [Fact]
        public void TestProbeParentActorWithHandlers_ReceivesMessageWithNoMatchingHandler_DoesNotReply()
        {
            //arrange
            TestActorRef<TestProbeParentActor> sut = CreateTestProbeParentActor();

            //act
            sut.Tell(TestHelper.Generate<object>());

            //assert
            ExpectNoMsg();
        }
        
        [Fact]
        public void TestProbeParentActorWithHandlers_ReceivesMessageWithMatchingHandler_RepliesToSubsequentMessages()
        {
            //arrange
            TestActorRef<TestProbeParentActor> sut = CreateTestProbeParentActor();
            sut.Tell(ParentMessage1);

            //act
            sut.Tell(ParentMessage2);

            //assert
            ExpectMsgAllOf(ParentReply1, ParentReply2);
        }
        
        [Fact]
        public void TestProbeParentActorWithHandlers_ReceivesMessageWitNohMatchingHandler_RepliesToSubsequentMessages()
        {
            //arrange
            TestActorRef<TestProbeParentActor> sut = CreateTestProbeParentActor();
            sut.Tell(TestHelper.Generate<object>());

            //act
            sut.Tell(ParentMessage2);

            //assert
            ExpectMsg(ParentReply2);
        }
    }
}