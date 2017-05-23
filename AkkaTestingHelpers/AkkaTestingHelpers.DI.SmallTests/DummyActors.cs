using System.Collections.Generic;
using System.Threading;
using Akka.Actor;
using Akka.DI.Core;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests
{
    public class ParentActor<TChildActor> : ReceiveActor where TChildActor : ActorBase
    {
        public ParentActor(params string[] childrenNames)
        {
            CreateChildren(childrenNames);
            Receive<IEnumerable<string>>(names => CreateChildren(names));
            ReceiveAny(o =>
            {
                foreach (IActorRef childRef in Context.GetChildren())
                {
                    childRef.Forward(o);
                }
            });
        }
        
        private static void CreateChildren(IEnumerable<string> childrenNames)
        {
            foreach (string childName in childrenNames)
            {
                Context.ActorOf(Context.DI().Props<TChildActor>(), childName);
            }
        }
    }

    public class ReplyChildActor : ReceiveActor
    {
        public ReplyChildActor()
        {
            Thread.Sleep(500);
            ReceiveAny(o => Context.Sender.Tell(o));
        }
    }

    public class DependancyChildActor : ReceiveActor
    {
        public DependancyChildActor(IDependancy dependancy)
        {
            Thread.Sleep(500);
            ReceiveAny(dependancy.SetResut);
        }
    }

    public interface IDependancy
    {
        void SetResut(object message);
    }
}