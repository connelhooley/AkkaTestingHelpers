using System;
using System.Collections.Immutable;
using Akka.Actor;
using Akka.TestKit;
using Akka.TestKit.Xunit2;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using Moq;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.ConcreteResolverSettingsTests
{
    public class TestBase : TestKit
    {
        private readonly IDisposable _shimContext;

        internal int ConcreteDependencyResolverAdderCreatorConstructorCount;
        internal readonly Mock<IConcreteDependencyResolverAdder> ConcreteDependencyResolverAdderMock;
        protected ImmutableDictionary<Type, Func<ActorBase>> HandlersPassedIntoMock;
        
        public TestBase() : base(AkkaConfig.Config)
        {
            // Create mocks
            ConcreteDependencyResolverAdderMock = new Mock<IConcreteDependencyResolverAdder>();

            ConcreteDependencyResolverAdderMock
                .Setup(adder => adder.Add(
                    It.IsAny<TestKitBase>(),
                    It.IsAny<ImmutableDictionary<Type, Func<ActorBase>>>()))
                .Callback((TestKitBase testKit, ImmutableDictionary<Type, Func<ActorBase>> handlers) =>
                    HandlersPassedIntoMock = handlers);

            // Create shims
            _shimContext = ShimsContext.Create();

            //Set up shims
            ShimConcreteDependencyResolverAdderCreator.Constructor = @this =>
            {
                ConcreteDependencyResolverAdderCreatorConstructorCount++;
            };

            ShimConcreteDependencyResolverAdderCreator.AllInstances.Create = @this => 
                ConcreteDependencyResolverAdderMock.Object;
        }

        protected override void Dispose(bool disposing)
        {
            _shimContext.Dispose();
            base.Dispose(disposing);
        }

        protected class DummyActor1 : ReceiveActor { }

        protected class DummyActor2 : ReceiveActor { }
    }
}