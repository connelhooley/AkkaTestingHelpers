using System;
using System.Collections.Generic;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;
using ConnelHooley.TestHelpers;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.TestProbeChildActorTests
{
    public class ReceiveMessage : TestBase
    {
        [Fact]
        public void TestProbeChildActorWithHandlers_ReceivesMessageWithMatchingHandler_SendsCorrectReplies()
        {
            //arrange
            TestActorRef<TestProbeChildActor> sut = CreateTestProbeChildActorWithoutSupervisorStrategy();

            //act
            sut.Tell(Message2);

            //assert
            ExpectMsg(Reply2);
        }

        [Fact]
        public void TestProbeChildActorWithHandlers_ReceivesMessageWithMatchingHandler_TestProbeIsForwardedMessages()
        {
            //arrange
            TestActorRef<TestProbeChildActor> sut = CreateTestProbeChildActorWithoutSupervisorStrategy();

            //act
            sut.Tell(Message2);

            //assert
            sut.UnderlyingActor.TestProbe.ExpectMsg(Message2);
        }

        [Fact]
        public void TestProbeChildActorWitouthHandlers_ReceivesMessage_DoesNotReply()
        {
            //arrange
            Handlers = new Dictionary<Type, Func<object, object>>();
            TestActorRef<TestProbeChildActor> sut = CreateTestProbeChildActorWithoutSupervisorStrategy();

            //act
            sut.Tell(Message2);

            //assert
            ExpectNoMsg();
        }
        
        [Fact]
        public void TestProbeChildActorWithHandlers_ReceivesMessageWithNoMatchingHandler_DoesNotReply()
        {
            //arrange
            TestActorRef<TestProbeChildActor> sut = CreateTestProbeChildActorWithoutSupervisorStrategy();

            //act
            sut.Tell(TestHelper.Generate<object>());

            //assert
            ExpectNoMsg();
        }
        
        [Fact]
        public void TestProbeChildActorWithHandlers_ReceivesMessageWithMatchingHandler_RepliesToSubsequentMessages()
        {
            //arrange
            TestActorRef<TestProbeChildActor> sut = CreateTestProbeChildActorWithoutSupervisorStrategy();
            sut.Tell(Message1);

            //act
            sut.Tell(Message2);

            //assert
            ExpectMsgAllOf(Reply1, Reply2);
        }
        
        [Fact]
        public void TestProbeChildActorWithHandlers_ReceivesMessageWitNohMatchingHandler_RepliesToSubsequentMessages()
        {
            //arrange
            TestActorRef<TestProbeChildActor> sut = CreateTestProbeChildActorWithoutSupervisorStrategy();
            sut.Tell(TestHelper.Generate<object>());

            //act
            sut.Tell(Message2);

            //assert
            ExpectMsg(Reply2);
        }
    }
}