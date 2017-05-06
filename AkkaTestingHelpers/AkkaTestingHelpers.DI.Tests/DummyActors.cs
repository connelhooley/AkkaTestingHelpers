using System.Linq;
using System.Threading;
using Akka.Actor;
using Akka.DI.Core;

namespace AkkaTestingHelpers.DI.Tests
{
    public class ParentActor1 : ReceiveActor
    {
        public ParentActor1()
        {
            IActorRef child = Context.ActorOf(Context.DI().Props<ChildActor1>());
            ReceiveAny(o => child.Tell(o));
        }
    }

    public class ChildActor1 : ReceiveActor
    {
        public ChildActor1(ICounter counter)
        {
            ReceiveAny(o =>
            {
                counter.Count();
            });
        }
    }

    public interface ICounter
    {
        void Count();
    }

    public class ParentActor2 : ReceiveActor
    {
        public ParentActor2()
        {
            IActorRef child = Context.ActorOf(Context.DI().Props<ChildActor2>());
            ReceiveAny(o => child.Tell(o));
        }
    }

    public class ChildActor2 : ReceiveActor
    {
        public static int Count { get; private set; }

        public ChildActor2()
        {
            ReceiveAny(o => Count++);
        }
    }

    public class ParentActor3 : ReceiveActor
    {
        public ParentActor3()
        {
            IActorRef child1 = Context.ActorOf(Context.DI().Props<ChildActor3>());
            Context.ActorOf(Context.DI().Props<ChildActor3>());
            Context.ActorOf(Context.DI().Props<ChildActor3>());
            ReceiveAny(o => child1.Tell(o));
        }
    }

    public class ChildActor3 : ReceiveActor
    {
        public static int Count { get; private set; }

        public ChildActor3()
        {
            Thread.Sleep(500);
            ReceiveAny(o => Count++);
        }
    }

    public class ParentActor4 : ReceiveActor
    {
        public ParentActor4()
        {
            Receive<int>(count =>
            {
                for (int i = 0; i < count; i++)
                {
                    Context.ActorOf(Context.DI().Props<ChildActor4>());
                }
            });
        }
    }

    public class ChildActor4 : ReceiveActor
    {
        public static int Count { get; private set; }

        public ChildActor4()
        {
            Thread.Sleep(500);
            Count++;
        }
    }
}