using System;
using System.Collections.Generic;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.TestProbeActorTests
{
    public class ReceiveMessage : TestBase
    {
        [Fact]
        public void TestProbeActorWithHandlers_ReceivesMessageWithMatchingHandler_SendsCorrectReplies()
        {
            //arrange
            TestActorRef<TestProbeActor> sut = CreateTestProbeActor();

            //act
            sut.Tell(Message2);

            //assert
            ExpectMsg(Reply2);
        }

        [Fact]
        public void TestProbeActorWithHandlers_ReceivesMessageWithMatchingHandler_TestProbeIsForwardedMessages()
        {
            //arrange
            TestActorRef<TestProbeActor> sut = CreateTestProbeActor();

            //act
            sut.Tell(Message2);

            //assert
            sut.UnderlyingActor.TestProbe.ExpectMsg(Message2);
        }

        [Fact]
        public void TestProbeActorWitouthHandlers_ReceivesMessage_DoesNotReply()
        {
            //arrange
            Handlers = new Dictionary<Type, Func<object, object>>();
            TestActorRef<TestProbeActor> sut = CreateTestProbeActor();

            //act
            sut.Tell(Message2);

            //assert
            ExpectNoMsg();
        }
        
        [Fact]
        public void TestProbeActorWithHandlers_ReceivesMessageWithNoMatchingHandler_DoesNotReply()
        {
            //arrange
            TestActorRef<TestProbeActor> sut = CreateTestProbeActor();

            //act
            sut.Tell(TestUtils.Create<object>());

            //assert
            ExpectNoMsg();
        }
        
        [Fact]
        public void TestProbeActorWithHandlers_ReceivesMessageWithMatchingHandler_RepliesToSubsequentMessages()
        {
            //arrange
            TestActorRef<TestProbeActor> sut = CreateTestProbeActor();
            sut.Tell(Message1);

            //act
            sut.Tell(Message2);

            //assert
            ExpectMsgAllOf(Reply1, Reply2);
        }
        
        [Fact]
        public void TestProbeActorWithHandlers_ReceivesMessageWitNohMatchingHandler_RepliesToSubsequentMessages()
        {
            //arrange
            TestActorRef<TestProbeActor> sut = CreateTestProbeActor();
            sut.Tell(TestUtils.Create<object>());

            //act
            sut.Tell(Message2);

            //assert
            ExpectMsg(Reply2);
        }
    }
}