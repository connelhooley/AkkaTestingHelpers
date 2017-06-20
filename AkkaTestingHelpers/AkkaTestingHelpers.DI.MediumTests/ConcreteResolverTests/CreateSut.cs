using System;
using System.Linq;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.Xunit2;
using FluentAssertions;
using Moq;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.MediumTests.ConcreteResolverTests
{
    public class CreateSut : TestKit
    {
        public CreateSut(): base(AkkaConfig.Config) { }

        [Fact]
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

        [Fact]
        public void ConcreteResolver_CreatesChildrenWithDependancies()
        {
            //arrange
            const int childCount = 5;
            Mock<IDependancy> dependancyMock = new Mock<IDependancy>();
            IDependancy dependancyMockInstance = dependancyMock.Object;
            ConcreteResolver sut = ConcreteResolverSettings
                .Empty
                .Register<EmptyChildActor>()
                .Register(() => new ChildActor(dependancyMockInstance))
                .CreateResolver(this);

            //act
            TestActorRef<ParentActor> actor = sut.CreateSut<ParentActor>(Props.Create(() => new ParentActor(childCount)), childCount);

            //assert
            actor.Tell(new object());
            AwaitAssert(() =>
                dependancyMock.Verify(
                    dependancy => dependancy.SetResut(ChildActor.Token), 
                    Times.Exactly(childCount)));
        }

        [Fact]
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

        [Fact]
        public void ConcreteResolver_TimesOutWhenChildrenCountIsTooHigh()
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

        [Fact]
        public void ConcreteResolver_UsesLatestFactory()
        {
            //arrange
            const int childCount = 5;
            Mock<IDependancy> dependancyMock = new Mock<IDependancy>();
            IDependancy dependancyMockInstance = dependancyMock.Object;
            ConcreteResolver sut = ConcreteResolverSettings
                .Empty
                .Register<EmptyChildActor>()
                .Register<ChildActor>()
                .Register(() => new ChildActor(dependancyMockInstance))
                .CreateResolver(this);

            //act
            TestActorRef<ParentActor> actor = sut.CreateSut<ParentActor>(Props.Create(() => new ParentActor(childCount)), childCount);

            //assert
            actor.Tell(new object());
            AwaitAssert(() => 
                dependancyMock.Verify(
                    dependancy => dependancy.SetResut(ChildActor.Token), 
                    Times.Exactly(childCount)));
        }
    }
}