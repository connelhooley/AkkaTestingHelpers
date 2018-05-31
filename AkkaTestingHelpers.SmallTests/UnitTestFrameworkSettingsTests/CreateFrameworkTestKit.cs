using System;
using System.Collections.Immutable;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.UnitTestFrameworkSettingsTests
{
    public class CreateFrameworkTestKit : TestBase
    {
        #region Null tests
        [Fact]
        public void UnitTestFrameworkSettings_CreateFrameworkWithNullTestKit_ThrowsArgumentNullException()
        {
            //arrange
            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings.Empty;

            //act
            Action act = () => sut.CreateFramework<DummyChildActor1>(null);

            //assert
            act.Should().Throw<ArgumentNullException>();
        }
        #endregion

        [Fact]
        public void UnitTestFrameworkSettings_CreateFramework_ConstructsOnlyOneUnitTestFrameworkCreator()
        {
            //arrange
            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings.Empty;

            //act
            sut.CreateFramework<DummyChildActor1>(TestKitPassedIntoSut);

            //assert
            UnitTestFrameworkCreatorConstructorCount.Should().Be(1);
        }
        
        [Fact]
        public void UnitTestFrameworkSettingsWithHandlers_CreateFramework_ConstructsTestProbeResolverWithCorrectHandlers()
        {
            //arrange
            Reply1 reply1 = new Reply1();
            Reply2 reply2 = new Reply2();
            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings
                .Empty
                .RegisterHandler<DummyChildActor1, Message1>(message1 => reply1)
                .RegisterHandler<DummyChildActor1, Message2>(message2 => reply2)
                .RegisterHandler<DummyChildActor2, Message1>(message1 => reply1);

            //act
            sut.CreateFramework<DummyParentActor>(this);

            //assert
            HandlersPassedIntoShim.ToImmutableList().Should().BeEquivalentTo(
                ImmutableDictionary<(Type, Type), Func<object, object>>
                    .Empty
                    .Add((typeof(DummyChildActor1), typeof(Message1)), message1 => reply1)
                    .Add((typeof(DummyChildActor1), typeof(Message2)), message2 => reply2)
                    .Add((typeof(DummyChildActor2), typeof(Message1)), message1 => reply1)
                    .ToImmutableList(),
                options => options
                    .Using<Func<object, object>>(context => context.Subject.Invoke(null).Should().BeSameAs(context.Expectation.Invoke(null)))
                    .WhenTypeIs<Func<object, object>>());
        }

        [Fact]
        public void UnitTestFrameworkSettingsWithDuplicateHandlers_CreateFramework_ConstructsTestProbeResolverWithCorrectHandlers()
        {
            //arrange
            Reply1 reply1 = new Reply1();
            Reply2 reply2 = new Reply2();
            Reply1 duplicateReply1 = new Reply1();
            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings
                .Empty
                .RegisterHandler<DummyChildActor1, Message1>(message1 => reply1)
                .RegisterHandler<DummyChildActor2, Message2>(message2 => reply2)
                .RegisterHandler<DummyChildActor1, Message1>(message1 => duplicateReply1);

            //act
            sut.CreateFramework<DummyParentActor>(this);

            //assert
            HandlersPassedIntoShim.ToImmutableList().Should().BeEquivalentTo(
                ImmutableDictionary<(Type, Type), Func<object, object>>
                    .Empty
                    .Add((typeof(DummyChildActor1), typeof(Message1)), message1 => duplicateReply1)
                    .Add((typeof(DummyChildActor2), typeof(Message2)), message2 => reply2)
                    .ToImmutableList(),
                options => options
                    .Using<Func<object, object>>(context => context.Subject.Invoke(null).Should().BeSameAs(context.Expectation.Invoke(null)))
                    .WhenTypeIs<Func<object, object>>());
        }

        [Fact]
        public void UnitTestFrameworkSettingsWithHandlersInDifferentInstances_CreateFramework_ConstructsTestProbeResolverWithCorrectHandlers()
        {
            //arrange
            Reply1 reply1 = new Reply1();
            Reply2 reply2 = new Reply2();
            Reply1 duplicateReply1 = new Reply1();
            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings
                .Empty
                .RegisterHandler<DummyChildActor1, Message1>(message1 => reply1)
                .RegisterHandler<DummyChildActor2, Message2>(message1 => reply2);

            UnitTestFrameworkSettings differentInstance = sut
                .RegisterHandler<DummyChildActor1, Message1>(message1 => duplicateReply1);

            //act
            sut.CreateFramework<DummyParentActor>(this);

            //assert
            HandlersPassedIntoShim.ToImmutableList().Should().BeEquivalentTo(
                ImmutableDictionary<(Type, Type), Func<object, object>>
                    .Empty
                    .Add((typeof(DummyChildActor1), typeof(Message1)), message1 => reply1)
                    .Add((typeof(DummyChildActor2), typeof(Message2)), message1 => reply2)
                    .ToImmutableList(),
                options => options
                    .Using<Func<object, object>>(context => context.Subject.Invoke(null).Should().BeSameAs(context.Expectation.Invoke(null)))
                    .WhenTypeIs<Func<object, object>>());
        }

        [Fact]
        public void UnitTestFrameworkSettingsWithHandlers_CreateFramework_ConstructsTestProbeResolverWithHandlersThatReceiveTheCorrectMessage()
        {
            //arrange
            Message1 actual = null;
            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings
                .Empty
                .RegisterHandler<DummyChildActor1, Message1>(mess =>
                {
                    actual = mess;
                    return new Reply1();
                });

            //act
            sut.CreateFramework<DummyParentActor>(this);

            //assert
            Message1 exptectedMessage = new Message1();
            HandlersPassedIntoShim[(typeof(DummyChildActor1), typeof(Message1))].Invoke(exptectedMessage);
            actual.Should().BeSameAs(exptectedMessage);
        }

        [Fact]
        public void UnitTestFrameworkSettingsWithHandlers_CreateFramework_ConstructsTestProbeResolverWithHandlersThatThrowWhenGivenTheWrongMessageType()
        {
            //arrange
            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings
                .Empty
                .RegisterHandler<DummyChildActor1, Message1>(mess => new Reply1());

            //act
            sut.CreateFramework<DummyParentActor>(this);

            //assert
            Action invokeHandlerWithWrongMessageType = () => HandlersPassedIntoShim[(typeof(DummyChildActor1), typeof(Message1))].Invoke(new Message2());
            invokeHandlerWithWrongMessageType.Should().Throw<InvalidCastException>();
        }

        [Fact]
        public void UnitTestFrameworkSettings_CreateFramework_CreatesUnitTestFrameworkWithCorrectTestKit()
        {
            //arrange
            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings.Empty;

            //act
            sut.CreateFramework<DummyParentActor>(TestKitPassedIntoSut);

            //assert
            TestKitPassedIntoShim.Should().BeSameAs(TestKitPassedIntoSut);
        }
    
        [Fact]
        public void UnitTestFrameworkSettings_CreateFramework_CreatesUnitTestFrameworkWithCorrectProps()
        {
            //arrange
            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings.Empty;

            //act
            sut.CreateFramework<DummyParentActor>(TestKitPassedIntoSut);

            //assert
            PropsPassedIntoShim.Type.Should().Be<DummyParentActor>();
        }

        [Fact]
        public void UnitTestFrameworkSettings_CreateFramework_CreatesUnitTestFrameworkWithCorrectExpectedChildrenCount()
        {
            //arrange
            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings.Empty;

            //act
            sut.CreateFramework<DummyChildActor1>(TestKitPassedIntoSut);

            //assert
            ExpectedChildrenCountPassedIntoShim.Should().Be(0);
        }

        [Fact]
        public void UnitTestFrameworkSettings_CreateFramework_OnlyCreatesOneUnitTestFramework()
        {
            //arrange
            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings.Empty;

            //act
            sut.CreateFramework<DummyParentActor>(TestKitPassedIntoSut);

            //assert
            UnitTestFrameworkCreatorCreateCount.Should().Be(1);
        }

        [Fact]
        public void UnitTestFrameworkSettings_CreateFramework_ReturnsCreatedOneUnitTestFramework()
        {
            //arrange
            UnitTestFrameworkSettings sut = UnitTestFrameworkSettings.Empty;

            //act
            UnitTestFramework<DummyParentActor> result = sut.CreateFramework<DummyParentActor>(TestKitPassedIntoSut);

            //assert
            result.Should().BeSameAs(UnitTestFrameworkReturnedFromShim);
        }
    }
}