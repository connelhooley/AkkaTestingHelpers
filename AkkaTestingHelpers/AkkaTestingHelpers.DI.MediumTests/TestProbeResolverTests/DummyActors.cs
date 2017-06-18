using System;
using System.Threading;
using Akka.Actor;
using Akka.DI.Core;

namespace ConnelHooley.AkkaTestingHelpers.DI.MediumTests.TestProbeResolverTests
{
    #region Parent actor to be resolved by resolver

    internal class ParentActor : ReceiveActor
    {
        public ParentActor()
        {
            Thread.Sleep(100);
            Become(Ready);
        }

        public ParentActor(Type initalChildrenType, int initalChildrenCount)
        {
            Thread.Sleep(100);
            CreateChildren(initalChildrenType, initalChildrenCount);
            Become(Ready);
        }

        private void Ready()
        {
            Receive<CreateChildren>(message => CreateChildren(message.Type, message.Count));
            Receive<CreateChild>(message => Context.ActorOf(Context.DI().Props(message.Type), message.Name));
            Receive<TellAllChildren>(message =>
            {
                foreach (IActorRef childRef in Context.GetChildren())
                {
                    childRef.Forward(message.Message);
                }
            });
            Receive<TellChild>(message => Context.Child(message.Name).Forward(message.Message));
            Receive<TellParent>(message => Context.Parent.Forward(message.Message));
        }
        
        private static void CreateChildren(Type childType, int childCount)
        {
            for (int i = 0; i < childCount; i++)
            {
                Context.ActorOf(Context.DI().Props(childType));
            }
        }
    }

    #endregion

    #region Child actors to be resolved by resolver

    internal class ReplyChildActor1 : ReceiveActor
    {
        public ReplyChildActor1()
        {
            Thread.Sleep(100);
            ReceiveAny(o => Context.Sender.Tell(0));
        }
    }

    internal class ReplyChildActor2 : ReceiveActor
    {
        public ReplyChildActor2()
        {
            Thread.Sleep(100);
            ReceiveAny(o => Context.Sender.Tell(0));
        }
    }

    #endregion

    #region Messages to be sent to parent actors to drive tests

    internal class CreateChildren
    {
        public Type Type { get; }
        public int Count { get; }

        public CreateChildren(Type type, int count)
        {
            Type = type;
            Count = count;
        }
    }

    internal class CreateChild
    {
        public string Name { get; }
        public Type Type { get; }

        public CreateChild(string name, Type type)
        {
            Name = name;
            Type = type;
        }
    }

    internal class TellAllChildren
    {
        public object Message { get; }

        public TellAllChildren(object message)
        {
            Message = message;
        }
    }

    internal class TellChild
    {
        public string Name { get; }
        public object Message { get; }

        public TellChild(string name, object message)
        {
            Name = name;
            Message = message;
        }
    }

    internal class TellParent
    {
        public object Message { get; }

        public TellParent(object message)
        {
            Message = message;
        }
    }

    #endregion
}