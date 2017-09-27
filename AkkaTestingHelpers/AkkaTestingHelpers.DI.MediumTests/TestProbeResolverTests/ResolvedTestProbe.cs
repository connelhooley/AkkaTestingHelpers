//using System;
//using Akka.Actor;
//using Akka.TestKit;
//using Akka.TestKit.Xunit2;
//using Akka.TestKit.TestActors;
//using FluentAssertions;
//using Xunit;

//namespace ConnelHooley.AkkaTestingHelpers.DI.MediumTests.TestProbeResolverTests
//{
//    public class ResolvedTestProbe : TestKit
//    {
//        public ResolvedTestProbe() : base(AkkaConfig.Config) { }

//        [Fact]
//        public void TestProbeResolver_ResolvedTestProbesAreStored()
//        {
//            //arrange
//            Type childType = typeof(BlackHoleActor);
//            string childName = Guid.NewGuid().ToString();
//            Guid message = Guid.NewGuid();
//            TestProbeResolver sut = TestProbeResolverSettings
//                .Empty
//                .CreateResolver(this);

//            //act
//            TestActorRef<ParentActor> actor = sut.CreateSut<ParentActor>(Props.Create(() => new ParentActor()), 0);
//            sut.TellMessage(actor, new CreateChild(childName, childType), 1);
//            TestProbe result = sut.ResolvedTestProbe(actor, childName);

//            //assert
//            actor.Tell(new TellChild(childName, message));
//            result.ExpectMsg(message);
//        }

//        [Fact]
//        public void TestProbeResolver_ThrownsWhenChildHasNotBeenResolved()
//        {
//            //arrange
//            TestProbeResolver sut = TestProbeResolverSettings
//                .Empty
//                .CreateResolver(this);

//            //act
//            TestActorRef<ParentActor> actor = sut.CreateSut<ParentActor>(Props.Create(() => new ParentActor()), 0);
//            sut.TellMessage(actor, new CreateChild(Guid.NewGuid().ToString(), typeof(EchoActor)), 1);
//            Action act = () => sut.ResolvedTestProbe(actor, Guid.NewGuid().ToString());

//            //assert
//            act.ShouldThrow<ActorNotFoundException>();
//        }
//    }
//}