using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract;

namespace ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete
{
    internal class ConcreteDependencyResolverAdderCreator
    {
        public IConcreteDependencyResolverAdder Create() => new ConcreteDependencyResolverAdder(new DependencyResolverAdder());
    }
}