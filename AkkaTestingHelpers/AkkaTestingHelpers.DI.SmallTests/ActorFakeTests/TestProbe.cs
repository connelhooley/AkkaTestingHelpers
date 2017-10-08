//using System;
//using ConnelHooley.AkkaTestingHelpers.DI.Actors.Abstract;
//using ConnelHooley.AkkaTestingHelpers.DI.Actors.Concrete;
//using FluentAssertions;
//using Xunit;

//namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.FakeActorTests
//{
//    public class TestProbe : TestBase
//    {
//        [Fact]
//        public void FakeActor_TestProbeBeforeRegister_ThrowsInvalidOperationException()
//        {
//            //arrange
//            FakeActor sut = FakeActor.Create();

//            //act
//            Action act = () =>
//            {
//                Akka.TestKit.TestProbe result = sut.TestProbe;
//            };

//            //assert
//            act
//                .ShouldThrow<InvalidOperationException>()
//                .WithMessage("Fake actor has not been registered by resolver");
//        }

//        [Fact]
//        public void FakeActor_TestProbeAfterRegister_DoesNotThrow()
//        {
//            //arrange
//            FakeActor sut = FakeActor.Create();
//            ((IRegisterableFakeActor)sut).RegisterActor(this);

//            //act
//            Action act = () =>
//            {
//                Akka.TestKit.TestProbe result = sut.TestProbe;
//            };

//            //assert
//            act.ShouldNotThrow();
//        }
//    }
//}