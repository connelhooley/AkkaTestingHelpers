using System;
using System.Threading;
using Akka.Actor;
using Akka.DI.Core;

namespace ConnelHooley.AkkaTestingHelpers.MediumTests.UnitTestFrameworkTests
{
    #region Parent actor to be resolved by resolver

    public class ParentActor : ReceiveActor
    {
        public ParentActor()
        {
            Props = Context.Props;
            Thread.Sleep(5);
            Become(Ready);
        }

        public ParentActor(Type initalChildrenType, int initalChildrenCount)
        {
            Props = Context.Props;
            Thread.Sleep(5);
            CreateChildren(initalChildrenType, initalChildrenCount);
            Become(Ready);
        }

        public Props Props { get; }

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

    public class ThrowingParentActor : ReceiveActor
    {
        private static int _restartCount = 0;
        
        public ThrowingParentActor(Exception exception1, Exception exception2)
        {
            Thread.Sleep(5);
            ReceiveAny(o =>
            {
                if (_restartCount == 0)
                {
                    Thread.Sleep(1000);
                    _restartCount++;
                    throw exception1;
                }
                else if (_restartCount == 1)
                {
                    Thread.Sleep(1000);
                    _restartCount++;
                    throw exception2;
                } else
                {
                    _restartCount++;
                }
            });
        }

        protected override void PreRestart(Exception reason, object message)
        {
            Context.Self.Tell(message);
            base.PreRestart(reason, message);
        }
    }

    public class ParentActorWithSupervisorStratery : ReceiveActor
    {
        private readonly SupervisorStrategy _supervisorStrategy;
        
        public ParentActorWithSupervisorStratery(SupervisorStrategy sutSupervisorStrategy, SupervisorStrategy childSupervisorStrategy)
        {
            _supervisorStrategy = sutSupervisorStrategy;
            Thread.Sleep(5);
            Context.ActorOf(Context.DI().Props<ReplyChildActor1>(), "ChildWithParentSupervisorStrategy");
            Context.ActorOf(Context.DI().Props<ReplyChildActor1>().WithSupervisorStrategy(childSupervisorStrategy), "ChildWithChildSupervisorStrategy");
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

        protected override SupervisorStrategy SupervisorStrategy() => _supervisorStrategy;
    }

    #endregion

    #region Child actors to be resolved by resolver

    public class ReplyChildActor1 : ReceiveActor
    {
        public ReplyChildActor1()
        {
            Thread.Sleep(5);
            ReceiveAny(o => Context.Sender.Tell(0));
        }
    }

    public class ReplyChildActor2 : ReceiveActor
    {
        public ReplyChildActor2()
        {
            Thread.Sleep(5);
            ReceiveAny(o => Context.Sender.Tell(0));
        }
    }

    #endregion

    #region Messages to be sent to parent actors to drive tests

    public class CreateChildren
    {
        public Type Type { get; }
        public int Count { get; }

        public CreateChildren(Type type, int count)
        {
            Type = type;
            Count = count;
        }
    }

    public class CreateChild
    {
        public string Name { get; }
        public Type Type { get; }

        public CreateChild(string name, Type type)
        {
            Name = name;
            Type = type;
        }
    }

    public class TellAllChildren
    {
        public object Message { get; }

        public TellAllChildren(object message)
        {
            Message = message;
        }
    }

    public class TellChild
    {
        public string Name { get; }
        public object Message { get; }

        public TellChild(string name, object message)
        {
            Name = name;
            Message = message;
        }
    }

    public class TellParent
    {
        public object Message { get; }

        public TellParent(object message)
        {
            Message = message;
        }
    }

    public class ThrowExceptions
    {
        public Exception Exception1 { get; }
        public Exception Exception2 { get; }

        public ThrowExceptions(Exception exception1, Exception exception2)
        {
            Exception1 = exception1;
            Exception2 = exception2;
        }
    }

    #endregion
}