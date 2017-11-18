//using System;
//using System.Reflection;
//using System.Threading;
//using System.Threading.Tasks;
//using Akka.Actor;
//using Akka.Event;
//using Akka.TestKit.Xunit2;
//using FluentAssertions;
//using Xunit;

//namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests
//{
//    public class Exploratory : TestKit
//    {
//        [Fact]
//        public void Test1()
//        {
//            var actor = ActorOfAsTestActorRef<ParentActor>(Props.Create<ParentActor>());
//            Watch(actor);
//            actor.Tell('a');
//            ExpectTerminated(actor);
//        }

//        [Fact]
//        public async Task Test2()
//        {
//            var actor = ActorOfAsTestActorRef<ParentActor>(Props.Create<ParentActor>());

//            MethodInfo method = actor.UnderlyingActor.GetType().GetMethod(
//                "SupervisorStrategy", 
//                BindingFlags.NonPublic | BindingFlags.Instance);
//            var ss = (SupervisorStrategy)method.Invoke(actor.UnderlyingActor, new object[0]);

//            var child1 = await ActorSelection(actor, "child1").ResolveOne(RemainingOrDefault);
//            SupervisorStrategy ss1 = await child1.Ask<SupervisorStrategy>(new ChildActor.GetSupervisorStrategy());
//            Directive result1 = ss1.Decider.Decide(new InvalidOperationException());

//            var child2 = await ActorSelection(actor, "child2").ResolveOne(RemainingOrDefault);
//            SupervisorStrategy ss2 = await child2.Ask<SupervisorStrategy>(new ChildActor.GetSupervisorStrategy());
//            Directive? result2 = ss2?.Decider?.Decide(new InvalidOperationException());
            
//            InvalidOperationException exception = null;
//            EventFilter
//                .Custom(logEvent =>
//                {
//                    if (logEvent is Error error)
//                    {
//                        if (error.Cause is InvalidOperationException ex)
//                        {
//                            exception = ex;
//                            return true;
//                        }
//                    }
//                    return false;
//                }).ExpectOne(() => child1.Tell(""));
//            exception.Message.Should().Be("Hello world");
//        }
        
//        public class ParentActor : ReceiveActor
//        {
//            public ParentActor()
//            {
//                var child1 = Context.ActorOf(ChildActor.Props.WithSupervisorStrategy(
//                    new AllForOneStrategy(3, 1000, exception => Directive.Escalate)), "child1");
//                var child2 = Context.ActorOf(ChildActor.Props, "child2");
//                ReceiveAny(o => child1.Forward(o));
                
//            }

//            protected override SupervisorStrategy SupervisorStrategy() => 
//                new AllForOneStrategy(3, 1000, exception => Directive.Escalate);
//        }

//        public class ChildActor : ReceiveActor
//        {
//            public ChildActor()
//            {
//                Receive<string>(message =>
//                {
//                    Context.GetLogger().Error(new InvalidOperationException(), "");
//                    //throw new InvalidOperationException("Hello world");
//                });
//                Receive<GetSupervisorStrategy>(message =>
//                {
//                    Context.Sender.Tell(Context.Props.SupervisorStrategy);
//                });
//            }

//            public static Props Props => Props.Create<ChildActor>();

//            public class GetSupervisorStrategy { }
//        }
//    }

//    public class ExtensionId : IExtensionId
//    {
//        public object Apply(ActorSystem system)
//        {
//            throw new NotImplementedException();
//        }

//        public object Get(ActorSystem system)
//        {
//            throw new NotImplementedException();
//        }

//        public object CreateExtension(ExtendedActorSystem system)
//        {
//            IActorRef x = system.Guardian.GetChild(new[] {"user"});
//            return system;
//        }

//        public Type ExtensionType => typeof(ExtensionId);
//    }
//}