using System;
using Akka.Actor;
using Akka.TestKit.Xunit2;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;
using Moq;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.DependencyResolverAdderTests
{
    public class TestBase : TestKit
    {
        internal Func<Type, ActorBase> ActorFactory;
        internal Type TypeGivenToFactory;
        internal ActorBase ReturnedActor;

        public TestBase() : base(AkkaConfig.Config)
        {
            // Create objects passed into sut methods
            ReturnedActor = new Mock<ActorBase>().Object;
            ActorFactory = type =>
            {
                TypeGivenToFactory = type;
                return ReturnedActor;
            };
        }

        internal DependencyResolverAdder CreateDependencyResolverAdder() => new DependencyResolverAdder();
        
        protected class DummyActor : ReceiveActor { }
    }
}