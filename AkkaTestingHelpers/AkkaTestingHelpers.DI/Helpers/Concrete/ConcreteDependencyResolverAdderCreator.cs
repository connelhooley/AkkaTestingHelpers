using ConnelHooley.AkkaTestingHelpers.Helpers.Abstract;

namespace ConnelHooley.AkkaTestingHelpers.Helpers.Concrete
{
    internal class ConcreteDependencyResolverAdderCreator
    {
        public IConcreteDependencyResolverAdder Create() => new ConcreteDependencyResolverAdder(new DependencyResolverAdder());
    }
}