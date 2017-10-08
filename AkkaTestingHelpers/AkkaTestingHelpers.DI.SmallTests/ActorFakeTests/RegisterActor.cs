//using System;
//using ConnelHooley.AkkaTestingHelpers.DI.Actors.Abstract;
//using ConnelHooley.AkkaTestingHelpers.DI.Actors.Concrete;
//using FluentAssertions;
//using Xunit;

//namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.FakeActorTests
//{
//    public class RegisterActor : TestBase
//    {
//        [Fact]
//        public void FakeActor_RegisterActorWithNoHandlers_ThrowsArgumentNullException()
//        {
//            //arrange
//            IRegisterableFakeActor sut = FakeActor.Create();

//            //act
//            Action act = () => sut.RegisterActor(null);

//            //assert
//            act.ShouldThrow<ArgumentNullException>();
//        }

//        [Fact]
//        public void FakeActor_RegisterActorWithNoHandlers_DoesNotThrow()
//        {
//            //arrange
//            IRegisterableFakeActor sut = FakeActor.Create();
            
//            //act
//            Action act = () => sut.RegisterActor(this);

//            //assert
//            act.ShouldNotThrow();
//        }

//        [Fact]
//        public void FakeActor_RegisterActorWithNonReplyingHandlers_DoesNotThrow()
//        {
//            //arrange
//            IRegisterableFakeActor sut = FakeActor
//                .Create()
//                .RegisterNonReplyingHandler(TestUtils.Create<Action<string>>())
//                .RegisterNonReplyingHandler(TestUtils.Create<Action<int>>());

//            //act
//            Action act = () => sut.RegisterActor(this);

//            //assert
//            act.ShouldNotThrow();
//        }
        
//        [Fact]
//        public void FakeActor_RegisterActorWithDuplicateNonReplyingHandlers_DoesNotThrow()
//        {
//            //arrange
//            IRegisterableFakeActor sut = FakeActor
//                .Create()
//                .RegisterNonReplyingHandler(TestUtils.Create<Action<string>>())
//                .RegisterNonReplyingHandler(TestUtils.Create<Action<string>>());

//            //act
//            Action act = () => sut.RegisterActor(this);

//            //assert
//            act.ShouldNotThrow();
//        }

//        [Fact]
//        public void FakeActor_RegisterActorWithReplyingHandlers_DoesNotThrow()
//        {
//            //arrange
//            IRegisterableFakeActor sut = FakeActor
//                .Create()
//                .RegisterReplyingHandler(TestUtils.Create<Func<string, object>>())
//                .RegisterReplyingHandler(TestUtils.Create<Func<int, object>>());

//            //act
//            Action act = () => sut.RegisterActor(this);

//            //assert
//            act.ShouldNotThrow();
//        }

//        [Fact]
//        public void FakeActor_RegisterActorWithDuplicateReplyingHandlers_DoesNotThrow()
//        {
//            //arrange
//            IRegisterableFakeActor sut = FakeActor
//                .Create()
//                .RegisterReplyingHandler(TestUtils.Create<Func<string, object>>())
//                .RegisterReplyingHandler(TestUtils.Create<Func<string, object>>());

//            //act
//            Action act = () => sut.RegisterActor(this);

//            //assert
//            act.ShouldNotThrow();
//        }

//        [Fact]
//        public void FakeActor_RegisterActorWithNonReplyingAndReplyingHandlers_DoesNotThrow()
//        {
//            //arrange
//            IRegisterableFakeActor sut = FakeActor
//                .Create()
//                .RegisterNonReplyingHandler(TestUtils.Create<Action<string>>())
//                .RegisterNonReplyingHandler(TestUtils.Create<Action<int>>())
//                .RegisterReplyingHandler(TestUtils.Create<Func<string, object>>())
//                .RegisterReplyingHandler(TestUtils.Create<Func<int, object>>());

//            //act
//            Action act = () => sut.RegisterActor(this);

//            //assert
//            act.ShouldNotThrow();
//        }
//    }
//}