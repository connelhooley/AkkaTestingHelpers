using Akka.Actor;
using Akka.TestKit;

namespace ConnelHooley.AkkaTestingHelpers.Helpers.Abstract
{
    internal interface ITellChildWaiter
    {
        void TellMessage<TMessage>(
            IWaiter childWaiter, 
            TestKitBase testKit, 
            IActorRef recipient, 
            TMessage message, 
            int waitForChildrenCount, 
            IActorRef sender = null);
    }
}