using System;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;
using FluentAssertions;
using Xunit;
using SettingsHandlers = System.Collections.Immutable.ImmutableDictionary<(System.Type, System.Type), System.Func<object, object>>;
using MappedMessageHandlers = System.Collections.Immutable.ImmutableDictionary<System.Type, System.Func<object, object>>;
using MappedHandlers = System.Collections.Immutable.ImmutableDictionary<System.Type, System.Collections.Immutable.ImmutableDictionary<System.Type, System.Func<object, object>>>;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.TestProbeHandlersMapperTests
{
    public class Map : TestBase
    {
        #region Null tests
        [Fact]
        public void TestProbeHandlersMapper_MapWithNullSettingsHandlers_ThrowsArgumentNullException()
        {
            //arrange
            TestProbeHandlersMapper sut = CreateTestProbeHandlersMapper();

            //act
            Action act = () => sut.Map(null);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }
        #endregion

        [Fact]
        public void TestProbeHandlersMapper_MapNoActors_ReturnsEmptyResult()
        {
            //arrange
            TestProbeHandlersMapper sut = CreateTestProbeHandlersMapper();

            //act
            MappedHandlers result = sut.Map(SettingsHandlers.Empty);

            //assert
            result.Should().BeEmpty();
        }
        
        [Fact]
        public void TestProbeHandlersMapper_MapASingleActorWithASingleHandler_ReturnsCorrectResult()
        {
            //arrange
            (Type actor, Type message, Func<object, object> handler) = CreateSettingsHandler();
            TestProbeHandlersMapper sut = CreateTestProbeHandlersMapper();
            
            //act
            MappedHandlers result = sut.Map(SettingsHandlers.Empty
                .Add((actor, message), handler));

            //assert
            result.ShouldAllBeEquivalentTo(MappedHandlers.Empty
                .Add(actor, MappedMessageHandlers.Empty
                    .Add(message, handler)));
        }

        [Fact]
        public void TestProbeHandlersMapper_MapASingleActorWithMultipleHandlers_ReturnsCorrectResult()
        {
            //arrange
            (Type actor, Type message1, Func<object, object> handler1) = CreateSettingsHandler();
            (_, Type message2, Func<object, object> handler2) = CreateSettingsHandler();
            TestProbeHandlersMapper sut = CreateTestProbeHandlersMapper();

            //act
            MappedHandlers result = sut.Map(SettingsHandlers.Empty
                .Add((actor, message1), handler1)
                .Add((actor, message2), handler2));

            //assert
            result.ShouldAllBeEquivalentTo(MappedHandlers.Empty
                .Add(actor, MappedMessageHandlers.Empty
                    .Add(message1, handler1)
                    .Add(message2, handler2)));
        }

        [Fact]
        public void TestProbeHandlersMapper_MapMultipleActorsWithSingleHandlers_ReturnsCorrectResult()
        {
            //arrange
            (Type actor1, Type message1, Func<object, object> handler1) = CreateSettingsHandler();
            (Type actor2, Type message2, Func<object, object> handler2) = CreateSettingsHandler();
            TestProbeHandlersMapper sut = CreateTestProbeHandlersMapper();

            //act
            MappedHandlers result = sut.Map(SettingsHandlers.Empty
                .Add((actor1, message1), handler1)
                .Add((actor2, message2), handler2));

            //assert
            result.ShouldAllBeEquivalentTo(
                MappedHandlers.Empty
                    .Add(actor1, MappedMessageHandlers.Empty
                        .Add(message1, handler1))
                    .Add(actor2, MappedMessageHandlers.Empty
                        .Add(message2, handler2)));
        }
        
        [Fact]
        public void TestProbeHandlersMapper_MapMultipleActorsWithMultipleHandlers_ReturnsCorrectResult()
        {
            //arrange
            (Type actor1, Type message1, Func<object, object> handler1) = CreateSettingsHandler();
            (_, Type message2, Func<object, object> handler2) = CreateSettingsHandler();
            (_, Type message3, Func<object, object> handler3) = CreateSettingsHandler();
            (Type actor2, Type message4, Func<object, object> handler4) = CreateSettingsHandler();
            (_, Type message5, Func<object, object> handler5) = CreateSettingsHandler();
            TestProbeHandlersMapper sut = CreateTestProbeHandlersMapper();

            //act
            MappedHandlers result = sut.Map(SettingsHandlers.Empty
                .Add((actor1, message1), handler1)
                .Add((actor1, message2), handler2)
                .Add((actor1, message3), handler3)
                .Add((actor2, message4), handler4)
                .Add((actor2, message5), handler5));

            //assert
            result.ShouldAllBeEquivalentTo(MappedHandlers.Empty
                .Add(actor1, MappedMessageHandlers.Empty
                    .Add(message1, handler1)
                    .Add(message2, handler2)
                    .Add(message3, handler3))
                .Add(actor2, MappedMessageHandlers.Empty
                    .Add(message4, handler4)
                    .Add(message5, handler5)));
        }

        [Fact]
        public void TestProbeHandlersMapper_MapASingleActorWithASingleHandlerAndAnActorWithMultipleHandlers_ReturnsCorrectResult()
        {
            //arrange
            (Type actor1, Type message1, Func<object, object> handler1) = CreateSettingsHandler();
            (_, Type message2, Func<object, object> handler2) = CreateSettingsHandler();
            (_, Type message3, Func<object, object> handler3) = CreateSettingsHandler();
            (Type actor2, Type message4, Func<object, object> handler4) = CreateSettingsHandler();
            TestProbeHandlersMapper sut = CreateTestProbeHandlersMapper();
            
            //act
            MappedHandlers result = sut.Map(SettingsHandlers.Empty
                .Add((actor1, message1), handler1)
                .Add((actor1, message2), handler2)
                .Add((actor1, message3), handler3)
                .Add((actor2, message4), handler4));

            //assert
            result.ShouldAllBeEquivalentTo(MappedHandlers.Empty
                .Add(actor1, MappedMessageHandlers.Empty
                    .Add(message1, handler1)
                    .Add(message2, handler2)
                    .Add(message3, handler3))
                .Add(actor2, MappedMessageHandlers.Empty
                    .Add(message4, handler4)));
        }
    }
}