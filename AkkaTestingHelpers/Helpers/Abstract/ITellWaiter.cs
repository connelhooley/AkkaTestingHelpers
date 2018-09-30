using Akka.Actor;
using Akka.TestKit;

namespace ConnelHooley.AkkaTestingHelpers.Helpers.Abstract
{
    internal interface ITellWaiter
    {
        void TellMessage<TMessage>(
            IWaiter waiter,
            TestKitBase testKit,
            IActorRef recipient,
            TMessage message,
            int waitForCount,
            IActorRef sender = null);
    }
}