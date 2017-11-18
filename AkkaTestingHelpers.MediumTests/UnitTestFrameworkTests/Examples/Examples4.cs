using System;
using System.Threading;
using Akka.Actor;
using Akka.DI.Core;
using Akka.TestKit.Xunit2;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.MediumTests.UnitTestFrameworkTests.Examples
{
    public class Examples4 : TestKit
    {
        public Examples4() : base(AkkaConfig.Config) { }

        public class ParentActor : ReceiveActor
        {
            public ParentActor()
            {
                Thread.Sleep(5);
                Context.ActorOf(
                    Context
                        .DI()
                        .Props<ChildActor>(), 
                    "child-1");
                Context.ActorOf(Context
                    .DI()
                    .Props<ChildActor>()
                    .WithSupervisorStrategy(new AllForOneStrategy(
                        3,
                        500,
                        exception => Directive.Escalate)), 
                    "child-2");
            }
            
            protected override SupervisorStrategy SupervisorStrategy() => 
                new OneForOneStrategy(
                    1, 
                    1000, 
                    exception => Directive.Stop);
        }

        public class ChildActor : ReceiveActor { }

        [Fact]
        public void ParentActor_Constructor_CreatesChild1WithOneForOneStrategy()
        {
            //act
            UnitTestFramework<ParentActor> framework = UnitTestFrameworkSettings
                .Empty
                .CreateFramework<ParentActor>(this, 2);
            
            //assert
            framework.ResolvedSupervisorStrategy("child-1")
                .Should().BeOfType<OneForOneStrategy>();
        }

        [Fact]
        public void ParentActor_Constructor_CreatesChild1WithCorrectMaxNumberOfRetries()
        {
            //act
            UnitTestFramework<ParentActor> framework = UnitTestFrameworkSettings
                .Empty
                .CreateFramework<ParentActor>(this, 2);

            //assert
            framework.ResolvedSupervisorStrategy("child-1")
                .As<OneForOneStrategy>().MaxNumberOfRetries
                .Should().Be(1);
        }

        [Fact]
        public void ParentActor_Constructor_CreatesChild1WithCorrectTimeout()
        {
            //act
            UnitTestFramework<ParentActor> framework = UnitTestFrameworkSettings
                .Empty
                .CreateFramework<ParentActor>(this, 2);

            //assert
            framework.ResolvedSupervisorStrategy("child-1")
                .As<OneForOneStrategy>().WithinTimeRangeMilliseconds
                .Should().Be(1000);
        }

        [Fact]
        public void ParentActor_Constructor_CreatesChild1WithCorrectDecider()
        {
            //act
            UnitTestFramework<ParentActor> framework = UnitTestFrameworkSettings
                .Empty
                .CreateFramework<ParentActor>(this, 2);

            //assert
            framework.ResolvedSupervisorStrategy("child-1")
                .As<OneForOneStrategy>().Decider.Decide(new Exception())
                .Should().Be(Directive.Stop);
        }

        [Fact]
        public void ParentActor_Constructor_CreatesChild2WithOneForOneStrategy()
        {
            //act
            UnitTestFramework<ParentActor> framework = UnitTestFrameworkSettings
                .Empty
                .CreateFramework<ParentActor>(this, 2);

            //assert
            framework.ResolvedSupervisorStrategy("child-2")
                .Should().BeOfType<AllForOneStrategy>();
        }

        [Fact]
        public void ParentActor_Constructor_CreatesChild2WithCorrectMaxNumberOfRetries()
        {
            //act
            UnitTestFramework<ParentActor> framework = UnitTestFrameworkSettings
                .Empty
                .CreateFramework<ParentActor>(this, 2);

            //assert
            framework.ResolvedSupervisorStrategy("child-2")
                .As<AllForOneStrategy>().MaxNumberOfRetries
                .Should().Be(3);
        }

        [Fact]
        public void ParentActor_Constructor_CreatesChild2WithCorrectTimeout()
        {
            //act
            UnitTestFramework<ParentActor> framework = UnitTestFrameworkSettings
                .Empty
                .CreateFramework<ParentActor>(this, 2);

            //assert
            framework.ResolvedSupervisorStrategy("child-2")
                .As<AllForOneStrategy>().WithinTimeRangeMilliseconds
                .Should().Be(500);
        }

        [Fact]
        public void ParentActor_Constructor_CreatesChild2WithCorrectDecider()
        {
            //act
            UnitTestFramework<ParentActor> framework = UnitTestFrameworkSettings
                .Empty
                .CreateFramework<ParentActor>(this, 2);

            //assert
            framework.ResolvedSupervisorStrategy("child-2")
                .As<AllForOneStrategy>().Decider.Decide(new Exception())
                .Should().Be(Directive.Escalate);
        }
    }
}