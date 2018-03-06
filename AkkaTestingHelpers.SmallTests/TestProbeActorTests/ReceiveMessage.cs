using System;
using System.Collections.Generic;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;
using ConnelHooley.TestHelpers;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.TestProbeActorTests
{
    public class ReceiveMessage : TestBase
    {
        [Fact]
        public void TestProbeActorWithHandlers_ReceivesMessageWithMatchingHandler_SendsCorrectReplies()
        {
            //arrange
            TestActorRef<TestProbeActor> sut = CreateTestProbeActorWithoutSupervisorStrategy();

            //act
            sut.Tell(Message2);

            //assert
            ExpectMsg(Reply2);
        }

        [Fact]
        public void TestProbeActorWithHandlers_ReceivesMessageWithMatchingHandler_TestProbeIsForwardedMessages()
        {
            //arrange
            TestActorRef<TestProbeActor> sut = CreateTestProbeActorWithoutSupervisorStrategy();

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
            TestActorRef<TestProbeActor> sut = CreateTestProbeActorWithoutSupervisorStrategy();

            //act
            sut.Tell(Message2);

            //assert
            ExpectNoMsg();
        }
        
        [Fact]
        public void TestProbeActorWithHandlers_ReceivesMessageWithNoMatchingHandler_DoesNotReply()
        {
            //arrange
            TestActorRef<TestProbeActor> sut = CreateTestProbeActorWithoutSupervisorStrategy();

            //act
            sut.Tell(TestHelper.Generate<object>());

            //assert
            ExpectNoMsg();
        }
        
        [Fact]
        public void TestProbeActorWithHandlers_ReceivesMessageWithMatchingHandler_RepliesToSubsequentMessages()
        {
            //arrange
            TestActorRef<TestProbeActor> sut = CreateTestProbeActorWithoutSupervisorStrategy();
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
            TestActorRef<TestProbeActor> sut = CreateTestProbeActorWithoutSupervisorStrategy();
            sut.Tell(TestHelper.Generate<object>());

            //act
            sut.Tell(Message2);

            //assert
            ExpectMsg(Reply2);
        }
    }
}