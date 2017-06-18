using System;
using System.Threading;
using Akka.Actor;
using Akka.DI.Core;

namespace ConnelHooley.AkkaTestingHelpers.DI.MediumTests.ConcreteResolverTests
{
    #region Parent actor to be resolved by resolver

    internal class ParentActor : ReceiveActor
    {
        private int _childName;

        public ParentActor(int initalCount)
        {
            Thread.Sleep(5);
            CreateChildren(initalCount);
            Receive<int>(count => CreateChildren(count));
            ReceiveAny(o =>
            {
                foreach (IActorRef childRef in Context.GetChildren())
                {
                    childRef.Forward(o);
                }
            });
        }
        
        private void CreateChildren(int childCount)
        {
            for (int i = 0; i < childCount; i++)
            {
                Context.ActorOf(Context.DI().Props<ChildActor>(), _childName++.ToString());
            }
        }
    }

    #endregion

    #region Child actors to be resolved by resolver

    internal class ChildActor : ReceiveActor
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

    internal class EmptyChildActor : ReceiveActor { }

    #endregion

    #region Interface to be mocked and injected into children

    public interface IDependancy
    {
        void SetResut(object message);
    }

    #endregion
}