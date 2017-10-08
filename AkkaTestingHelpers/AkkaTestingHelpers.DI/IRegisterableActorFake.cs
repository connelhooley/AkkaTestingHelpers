using ConnelHooley.AkkaTestingHelpers.DI.Actors.Abstract;

namespace ConnelHooley.AkkaTestingHelpers.DI
{
    internal interface IRegisterableActorFake
    {
        void RegisterActor(ITestProbeActor probeActor);
    }
}