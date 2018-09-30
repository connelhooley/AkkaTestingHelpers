using Akka.Actor;
using Akka.TestKit;
using ConnelHooley.AkkaTestingHelpers.Helpers.Abstract;

namespace ConnelHooley.AkkaTestingHelpers.Helpers.Concrete
{
    internal sealed class TellChildWaiter : ITellChildWaiter
    {
        public void TellMessage<TMessage>(IWaiter childWaiter, TestKitBase testKit, IActorRef recipient, TMessage message, int waitForChildrenCount, IActorRef sender = null)
        {
            childWaiter.Start(testKit, waitForChildrenCount);
            if (sender == null) recipient.Tell(message);
            else recipient.Tell(message, sender);
            childWaiter.Wait();
        }
    }
}