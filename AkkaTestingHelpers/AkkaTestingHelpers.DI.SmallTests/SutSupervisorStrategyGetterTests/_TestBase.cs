﻿using Akka.Actor;
using Akka.TestKit.Xunit2;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.SutSupervisorStrategyGetterTests
{
    public class TestBase : TestKit
    {
        protected ActorBase ActorWithNonDefaultSupervisorStrategy;
        protected ActorBase ActorWithDefaultSupervisorStrategy;
        protected SupervisorStrategy SupervisorStrategy;

        public TestBase() : base(AkkaConfig.Config)
        {
            SupervisorStrategy = new AllForOneStrategy(
                TestUtils.Create<int>(),
                TestUtils.Create<int>(),
                exception => TestUtils.Create<Directive>());

            ActorWithNonDefaultSupervisorStrategy =
                ActorOfAsTestActorRef<DummyActor1>(
                    Props.Create(() => new DummyActor1(SupervisorStrategy)))
                .UnderlyingActor;

            ActorWithDefaultSupervisorStrategy =
                ActorOfAsTestActorRef<DummyActor2>(
                        Props.Create(() => new DummyActor2()))
                    .UnderlyingActor;
        }

        internal SutSupervisorStrategyGetter CreateSutSupervisorStrategyGetter() => new SutSupervisorStrategyGetter();

        protected class DummyActor1 : ReceiveActor
        {
            private readonly SupervisorStrategy _supervisorStrategy;

            public DummyActor1(SupervisorStrategy supervisorStrategy) => 
                _supervisorStrategy = supervisorStrategy;

            protected override SupervisorStrategy SupervisorStrategy() => 
                _supervisorStrategy;
        }

        protected class DummyActor2 : ReceiveActor { }
    }
}