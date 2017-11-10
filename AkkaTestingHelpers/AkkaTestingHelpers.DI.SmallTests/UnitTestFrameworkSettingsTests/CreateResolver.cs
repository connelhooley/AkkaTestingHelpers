//using System;
//using System.Collections.Immutable;
//using FluentAssertions;
//using Xunit;

//namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.UnitTestFrameworkSettingsTests
//{
//    public class CreateResolver : TestBase
//    {
//        #region Null tests
//        [Fact]
//        public void UnitTestFrameworkSettings_CreateResolverWithNullTestKit_ThrowsArgumentNullException()
//        {
//            //arrange
//            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings.Empty;

//            //act
//            Action act = () => sut.CreateFramework<DummyActor1>(null);

//            //assert
//            act.ShouldThrow<ArgumentNullException>();
//        }
//        #endregion

//        [Fact]
//        public void UnitTestFrameworkSettings_CreateResolver_ReturnsTestProbeResolver()
//        {
//            //arrange
//            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings.Empty;

//            //act
//            UnitTestFramework<> result = sut.CreateResolver(this);

//            //assert
//            result.Should().BeSameAs(ConstructedTestProbeResolver);
//        }

//        [Fact] //todo fix
//        public void UnitTestFrameworkSettings_CreateResolver_ConstructsOnlyOneTestProbeResolver()
//        {
//            //arrange
//            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings.Empty;

//            //act
//            sut.CreateResolver(this);

//            //assert
//            TestProbeResolverConstructorCount.Should().Be(1);
//        }

//        [Fact]
//        public void UnitTestFrameworkSettings_CreateResolver_ConstructsTestProbeResolverWithSutCreatorClass()
//        {
//            //arrange
//            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings.Empty;

//            //act
//            sut.CreateResolver(this);

//            //assert
//            SutCreatorPassedIntoShim.Should().BeSameAs(ConstructedSutCreator);
//        }

//        [Fact]
//        public void UnitTestFrameworkSettings_CreateResolver_OnlyConstructsOneSutCreator()
//        {
//            //arrange
//            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings.Empty;

//            //act
//            sut.CreateResolver(this);

//            //assert
//            SutCreatorConstructorCount.Should().Be(1);
//        }

//        [Fact]
//        public void UnitTestFrameworkSettings_CreateResolver_ConstructsTestProbeResolverWithChildTellerClass()
//        {
//            //arrange
//            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings.Empty;

//            //act
//            sut.CreateResolver(this);

//            //assert
//            ChildTellerPassedIntoShim.Should().BeSameAs(ConstructedChildTeller);
//        }

//        [Fact]
//        public void UnitTestFrameworkSettings_CreateResolver_OnlyConstructsOneChildTeller()
//        {
//            //arrange
//            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings.Empty;

//            //act
//            sut.CreateResolver(this);

//            //assert
//            ChildTellerConstructorCount.Should().Be(1);
//        }

//        [Fact]
//        public void UnitTestFrameworkSettings_CreateResolver_ConstructsTestProbeResolverWithChildWaiterClass()
//        {
//            //arrange
//            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings.Empty;

//            //act
//            sut.CreateResolver(this);

//            //assert
//            ChildWaiterPassedIntoShim.Should().BeSameAs(ConstructedChildWaiter);
//        }

//        [Fact]
//        public void UnitTestFrameworkSettings_CreateResolver_OnlyConstructsOneChildWaiter()
//        {
//            //arrange
//            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings.Empty;

//            //act
//            sut.CreateResolver(this);

//            //assert
//            ChildWaiterConstructorCount.Should().Be(1);
//        }

//        [Fact]
//        public void UnitTestFrameworkSettings_CreateResolver_ConstructsTestProbeResolverWithDependencyResolverAdderClass()
//        {
//            //arrange
//            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings.Empty;

//            //act
//            sut.CreateResolver(this);

//            //assert
//            DependencyResolverAdderPassedIntoShim.Should().BeSameAs(ConstructedDependencyResolverAdder);
//        }

//        [Fact]
//        public void UnitTestFrameworkSettings_CreateResolver_OnlyConstructsOneDependencyResolverAdder()
//        {
//            //arrange
//            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings.Empty;

//            //act
//            sut.CreateResolver(this);

//            //assert
//            DependencyResolverAdderConstructorCount.Should().Be(1);
//        }

//        [Fact]
//        public void UnitTestFrameworkSettings_CreateResolver_ConstructsTestProbeResolverWithConcreteDependencyResolverAdder()
//        {
//            //arrange
//            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings.Empty;

//            //act
//            sut.CreateResolver(this);

//            //assert
//            TestProbeDependencyResolverAdderPassedIntoShim.Should().BeSameAs(ConstructedTestProbeDependencyResolverAdder);
//        }

//        [Fact]
//        public void UnitTestFrameworkSettings_CreateResolver_OnlyConstructsOneConcreteDependencyResolverAdder()
//        {
//            //arrange
//            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings.Empty;

//            //act
//            sut.CreateResolver(this);

//            //assert
//            TestProbeDependencyResolverAdderConstructorCount.Should().Be(1);
//        }

//        [Fact]
//        public void UnitTestFrameworkSettings_CreateResolver_ConstructsTestProbeResolverWithTestProbeCreator()
//        {
//            //arrange
//            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings.Empty;

//            //act
//            sut.CreateResolver(this);

//            //assert
//            TestProbeCreatorPassedIntoShim.Should().BeSameAs(ConstructedTestProbeCreator);
//        }

//        [Fact]
//        public void UnitTestFrameworkSettings_CreateResolver_OnlyConstructsOneTestProbeCreator()
//        {
//            //arrange
//            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings.Empty;

//            //act
//            sut.CreateResolver(this);

//            //assert
//            TestProbeCreatorConstructorCount.Should().Be(1);
//        }

//        [Fact]
//        public void UnitTestFrameworkSettings_CreateResolver_ConstructsTestProbeResolverWithResolvedTestProbeStore()
//        {
//            //arrange
//            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings.Empty;

//            //act
//            sut.CreateResolver(this);

//            //assert
//            ResolvedTestProbeStorePassedIntoShim.Should().BeSameAs(ConstructedResolvedTestProbeStore);
//        }

//        [Fact]
//        public void UnitTestFrameworkSettings_CreateResolver_OnlyConstructsOneResolvedTestProbeStore()
//        {
//            //arrange
//            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings.Empty;

//            //act
//            sut.CreateResolver(this);

//            //assert
//            ResolvedTestProbeStoreConstructorCount.Should().Be(1);
//        }

//        [Fact]
//        public void UnitTestFrameworkSettings_CreateResolver_ConstructsTestProbeResolverWithTestProbeActorCreator()
//        {
//            //arrange
//            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings.Empty;

//            //act
//            sut.CreateResolver(this);

//            //assert
//            TestProbeActorCreatorPassedIntoShim.Should().BeSameAs(ConstructedTestProbeActorCreator);
//        }

//        [Fact]
//        public void UnitTestFrameworkSettings_CreateResolver_OnlyConstructsOneTestProbeActorCreator()
//        {
//            //arrange
//            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings.Empty;

//            //act
//            sut.CreateResolver(this);

//            //assert
//            TestProbeActorCreatorConstructorCount.Should().Be(1);
//        }

//        [Fact]
//        public void UnitTestFrameworkSettings_CreateResolver_ConstructsTestProbeResolverWithTestProbeHandlersMapper()
//        {
//            //arrange
//            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings.Empty;

//            //act
//            sut.CreateResolver(this);

//            //assert
//            TestProbeHandlersMapperPassedIntoShim.Should().BeSameAs(ConstructedTestProbeHandlersMapper);
//        }

//        [Fact]
//        public void UnitTestFrameworkSettings_CreateResolver_OnlyConstructsOneTestProbeHandlersMapper()
//        {
//            //arrange
//            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings.Empty;

//            //act
//            sut.CreateResolver(this);

//            //assert
//            TestProbeHandlersMapperConstructorCount.Should().Be(1);
//        }

//        [Fact]
//        public void UnitTestFrameworkSettings_CreateResolver_ConstructsTestProbeResolverWithCorrectTestKit()
//        {
//            //arrange
//            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings.Empty;

//            //act
//            sut.CreateResolver(this);

//            //assert
//            TestKitPassedIntoShim.Should().BeSameAs(this);
//        }

//        [Fact]
//        public void UnitTestFrameworkSettings_CreateResolverWithNoHandlers_ConstructsTestProbeResolverWithEmptyHandlers()
//        {
//            //arrange
//            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings.Empty;

//            //act
//            sut.CreateResolver(this);

//            //assert
//            HandlersPassedIntoShim.Should().BeEmpty();
//        }

//        [Fact]
//        public void UnitTestFrameworkSettings_CreateResolverWithHandlers_ConstructsTestProbeResolverWithCorrectHandlers()
//        {
//            //arrange
//            Reply1 reply1 = new Reply1();
//            Reply2 reply2 = new Reply2();
//            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings
//                .Empty
//                .RegisterHandler<DummyActor1, Message1>(message1 => reply1)
//                .RegisterHandler<DummyActor1, Message2>(message2 => reply2)
//                .RegisterHandler<DummyActor2, Message1>(message1 => reply1);

//            //act
//            sut.CreateResolver(this);

//            //assert
//            HandlersPassedIntoShim.ShouldAllBeEquivalentTo(
//                ImmutableDictionary<(Type, Type), Func<object, object>>
//                    .Empty
//                    .Add((typeof(DummyActor1), typeof(Message1)), message1 => reply1)
//                    .Add((typeof(DummyActor1), typeof(Message2)), message2 => reply2)
//                    .Add((typeof(DummyActor2), typeof(Message1)), message1 => reply1),
//                options => options
//                    .Using<Func<object, object>>(context => context.Subject.Invoke(null).Should().BeSameAs(context.Expectation.Invoke(null)))
//                    .WhenTypeIs<Func<object, object>>());
//        }

//        [Fact]
//        public void UnitTestFrameworkSettings_CreateResolverWithDuplicateHandlers_ConstructsTestProbeResolverWithCorrectHandlers()
//        {
//            //arrange
//            Reply1 reply1 = new Reply1();
//            Reply2 reply2 = new Reply2();
//            Reply1 duplicateReply1 = new Reply1();
//            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings
//                .Empty
//                .RegisterHandler<DummyActor1, Message1>(message1 => reply1)
//                .RegisterHandler<DummyActor2, Message2>(message2 => reply2)
//                .RegisterHandler<DummyActor1, Message1>(message1 => duplicateReply1);

//            //act
//            sut.CreateResolver(this);

//            //assert
//            HandlersPassedIntoShim.ShouldAllBeEquivalentTo(
//                ImmutableDictionary<(Type, Type), Func<object, object>>
//                    .Empty
//                    .Add((typeof(DummyActor1), typeof(Message1)), message1 => duplicateReply1)
//                    .Add((typeof(DummyActor2), typeof(Message2)), message2 => reply2),
//                options => options
//                    .Using<Func<object, object>>(context => context.Subject.Invoke(null).Should().BeSameAs(context.Expectation.Invoke(null)))
//                    .WhenTypeIs<Func<object, object>>());
//        }

//        [Fact]
//        public void UnitTestFrameworkSettings_CreateResolverWithHandlersInDifferentInstances_ConstructsTestProbeResolverWithCorrectHandlers()
//        {
//            //arrange
//            Reply1 reply1 = new Reply1();
//            Reply2 reply2 = new Reply2();
//            Reply1 duplicateReply1 = new Reply1();
//            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings
//                .Empty
//                .RegisterHandler<DummyActor1, Message1>(message1 => reply1)
//                .RegisterHandler<DummyActor2, Message2>(message1 => reply2);

//            UnitTestFrameworkSettings differentInstance = sut
//                .RegisterHandler<DummyActor1, Message1>(message1 => duplicateReply1);

//            //act
//            sut.CreateResolver(this);

//            //assert
//            HandlersPassedIntoShim.ShouldAllBeEquivalentTo(
//                ImmutableDictionary<(Type, Type), Func<object, object>>
//                    .Empty
//                    .Add((typeof(DummyActor1), typeof(Message1)), message1 => reply1)
//                    .Add((typeof(DummyActor2), typeof(Message2)), message1 => reply2),
//                options => options
//                    .Using<Func<object, object>>(context => context.Subject.Invoke(null).Should().BeSameAs(context.Expectation.Invoke(null)))
//                    .WhenTypeIs<Func<object, object>>());
//        }

//        [Fact]
//        public void UnitTestFrameworkSettings_CreateResolverWithHandlers_ConstructsTestProbeResolverWithHandlersThatReceiveTheCorrectMessage()
//        {
//            //arrange
//            Message1 actual = null;
//            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings
//                .Empty
//                .RegisterHandler<DummyActor1, Message1>(mess =>
//                {
//                    actual = mess;
//                    return new Reply1();
//                });

//            //act
//            sut.CreateResolver(this);

//            //assert
//            Message1 exptectedMessage = new Message1();
//            HandlersPassedIntoShim[(typeof(DummyActor1), typeof(Message1))].Invoke(exptectedMessage);
//            actual.Should().BeSameAs(exptectedMessage);
//        }

//        [Fact]
//        public void UnitTestFrameworkSettings_CreateResolverWithHandlers_ConstructsTestProbeResolverWithHandlersThatThrowWhenGivenTheWrongMessageType()
//        {
//            //arrange
//            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings
//                .Empty
//                .RegisterHandler<DummyActor1, Message1>(mess => new Reply1());

//            //act
//            sut.CreateResolver(this);

//            //assert
//            Action invokeHandlerWithWrongMessageType = () => HandlersPassedIntoShim[(typeof(DummyActor1), typeof(Message1))].Invoke(new Message2());
//            invokeHandlerWithWrongMessageType.ShouldThrow<InvalidCastException>();
//        }
//    }
//}