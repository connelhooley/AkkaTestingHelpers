using System;
using System.Collections.Immutable;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.DI.Actors.Abstract;
using EitherHandler = Akka.Util.Either<System.Action<object>, System.Func<object, object>>;
using NonReplyingHandler = Akka.Util.Left<System.Action<object>, System.Func<object, object>>;
using ReplyingHandler = Akka.Util.Right<System.Action<object>, System.Func<object, object>>;

namespace ConnelHooley.AkkaTestingHelpers.DI
{
    public sealed class ActorFake : IRegisterableActorFake
    {
        private readonly ImmutableDictionary<Type, EitherHandler> _handlers;
        private TestProbe _testProbe;

        public TestProbe TestProbe => _testProbe ?? throw new InvalidOperationException("Fake actor has not been registered by resolver");

        private ActorFake(ImmutableDictionary<Type, EitherHandler> handlers)
        {
            _handlers = handlers;
        }

        public static ActorFake Create() => new ActorFake(ImmutableDictionary<Type, EitherHandler>.Empty);

        public ActorFake RegisterNonReplyingHandler<TMessage>(Action<TMessage> handler) =>
            new ActorFake(
                _handlers.SetItem(
                    typeof(TMessage),
                    new NonReplyingHandler(mess => handler((TMessage)mess))));

        public ActorFake RegisterReplyingHandler<TMessage>(Func<TMessage, object> handler) =>
            new ActorFake(
                _handlers.SetItem(
                    typeof(TMessage),
                    new ReplyingHandler(mess => handler((TMessage)mess))));

        //todo test
        void IRegisterableActorFake.RegisterActor(ITestProbeActor probeActor)
        {
            probeActor.SetHandlers(_handlers);
            _testProbe = probeActor.TestProbe;
        }
    }
}
