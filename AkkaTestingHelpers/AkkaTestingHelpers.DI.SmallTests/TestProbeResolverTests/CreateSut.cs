using System;
using Akka.Actor;
using Akka.TestKit;
using FluentAssertions;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.TestProbeResolverTests
{
    public class CreateSut : TestBase
    {
        [Test]
        public void TestProbeResolver_CreateSut_NoMessageHandler_ChildIsInjectedWithNonresponsiveProbe()
        {
            //arrange
            TestProbeResolver sut = TestProbeResolverSettings
                .Empty
                .CreateResolver(this);

            //act
            TestActorRef<ParentActor<ReplyChildActor>> rootActor = 
                sut.CreateSut<ParentActor<ReplyChildActor>>(
                    Props.Create(() => new ParentActor<ReplyChildActor>(Fixture.Create<string>())));

            //assert
            rootActor.Tell(Fixture.Create<object>());
            ExpectNoMsg(1000);
        }

        [Test]
        public void TestProbeResolver_CreateSut_MessageHandler_ChildIsInjectedWithResponsiveProbes()
        {
            //arrange
            Guid prefix = Guid.NewGuid();
            TestProbeResolver sut = TestProbeResolverSettings
                .Empty
                .RegisterHandler<ReplyChildActor, Guid>(suffix => $"{prefix} : {suffix}")
                .CreateResolver(this);

            //act
            TestActorRef<ParentActor<ReplyChildActor>> rootActor = 
                sut.CreateSut<ParentActor<ReplyChildActor>>(
                    Props.Create(() => new ParentActor<ReplyChildActor>(Fixture.Create<string>())));

            //assert
            Guid message = Guid.NewGuid();
            rootActor.Tell(message);
            ExpectMsg<string>().Should().Be($"{prefix} : {message}");
        }
    }
}