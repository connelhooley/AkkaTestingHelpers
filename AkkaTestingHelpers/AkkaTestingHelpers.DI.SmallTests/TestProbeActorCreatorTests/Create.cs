using System;
using System.Collections.Immutable;
using Akka.Actor;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;
using FluentAssertions;
using NUnit.Framework;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.TestProbeActorCreatorTests
{
    internal class Create : TestBase
    {
        //[Test]
        //public void CreateTestProbeActorFactory_CreateWithNullActorKitBase_ThrowsArgumentNullException()
        //{
        //    //arrange
        //    TestProbeActorCreator sut = CreateTestProbeActorFactory();

        //    //act
        //    Action act = () => sut.Create(null, ImmutableDictionary<Type, Func<object, object>>.Empty);

        //    //assert
        //    act.ShouldThrow<ArgumentNullException>();
        //}

        //[Test]
        //public void CreateTestProbeActorFactory_CreateWithNullHandlers_DoesNotThrow()
        //{
        //    //arrange
        //    TestProbeActorCreator sut = CreateTestProbeActorFactory();

        //    //act
        //    Action act = () => sut.Create(this, null);

        //    //assert
        //    act.ShouldNotThrow();
        //}

        //[Test]
        //public void CreateTestProbeActorFactory_CreateWithNullHandlers_ReturnsActorThatDoesNotReply()
        //{
        //    //arrange
        //    TestProbeActorCreator sut = CreateTestProbeActorFactory();

        //    //act
        //    TestProbeActor result = sut.Create(null, null);

        //    //assert
        //    var actorRef = ActorOf(Props.Create(() => result));
        //    actorRef.Tell(TestUtils.Create<object>());
        //    ExpectNoMsg(100);
        //}

        [Test]
        public void CreateTestProbeActorFactory_CreateWithHandlersHandlers_ReturnsActorThatDoesReply()
        {
            ////arrange
            //TestProbeActorCreator sut = CreateTestProbeActorFactory();

            ////act
            ////TestProbeActor result = sut.Create(
            ////    this, 
            ////     ActorType,
            ////    ImmutableDictionary<Type, ImmutableDictionary<Type, Func<object, object>>>
            ////        .Empty
            ////        .Add());

            ////assert
            //var actorRef = ActorOf(Props.Create(() => result));
            //actorRef.Tell(TestUtils.Create<object>());
            //ExpectNoMsg(100);
        }
    }
}