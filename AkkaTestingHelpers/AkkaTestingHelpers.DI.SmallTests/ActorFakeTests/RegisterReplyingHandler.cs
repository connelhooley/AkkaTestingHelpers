//using System;
//using ConnelHooley.AkkaTestingHelpers.DI.Actors.Concrete;
//using FluentAssertions;
//using Xunit;

//namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.FakeActorTests
//{
//    public class RegisterReplyingHandler : TestBase
//    {
//        [Fact]
//        public void FakeActor_RegisterReplyingHandler_ReturnsNewInstance()
//        {
//            //arrange
//            FakeActor sut = FakeActor.Create();

//            //act
//            FakeActor result = sut.RegisterReplyingHandler(TestUtils.Create<Func<string, object>>());

//            //assert
//            result.Should().NotBeSameAs(sut);
//        }
        
//        [Fact]
//        public void FakeActor_RegisterDuplicateReplyingHandler_ReturnsNewInstance()
//        {
//            //arrange
//            FakeActor sut = FakeActor
//                .Create()
//                .RegisterReplyingHandler(TestUtils.Create<Func<string, object>>());

//            //act
//            FakeActor result = sut.RegisterReplyingHandler(TestUtils.Create<Func<string, object>>());

//            //assert
//            result.Should().NotBeSameAs(sut);
//        }
//    }
//}