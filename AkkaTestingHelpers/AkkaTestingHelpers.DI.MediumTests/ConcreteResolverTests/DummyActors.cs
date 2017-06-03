using System;
using System.Threading;
using Akka.Actor;
using Akka.DI.Core;

namespace ConnelHooley.AkkaTestingHelpers.DI.MediumTests.ConcreteResolverTests
{
    public class ParentActor : ReceiveActor
    {
        private int _childName;

        public ParentActor(int initalCount)
        {
            Thread.Sleep(100);
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

    public class ChildActor : ReceiveActor
    {
        public static Guid Token = Guid.NewGuid();

        public ChildActor()
        {
            Thread.Sleep(100);
            ReceiveAny(o => Context.Sender.Tell(Token));
        }

        public ChildActor(IDependancy dependancy)
        {
            Thread.Sleep(100);
            ReceiveAny(o => dependancy.SetResut(Token));
        }
    }

    public class EmptyChildActor : ReceiveActor { }

    public interface IDependancy
    {
        void SetResut(object message);
    }
}