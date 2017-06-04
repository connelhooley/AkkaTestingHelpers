using System;
using FluentAssertions;
using NUnit.Framework;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.TestProbeResolverTests
{
    internal class ResolvedType : TestBase
    {
        [Test]
        public void TestProbeResolver_ResolvedTypeWithNullParentActor_ThrowsArgumentNullException()
        {
            //arrange
            TestProbeResolver sut = CreateTestProbeResolver();

            //act
            Action act = () => sut.ResolvedType(null, ChildName);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void TestProbeResolver_ResolvedTypeWithNullChildName_ThrowsArgumentNullException()
        {
            //arrange
            TestProbeResolver sut = CreateTestProbeResolver();

            //act
            Action act = () => sut.ResolvedType(TestActor, null);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void TestProbeResolver_ResolvedTypeWithNullParentActorAndChildName_ThrowsArgumentNullException()
        {
            //arrange
            TestProbeResolver sut = CreateTestProbeResolver();

            //act
            Action act = () => sut.ResolvedType(null, null);

            //assert
            act.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void TestProbeResolver_ResolvedType_ReturnsCorrectProbe()
        {
            //arrange
            TestProbeResolver sut = CreateTestProbeResolver();

            //act
            Type result = sut.ResolvedType(TestActor, ChildName);

            //assert
            result.Should().BeSameAs(ResolvedType);
        }
    }
}