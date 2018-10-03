using Akka.Actor;
using Akka.DI.Core;
using Akka.TestKit.Xunit2;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.MediumTests.UnitTestFrameworkTests.Examples
{
    public class Example1_ChildTestProbes : TestKit
    {
        public Example1_ChildTestProbes() : base(AkkaConfig.Config) { }

        public class ChildActor : ReceiveActor { }

        public class SutActor : ReceiveActor
        {
            public SutActor()
            {
                var child1 = Context.ActorOf(Context.DI().Props<ChildActor>(), "child-actor-1");
                var child2 = Context.ActorOf(Context.DI().Props<ChildActor>(), "child-actor-2");
                child1.Tell("hello actor 1");
                child2.Tell(42);
            }
        }

        [Fact]
        public void SutActor_Constructor_CreatesChildWithCorrectTypeAndName()
        {
            //act
            UnitTestFramework<SutActor> framework = UnitTestFrameworkSettings
                .Empty
                .CreateFramework<SutActor>(this, 2);
            
            //assert
            framework
                .ResolvedType("child-actor-1")
                .Should().Be<ChildActor>();
        }

        [Fact]
        public void SutActor_Constructor_SendsChildCorrectMessage()
        {
            //act
            UnitTestFramework<SutActor> framework = UnitTestFrameworkSettings
                .Empty
                .CreateFramework<SutActor>(this, 2);

            //assert
            framework
                .ResolvedTestProbe("child-actor-1")
                .ExpectMsg("hello actor 1");
        }
    }
}