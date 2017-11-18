using Akka.Actor;
using Akka.DI.Core;
using Akka.TestKit.Xunit2;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.MediumTests.UnitTestFrameworkTests.Examples
{
    public class Examples1 : TestKit
    {
        public Examples1() : base(AkkaConfig.Config) { }

        public class ChildActor : ReceiveActor { }

        public class ParentActor : ReceiveActor
        {
            public ParentActor()
            {
                var child1 = Context.ActorOf(Context.DI().Props<ChildActor>(), "child-actor-1");
                var child2 = Context.ActorOf(Context.DI().Props<ChildActor>(), "child-actor-2");
                child1.Tell("hello actor 1");
                child2.Tell(42);
            }
        }

        [Fact]
        public void ParentActor_Constructor_CreatesChildWithCorrectTypeAndName()
        {
            //act
            UnitTestFramework<ParentActor> framework = UnitTestFrameworkSettings
                .Empty
                .CreateFramework<ParentActor>(this, 2);
            
            //assert
            framework
                .ResolvedType("child-actor-1")
                .Should().Be<ChildActor>();
        }

        [Fact]
        public void ParentActor_Constructor_SendsChildCorrectMessage()
        {
            //act
            UnitTestFramework<ParentActor> framework = UnitTestFrameworkSettings
                .Empty
                .CreateFramework<ParentActor>(this, 2);

            //assert
            framework
                .ResolvedTestProbe("child-actor-1")
                .ExpectMsg("hello actor 1");
        }
    }
}