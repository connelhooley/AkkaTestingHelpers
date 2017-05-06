using System;
using Akka.Actor;
using Akka.DI.Core;
using Akka.TestKit;

namespace ConnelHooley.AkkaTestingHelpers.DI
{
    public abstract class ResolverBase : IDependencyResolver
    {
        protected readonly TestKitBase TestKit;
        private readonly object _waitingLock;
        private TestLatch _waitForChildren;
        
        protected ResolverBase(TestKitBase testKit)
        {
            TestKit = testKit;
            _waitingLock = new object();
        }

        public void WaitForAChildToBeResolved() => WaitForChildrenToBeResolved(1);

        public void WaitForChildrenToBeResolved(int expectedCount)
        {
            lock (_waitingLock)
            {
                _waitForChildren = TestKit.CreateTestLatch(expectedCount);
                _waitForChildren = null;
            }
        }

        public TestActorRef<TActor> CreateSut<TActor>(Props props, int expectedChildrenCount = 0) where TActor : ActorBase
        {
            TestActorRef<TActor> sut = TestKit.ActorOfAsTestActorRef<TActor>(props);
            if (expectedChildrenCount > 0)
            {
                WaitForChildrenToBeResolved(expectedChildrenCount);
            }
            return sut;
        }

        protected void ResolvedChild() => _waitForChildren?.CountDown();
        
        protected abstract Func<ActorBase> Resolve(Type actorType);

        #region IDependencyResolver implementation

        Func<ActorBase> IDependencyResolver.CreateActorFactory(Type actorType) => 
            Resolve(actorType);

        Type IDependencyResolver.GetType(string actorName) => 
            throw new NotImplementedException();

        Props IDependencyResolver.Create<TActor>() => 
            throw new NotImplementedException();

        Props IDependencyResolver.Create(Type actorType) => 
            throw new NotImplementedException();

        void IDependencyResolver.Release(ActorBase actor)
        {
             //No implementation needed
        }

        #endregion
    }
}