using System;
using System.Collections.Immutable;
using Akka.Actor;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.TestProbeResolverSettingsTests
{
    public class CreateResolver : TestBase
    {
        #region Null tests
        [Fact]
        public void TestProbeResolverSettings_CreateResolverWithNullTestKit_ThrowsArgumentNullException()
        {
            //arrange
            TestProbeResolverSettings sut = TestProbeResolverSettings.Empty;

            //act
            Action act = () => sut.CreateResolver(null);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }
        #endregion

        [Fact]
        public void TestProbeResolverSettings_CreateResolver_ReturnsTestProbeResolver()
        {
            //arrange
            TestProbeResolverSettings sut = TestProbeResolverSettings.Empty;

            //act
            TestProbeResolver result = sut.CreateResolver(this);

            //assert
            result.Should().BeSameAs(ConstructedTestProbeResolver);
        }

        [Fact] //todo fix
        public void TestProbeResolverSettings_CreateResolver_ConstructsOnlyOneTestProbeResolver()
        {
            //arrange
            TestProbeResolverSettings sut = TestProbeResolverSettings.Empty;

            //act
            sut.CreateResolver(this);

            //assert
            TestProbeResolverConstructorCount.Should().Be(1);
        }

        [Fact]
        public void TestProbeResolverSettings_CreateResolver_ConstructsTestProbeResolverWithSutCreatorClass()
        {
            //arrange
            TestProbeResolverSettings sut = TestProbeResolverSettings.Empty;

            //act
            sut.CreateResolver(this);

            //assert
            SutCreatorPassedIntoShim.Should().BeSameAs(ConstructedSutCreator);
        }

        [Fact]
        public void TestProbeResolverSettings_CreateResolver_OnlyConstructsOneSutCreator()
        {
            //arrange
            TestProbeResolverSettings sut = TestProbeResolverSettings.Empty;

            //act
            sut.CreateResolver(this);

            //assert
            SutCreatorConstructorCount.Should().Be(1);
        }

        [Fact]
        public void TestProbeResolverSettings_CreateResolver_ConstructsTestProbeResolverWithChildTellerClass()
        {
            //arrange
            TestProbeResolverSettings sut = TestProbeResolverSettings.Empty;

            //act
            sut.CreateResolver(this);

            //assert
            ChildTellerPassedIntoShim.Should().BeSameAs(ConstructedChildTeller);
        }

        [Fact]
        public void TestProbeResolverSettings_CreateResolver_OnlyConstructsOneChildTeller()
        {
            //arrange
            TestProbeResolverSettings sut = TestProbeResolverSettings.Empty;

            //act
            sut.CreateResolver(this);

            //assert
            ChildTellerConstructorCount.Should().Be(1);
        }

        [Fact]
        public void TestProbeResolverSettings_CreateResolver_ConstructsTestProbeResolverWithChildWaiterClass()
        {
            //arrange
            TestProbeResolverSettings sut = TestProbeResolverSettings.Empty;

            //act
            sut.CreateResolver(this);

            //assert
            ChildWaiterPassedIntoShim.Should().BeSameAs(ConstructedChildWaiter);
        }

        [Fact]
        public void TestProbeResolverSettings_CreateResolver_OnlyConstructsOneChildWaiter()
        {
            //arrange
            TestProbeResolverSettings sut = TestProbeResolverSettings.Empty;

            //act
            sut.CreateResolver(this);

            //assert
            ChildWaiterConstructorCount.Should().Be(1);
        }

        [Fact]
        public void TestProbeResolverSettings_CreateResolver_ConstructsTestProbeResolverWithDependencyResolverAdderClass()
        {
            //arrange
            TestProbeResolverSettings sut = TestProbeResolverSettings.Empty;

            //act
            sut.CreateResolver(this);

            //assert
            DependencyResolverAdderPassedIntoShim.Should().BeSameAs(ConstructedDependencyResolverAdder);
        }

        [Fact]
        public void TestProbeResolverSettings_CreateResolver_OnlyConstructsOneDependencyResolverAdder()
        {
            //arrange
            TestProbeResolverSettings sut = TestProbeResolverSettings.Empty;

            //act
            sut.CreateResolver(this);

            //assert
            DependencyResolverAdderConstructorCount.Should().Be(1);
        }

        [Fact]
        public void TestProbeResolverSettings_CreateResolver_ConstructsTestProbeResolverWithConcreteDependencyResolverAdder()
        {
            //arrange
            TestProbeResolverSettings sut = TestProbeResolverSettings.Empty;

            //act
            sut.CreateResolver(this);

            //assert
            TestProbeDependencyResolverAdderPassedIntoShim.Should().BeSameAs(ConstructedTestProbeDependencyResolverAdder);
        }

        [Fact]
        public void TestProbeResolverSettings_CreateResolver_OnlyConstructsOneConcreteDependencyResolverAdder()
        {
            //arrange
            TestProbeResolverSettings sut = TestProbeResolverSettings.Empty;

            //act
            sut.CreateResolver(this);

            //assert
            TestProbeDependencyResolverAdderConstructorCount.Should().Be(1);
        }
        
        [Fact]
        public void TestProbeResolverSettings_CreateResolver_ConstructsTestProbeResolverWithTestProbeCreator()
        {
            //arrange
            TestProbeResolverSettings sut = TestProbeResolverSettings.Empty;

            //act
            sut.CreateResolver(this);

            //assert
            TestProbeCreatorPassedIntoShim.Should().BeSameAs(ConstructedTestProbeCreator);
        }

        [Fact]
        public void TestProbeResolverSettings_CreateResolver_OnlyConstructsOneTestProbeCreator()
        {
            //arrange
            TestProbeResolverSettings sut = TestProbeResolverSettings.Empty;

            //act
            sut.CreateResolver(this);

            //assert
            TestProbeCreatorConstructorCount.Should().Be(1);
        }

        [Fact]
        public void TestProbeResolverSettings_CreateResolver_ConstructsTestProbeResolverWithResolvedTestProbeStore()
        {
            //arrange
            TestProbeResolverSettings sut = TestProbeResolverSettings.Empty;

            //act
            sut.CreateResolver(this);

            //assert
            ResolvedTestProbeStorePassedIntoShim.Should().BeSameAs(ConstructedResolvedTestProbeStore);
        }

        [Fact]
        public void TestProbeResolverSettings_CreateResolver_OnlyConstructsOneResolvedTestProbeStore()
        {
            //arrange
            TestProbeResolverSettings sut = TestProbeResolverSettings.Empty;

            //act
            sut.CreateResolver(this);

            //assert
            ResolvedTestProbeStoreConstructorCount.Should().Be(1);
        }

        [Fact]
        public void TestProbeResolverSettings_CreateResolver_ConstructsTestProbeResolverWithTestProbeActorCreator()
        {
            //arrange
            TestProbeResolverSettings sut = TestProbeResolverSettings.Empty;

            //act
            sut.CreateResolver(this);

            //assert
            TestProbeActorCreatorPassedIntoShim.Should().BeSameAs(ConstructedTestProbeActorCreator);
        }

        [Fact]
        public void TestProbeResolverSettings_CreateResolver_OnlyConstructsOneTestProbeActorCreator()
        {
            //arrange
            TestProbeResolverSettings sut = TestProbeResolverSettings.Empty;

            //act
            sut.CreateResolver(this);

            //assert
            TestProbeActorCreatorConstructorCount.Should().Be(1);
        }

        [Fact]
        public void TestProbeResolverSettings_CreateResolver_ConstructsTestProbeResolverWithTestProbeHandlersMapper()
        {
            //arrange
            TestProbeResolverSettings sut = TestProbeResolverSettings.Empty;

            //act
            sut.CreateResolver(this);

            //assert
            TestProbeHandlersMapperPassedIntoShim.Should().BeSameAs(ConstructedTestProbeHandlersMapper);
        }

        [Fact]
        public void TestProbeResolverSettings_CreateResolver_OnlyConstructsOneTestProbeHandlersMapper()
        {
            //arrange
            TestProbeResolverSettings sut = TestProbeResolverSettings.Empty;

            //act
            sut.CreateResolver(this);

            //assert
            TestProbeHandlersMapperConstructorCount.Should().Be(1);
        }
        
        [Fact]
        public void TestProbeResolverSettings_CreateResolver_ConstructsTestProbeResolverWithCorrectTestKit()
        {
            //arrange
            TestProbeResolverSettings sut = TestProbeResolverSettings.Empty;

            //act
            sut.CreateResolver(this);

            //assert
            TestKitPassedIntoShim.Should().BeSameAs(this);
        }

        [Fact]
        public void TestProbeResolverSettings_CreateResolverWithNoHandlers_ConstructsTestProbeResolverWithEmptyHandlers()
        {
            //arrange
            TestProbeResolverSettings sut = TestProbeResolverSettings.Empty;

            //act
            sut.CreateResolver(this);

            //assert
            HandlersPassedIntoShim.Should().BeEmpty();
        }

        [Fact]
        public void TestProbeResolverSettings_CreateResolverWithHandlers_ConstructsTestProbeResolverWithCorrectHandlers()
        {
            //arrange
            Reply1 reply1 = new Reply1();
            Reply2 reply2 = new Reply2();
            TestProbeResolverSettings sut = TestProbeResolverSettings
                .Empty
                .RegisterHandler<DummyActor1, Message1>(message1 => reply1)
                .RegisterHandler<DummyActor1, Message2>(message2 => reply2)
                .RegisterHandler<DummyActor2, Message1>(message1 => reply1);

            //act
            sut.CreateResolver(this);

            //assert
            HandlersPassedIntoShim.ShouldAllBeEquivalentTo(
                ImmutableDictionary<(Type, Type), Func<object, object>>
                    .Empty
                    .Add((typeof(DummyActor1), typeof(Message1)), message1 => reply1)
                    .Add((typeof(DummyActor1), typeof(Message2)), message2 => reply2)
                    .Add((typeof(DummyActor2), typeof(Message1)), message1 => reply1),
                options => options
                    .Using<Func<object, object>>(context => context.Subject.Invoke(null).Should().BeSameAs(context.Expectation.Invoke(null)))
                    .WhenTypeIs<Func<object, object>>());
        }

        [Fact]
        public void TestProbeResolverSettings_CreateResolverWithDuplicateHandlers_ConstructsTestProbeResolverWithCorrectHandlers()
        {
            //arrange
            Reply1 reply1 = new Reply1();
            Reply2 reply2 = new Reply2();
            Reply1 duplicateReply1 = new Reply1();
            TestProbeResolverSettings sut = TestProbeResolverSettings
                .Empty
                .RegisterHandler<DummyActor1, Message1>(message1 => reply1)
                .RegisterHandler<DummyActor2, Message2>(message2 => reply2)
                .RegisterHandler<DummyActor1, Message1>(message1 => duplicateReply1);

            //act
            sut.CreateResolver(this);

            //assert
            HandlersPassedIntoShim.ShouldAllBeEquivalentTo(
                ImmutableDictionary<(Type, Type), Func<object, object>>
                    .Empty
                    .Add((typeof(DummyActor1), typeof(Message1)), message1 => duplicateReply1)
                    .Add((typeof(DummyActor2), typeof(Message2)), message2 => reply2),
                options => options
                    .Using<Func<object, object>>(context => context.Subject.Invoke(null).Should().BeSameAs(context.Expectation.Invoke(null)))
                    .WhenTypeIs<Func<object, object>>());
        }

        [Fact]
        public void TestProbeResolverSettings_CreateResolverWithHandlersInDifferentInstances_ConstructsTestProbeResolverWithCorrectHandlers()
        {
            //arrange
            Reply1 reply1 = new Reply1();
            Reply2 reply2 = new Reply2();
            Reply1 duplicateReply1 = new Reply1();
            TestProbeResolverSettings sut = TestProbeResolverSettings
                .Empty
                .RegisterHandler<DummyActor1, Message1>(message1 => reply1)
                .RegisterHandler<DummyActor2, Message2>(message1 => reply2);
                
            TestProbeResolverSettings differentInstance = sut
                .RegisterHandler<DummyActor1, Message1>(message1 => duplicateReply1);

            //act
            sut.CreateResolver(this);

            //assert
            HandlersPassedIntoShim.ShouldAllBeEquivalentTo(
                ImmutableDictionary<(Type, Type), Func<object, object>>
                    .Empty
                    .Add((typeof(DummyActor1), typeof(Message1)), message1 => reply1)
                    .Add((typeof(DummyActor2), typeof(Message2)), message1 => reply2),
                options => options
                    .Using<Func<object, object>>(context => context.Subject.Invoke(null).Should().BeSameAs(context.Expectation.Invoke(null)))
                    .WhenTypeIs<Func<object, object>>());
        }
        
        [Fact]
        public void TestProbeResolverSettings_CreateResolverWithHandlers_ConstructsTestProbeResolverWithHandlersThatReceiveTheCorrectMessage()
        {
            //arrange
            Message1 actual = null;
            TestProbeResolverSettings sut = TestProbeResolverSettings
                .Empty
                .RegisterHandler<DummyActor1, Message1>(mess =>
                {
                    actual = mess;
                    return new Reply1();
                });
            
            //act
            sut.CreateResolver(this);

            //assert
            Message1 exptectedMessage = new Message1();
            HandlersPassedIntoShim[(typeof(DummyActor1), typeof(Message1))].Invoke(exptectedMessage);
            actual.Should().BeSameAs(exptectedMessage);
        }

        [Fact]
        public void TestProbeResolverSettings_CreateResolverWithHandlers_ConstructsTestProbeResolverWithHandlersThatThrowWhenGivenTheWrongMessageType()
        {
            //arrange
            TestProbeResolverSettings sut = TestProbeResolverSettings
                .Empty
                .RegisterHandler<DummyActor1, Message1>(mess => new Reply1());

            //act
            sut.CreateResolver(this);

            //assert
            Action invokeHandlerWithWrongMessageType = () => HandlersPassedIntoShim[(typeof(DummyActor1), typeof(Message1))].Invoke(new Message2());
            invokeHandlerWithWrongMessageType.ShouldThrow<InvalidCastException>();
        }
    }
}