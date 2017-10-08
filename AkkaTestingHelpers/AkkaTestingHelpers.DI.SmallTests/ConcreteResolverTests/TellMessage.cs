using System;
using System.Collections.Immutable;
using FluentAssertions;
using Moq;
using Xunit;
using EitherFactory = Akka.Util.Either<
    System.Func<Akka.Actor.ActorBase>,
    ConnelHooley.AkkaTestingHelpers.DI.IRegisterableActorFake>;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.ConcreteResolverTests
{
    public class TellMessage : TestBase
    {
        #region null checks

        [Fact]
        public void ConcreteResolver_TellMessageNoSenderWithNullRecipient_ThrowsArgumentNullException()
        {
            //arrange
            ConcreteResolver sut = CreateConcreteResolver(ImmutableDictionary<Type, EitherFactory>.Empty);

            //act
            Action act = () => sut.TellMessage(null, Message, ExpectedChildrenCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void ConcreteResolver_TellMessageNoSenderWithNullMessage_ThrowsArgumentNullException()
        {
            //arrange
            ConcreteResolver sut = CreateConcreteResolver(ImmutableDictionary<Type, EitherFactory>.Empty);

            //act
            Action act = () => sut.TellMessage<object>(Recipient, null, ExpectedChildrenCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }
        
        [Fact]
        public void ConcreteResolver_TellMessageNoSenderWithNullRecipientAndMessage_ThrowsArgumentNullException()
        {
            //arrange
            ConcreteResolver sut = CreateConcreteResolver(ImmutableDictionary<Type, EitherFactory>.Empty);

            //act
            Action act = () => sut.TellMessage<object>(null, null, ExpectedChildrenCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void ConcreteResolver_TellMessageFromSenderWithNullRecipient_ThrowsArgumentNullException()
        {
            //arrange
            ConcreteResolver sut = CreateConcreteResolver(ImmutableDictionary<Type, EitherFactory>.Empty);

            //act
            Action act = () => sut.TellMessage(null, Message, Sender, ExpectedChildrenCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void ConcreteResolver_TellMessageFromSenderWithNullMessage_ThrowsArgumentNullException()
        {
            //arrange
            ConcreteResolver sut = CreateConcreteResolver(ImmutableDictionary<Type, EitherFactory>.Empty);

            //act
            Action act = () => sut.TellMessage<object>(Recipient, null, Sender, ExpectedChildrenCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void ConcreteResolver_TellMessageFromSenderWithNullSender_ThrowsArgumentNullException()
        {
            //arrange
            ConcreteResolver sut = CreateConcreteResolver(ImmutableDictionary<Type, EitherFactory>.Empty);

            //act
            Action act = () => sut.TellMessage(Recipient, Message, null, ExpectedChildrenCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void ConcreteResolver_TellMessageNoSenderWithNullRecipientAndMessageAndSender_ThrowsArgumentNullException()
        {
            //arrange
            ConcreteResolver sut = CreateConcreteResolver(ImmutableDictionary<Type, EitherFactory>.Empty);

            //act
            Action act = () => sut.TellMessage<object>(null, null, null, ExpectedChildrenCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void ConcreteResolver_TellMessageSenderWithNullSender_ThrowsArgumentNullException()
        {
            //arrange
            ConcreteResolver sut = CreateConcreteResolver(ImmutableDictionary<Type, EitherFactory>.Empty);

            //act
            Action act = () => sut.TellMessage(Recipient, Message, null, ExpectedChildrenCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void ConcreteResolver_TellMessageWithNullRecipientAndMessageAndSender_ThrowsArgumentNullException()
        {
            //arrange
            ConcreteResolver sut = CreateConcreteResolver(ImmutableDictionary<Type, EitherFactory>.Empty);

            //act
            Action act = () => sut.TellMessage<object>(null, null, null, ExpectedChildrenCount);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        #endregion

        [Fact]
        public void ConcreteResolver_TellMessageNoSender_TellsChild()
        {
            //arrange
            ConcreteResolver sut = CreateConcreteResolver(ImmutableDictionary<Type, EitherFactory>.Empty);

            //act
            sut.TellMessage(Recipient, Message, ExpectedChildrenCount);

            //assert
            ChildTellerMock.Verify(
                teller => teller.TellMessage(ChildWaiterMock.Object, this, Recipient, Message, ExpectedChildrenCount, null),
                Times.Once);
        }
        
        [Fact]
        public void ConcreteResolver_TellMessageSender_TellsChild()
        {
            //arrange
            ConcreteResolver sut = CreateConcreteResolver(ImmutableDictionary<Type, EitherFactory>.Empty);

            //act
            sut.TellMessage(Recipient, Message, Sender, ExpectedChildrenCount);

            //assert
            ChildTellerMock.Verify(
                teller => teller.TellMessage(ChildWaiterMock.Object, this, Recipient, Message, ExpectedChildrenCount, Sender),
                Times.Once);
        }
    }
}