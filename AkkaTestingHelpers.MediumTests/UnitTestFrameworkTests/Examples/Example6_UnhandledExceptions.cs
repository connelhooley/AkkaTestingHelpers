using System;
using System.Linq;
using System.Threading;
using Akka.Actor;
using Akka.TestKit.Xunit2;
using FluentAssertions;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.MediumTests.UnitTestFrameworkTests.Examples
{
    public class Example6_UnhandledExceptions : TestKit
    {
        public Example6_UnhandledExceptions() : base(AkkaConfig.Config) { }

        public class ParentActor : ReceiveActor
        {
            public ParentActor()
            {
                Receive<Exception>(message => {
                    Thread.Sleep(500);
                    throw message;
                });
            }
        }

        [Fact]
        public void ParentActor_ReceiveExceptionMessage_ThrowsSameException()
        {
            //arrange
            Exception message = new ArithmeticException();
            UnitTestFramework<ParentActor> framework = UnitTestFrameworkSettings
                .Empty
                .CreateFramework<ParentActor>(this);

            //act
            framework.TellMessageAndWaitForException(message);

            //assert
            framework.UnhandledExceptions.First().Should().BeSameAs(message);
        }
    }
}