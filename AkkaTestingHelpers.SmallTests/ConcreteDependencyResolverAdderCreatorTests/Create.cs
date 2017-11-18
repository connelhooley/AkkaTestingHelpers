using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.ConcreteDependencyResolverAdderCreatorTests
{
    public class Create : TestBase
    {
        [Fact]
        public void ConcreteDependencyResolverAdderCreator_Create_OnlyConstructsOneConcreteDependencyResolverAdderClass()
        {
            //arrange
            var sut = CreateConcreteDependencyResolverAdderConcreteDependencyResolverAdderCreator();

            //act
            sut.Create();

            //assert
            ConcreteDependencyResolverAdderConstructorCount.Should().Be(1);
        }

        [Fact]
        public void ConcreteDependencyResolverAdderCreator_Create_ReturnsConcreteDependencyResolverAdder()
        {
            //arrange
            var sut = CreateConcreteDependencyResolverAdderConcreteDependencyResolverAdderCreator();

            //act
            var result = sut.Create();

            //assert
            AssertionExtensions.Should((object) result).Be(ConstructedConcreteDependencyResolverAdder);
        }

        [Fact]
        public void ConcreteDependencyResolverAdderCreator_Create_OnlyConstructsOneDependencyResolverAdderClass()
        {
            //arrange
            var sut = CreateConcreteDependencyResolverAdderConcreteDependencyResolverAdderCreator();

            //act
            sut.Create();

            //assert
            DependencyResolverAdderConstructorCount.Should().Be(1);
        }

        [Fact]
        public void ConcreteDependencyResolverAdderCreator_Create_ConstructsConcreteDependencyResolverAdderWithDependencyResolverAdder()
        {
            //arrange
            var sut = CreateConcreteDependencyResolverAdderConcreteDependencyResolverAdderCreator();

            //act
            sut.Create();

            //assert
            AssertionExtensions.Should((object) DependencyResolverAdderPassedIntoShim).BeSameAs(ConstructedDependencyResolverAdder);
        }
    }
}