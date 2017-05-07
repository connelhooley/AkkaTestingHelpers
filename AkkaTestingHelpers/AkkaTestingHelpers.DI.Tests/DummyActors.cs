using System;
using System.Collections.Generic;
using System.Threading;
using Akka.Actor;
using Akka.DI.Core;

namespace ConnelHooley.AkkaTestingHelpers.DI.Tests
{
    public static class ChildWithDependancy
    {
        public class ParentActor : ReceiveActor
        {
            public IActorRef Child { get; }

            public ParentActor()
            {
                Child = Context.ActorOf(Context.DI().Props<ChildActor>());
            }
        }

        public class ChildActor : ReceiveActor
        {
            public ChildActor(Guid childId, IDependancy dependancy)
            {
                Thread.Sleep(500);
                ReceiveAny(o =>
                {
                    dependancy.SetResut(childId);
                });
            }
        }

        public interface IDependancy
        {
            void SetResut(Guid childId);
        }
    }

    public static class ChildWithoutDependancy
    {
        public class ParentActor : ReceiveActor
        {
            public IActorRef Child { get; }

            public ParentActor()
            {
                Child = Context.ActorOf(Context.DI().Props<ChildActor>());
            }
        }

        public class ChildActor : ReceiveActor
        {
            public ChildActor()
            {
                Thread.Sleep(500);
                ReceiveAny(o => Context.Sender.Tell(o));
            }
        }
    }
    
    public static class ParentThatCreatesManyChildren
    {
        public class ParentActor : ReceiveActor
        {
            public List<IActorRef> Children { get; }

            public ParentActor(int initialChildren)
            {
                Children = new List<IActorRef>();
                for (int i = 0; i < initialChildren; i++)
                {
                    Children.Add(Context.ActorOf(Context.DI().Props<ChildActor>()));
                }
                Receive<int>(newChildren =>
                {
                    for (int i = 0; i < newChildren; i++)
                    {
                        Children.Add(Context.ActorOf(Context.DI().Props<ChildActor>()));
                    }
                });
            }
        }

        public class ChildActor : ReceiveActor
        {
            public ChildActor()
            {
                Thread.Sleep(500);
                ReceiveAny(o => Context.Sender.Tell(o));
            }
        }
    }
}