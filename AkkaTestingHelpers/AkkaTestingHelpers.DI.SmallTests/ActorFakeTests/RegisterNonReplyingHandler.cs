//using System;
//using ConnelHooley.AkkaTestingHelpers.DI.Actors.Concrete;
//using FluentAssertions;
//using Xunit;

//namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.FakeActorTests
//{
//    public class RegisterNonReplyingHandler : TestBase
//    {
//        [Fact]
//        public void FakeActor_RegisterNonReplyingHandler_ReturnsNewInstance()
//        {
//            //arrange
//            FakeActor sut = FakeActor.Create();

//            //act
//            FakeActor result = sut.RegisterNonReplyingHandler(TestUtils.Create<Action<string>>());

//            //assert
//            result.Should().NotBeSameAs(sut);
//        }

//        [Fact]
//        public void FakeActor_RegisterDuplicateNonReplyingHandler_ReturnsNewInstance()
//        {
//            //arrange
//            FakeActor sut = FakeActor
//                .Create()
//                .RegisterNonReplyingHandler(TestUtils.Create<Action<string>>());

//            //act
//            FakeActor result = sut.RegisterNonReplyingHandler(TestUtils.Create<Action<string>>());

//            //assert
//            result.Should().NotBeSameAs(sut);
//        }
//    }
//}