using System;
using System.Linq;
using Akka.Actor;
using Akka.TestKit.Xunit2;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.MediumTests.UnitTestFrameworkTests
{
    public class TellMessageAndWaitForExceptions : TestKit
    {
        public TellMessageAndWaitForExceptions() : base(AkkaConfig.Config) { }

        [Fact]
        public void UnitTestFramework_WaitsForExceptionsThrownWhenProcessingMessages()
        {
            //arrange
            const int initialChildCount = 0;
            const int exceptionCount = 2;
            Exception exception1 = new ArithmeticException();
            Exception exception2 = new InvalidCastException();

            UnitTestFramework<ThrowingParentActor> sut = UnitTestFrameworkSettings
                .Empty
                .CreateFramework<ThrowingParentActor>(this, Props.Create(() => new ThrowingParentActor(exception1, exception2)), initialChildCount);

            //act
            sut.TellMessageAndWaitForExceptions(new object(), exceptionCount);

            //assert
            sut.UnhandledExceptions.Should().Equal(exception1, exception2);
        }

        [Fact]
        public void UnitTestFramework_TimesOutWhenWaitingForExceptionsWithAnExpectedChildCountThatIsTooHigh()
        {
            //arrange
            const int initialChildCount = 0;
            const int exceptionCount = 2;
            Exception exception1 = new ArithmeticException();
            Exception exception2 = new InvalidCastException();

            UnitTestFramework<ThrowingParentActor> sut = UnitTestFrameworkSettings
                .Empty
                .CreateFramework<ThrowingParentActor>(this, Props.Create(() => new ThrowingParentActor(exception1, exception2)), initialChildCount);

            //act
            Action act = () => sut.TellMessageAndWaitForExceptions(new object(), exceptionCount + 1);

            //assert
            act.Should().Throw<TimeoutException>();
        }
    }
}