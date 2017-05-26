using System;
using System.Linq;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.NUnit3;
using ConnelHooley.AkkaTestingHelpers.DI;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace AkkaTestingHelpers.DI.MediumTests.ConcreteResolverTests
{
    public class CreateSut : TestKit
    {
        public CreateSut(): base(@"akka.test.timefactor = 0.6") { }

        [Test]
        public void ConcreteResolver_CreatesChildrenWithoutDependancies()
        {
            //arrange
            const int childCount = 5;
            ConcreteResolver sut = ConcreteResolverSettings
                .Empty
                .Register<EmptyChildActor>()
                .Register<ChildActor>()
                .CreateResolver(this);

            //act
            TestActorRef<ParentActor> actor = sut.CreateSut<ParentActor>(Props.Create(() => new ParentActor(childCount)), childCount);

            //assert
            actor.Tell(new object());
            ExpectMsgAllOf(Enumerable.Repeat(ChildActor.Token, childCount).ToArray());
        }

        [Test]
        public void ConcreteResolver_CreatesChildrenWithDependancies()
        {
            //arrange
            const int childCount = 5;
            Mock<IDependancy> dependancyMock = new Mock<IDependancy>();
            ConcreteResolver sut = ConcreteResolverSettings
                .Empty
                .Register<EmptyChildActor>()
                .Register(() => new ChildActor(dependancyMock.Object))
                .CreateResolver(this);

            //act
            TestActorRef<ParentActor> actor = sut.CreateSut<ParentActor>(Props.Create(() => new ParentActor(childCount)), childCount);

            //assert
            actor.Tell(new object());
            dependancyMock.Verify(dependancy => dependancy.SetResut(ChildActor.Token), Times.Exactly(childCount));
        }

        [Test]
        public void ConcreteResolver_TimesOutWithChildThatIsNotRegistered()
        {
            //arrange
            const int childCount = 5;
            ConcreteResolver sut = ConcreteResolverSettings
                .Empty
                .Register<EmptyChildActor>()
                .CreateResolver(this);

            //act
            Action act = () => sut.CreateSut<ParentActor>(Props.Create(() => new ParentActor(childCount)), childCount);

            //assert
            act.ShouldThrow<TimeoutException>(); //Invalid operation exception is swallowed by Akka
        }

        [Test]
        public void ConcreteResolver_TimesOutWhenCHildrenCountIsTooHigh()
        {
            //arrange
            const int childCount = 5;
            ConcreteResolver sut = ConcreteResolverSettings
                .Empty
                .Register<EmptyChildActor>()
                .CreateResolver(this);

            //act
            Action act = () => sut.CreateSut<ParentActor>(Props.Create(() => new ParentActor(childCount)), childCount+1);

            //assert
            act.ShouldThrow<TimeoutException>();
        }
    }
}