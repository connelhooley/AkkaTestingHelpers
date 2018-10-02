using Akka.Actor;
using Akka.TestKit.Xunit2;
using Xunit;
using Akka.TestKit;
using FluentAssertions;

namespace ConnelHooley.AkkaTestingHelpers.MediumTests.UnitTestFrameworkTests
{
    public class Sut : TestKit
    {
        public Sut() : base(AkkaConfig.Config) { }

        [Fact]
        public void UnitTestFramework_SutIsCreatedUsingProps()
        {
            //arrange
            Props expected = Props.Create(() => new ParentActor());
            UnitTestFramework <ParentActor> sut = UnitTestFrameworkSettings
                .Empty
                .CreateFramework<ParentActor>(this, expected);

            //act
            TestActorRef<ParentActor> result = sut.Sut;

            //assert
            Props actual = result.UnderlyingActor.Props;
            actual.Type.Should().BeSameAs(expected.Type);
        }
    }
}
