using System;
using System.Threading;
using Akka.Actor;
using Akka.DI.Core;

namespace ConnelHooley.AkkaTestingHelpers.MediumTests.BasicResolverSettingsTests
{
    #region Parent actor to be resolved by resolver

    public class ParentActor : ReceiveActor
    {
        public ParentActor()
        {
            var child = Context.ActorOf(Context.DI().Props<ChildActor>(), "child");
            ReceiveAny(o => child.Forward(o));
        }
    }

    #endregion

    #region Child actors to be resolved by resolver

    public class ChildActor : ReceiveActor
    {
        public static Guid Token = Guid.NewGuid();

        public ChildActor()
        {
            Thread.Sleep(5);
            ReceiveAny(o => Context.Sender.Tell(Token));
        }

        public ChildActor(IDependancy dependancy)
        {
            Thread.Sleep(5);
            ReceiveAny(o => dependancy.SetResut(Token));
        }
    }

    public class EmptyChildActor : ReceiveActor { }

    #endregion

    #region Interface to be mocked and injected into children

    public interface IDependancy
    {
        void SetResut(object message);
    }

    #endregion
}