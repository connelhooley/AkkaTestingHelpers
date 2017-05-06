using Akka.DI.Core;
using Akka.TestKit;

namespace ConnelHooley.AkkaTestingHelpers.DI.TestProbeDependancyResolver
{
    public static class TestKitBaseExtensions
    {
        public static Resolver CreateTestProbeResolver(this TestKitBase @this, Settings settings)
        {
            Resolver resolver = new Resolver(@this, settings);
            @this.Sys.AddDependencyResolver(resolver);
            return resolver;
        }
    }
}