using Akka.Actor;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.Helpers.Abstract;

namespace ConnelHooley.AkkaTestingHelpers.Helpers.Concrete
{
    internal sealed class TellWaiter : ITellWaiter
    {
        public void TellMessage<TMessage>(IWaiter waiter, TestKitBase testKit, IActorRef recipient, TMessage message, int waitForCount, IActorRef sender = null)
        {
            waiter.Start(testKit, waitForCount);
            if (sender == null) recipient.Tell(message);
            else recipient.Tell(message, sender);
            waiter.Wait();
        }
    }
}