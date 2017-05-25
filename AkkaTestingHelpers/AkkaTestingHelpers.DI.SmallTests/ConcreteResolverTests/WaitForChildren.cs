using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Abstract;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.ConcreteResolverTests
{
    internal class WaitForChildren : TestBase
    {
        [Test]
        public void ConcreteResolver_WaitForChildren_StartsWaitingForChildren()
        {
            //arrange
            ConcreteResolver sut = CreateConcreteResolver(ConcreteResolverSettings.Empty);

            //act
            sut.WaitForChildren(() => {}, ExpectedChildrenCount);

            //assert
            ChildWaiterMock.Verify(
                waiter => waiter.Start(this, ExpectedChildrenCount),
                Times.Once);
        }

        [Test]
        public void ConcreteResolver_WaitForChildren_WaitsForChildren()
        {
            //arrange
            ConcreteResolver sut = CreateConcreteResolver(ConcreteResolverSettings.Empty);

            //act
            sut.WaitForChildren(() => { }, ExpectedChildrenCount);

            //assert
            ChildWaiterMock.Verify(
                waiter => waiter.Wait(),
                Times.Once);
        }

        [Test]
        public void ConcreteResolver_WaitForChildren_CallsAction()
        {
            //arrange
            ConcreteResolver sut = CreateConcreteResolver(ConcreteResolverSettings.Empty);
            bool isCalled = false;

            //act
            sut.WaitForChildren(() => isCalled = true, ExpectedChildrenCount);

            //assert
            isCalled.Should().BeTrue();
        }

        [Test]
        public void ConcreteResolver_WaitForChildren_StartsWaitingForChildrenBeforeActionIsCalled()
        {
            //arrange
            ConcreteResolver sut = CreateConcreteResolver(ConcreteResolverSettings.Empty);
            //act
            sut.WaitForChildren(() => CallOrder.Add("callback"), ExpectedChildrenCount);

            //assert
            CallOrder.Should().ContainInOrder(nameof(IChildWaiter.Start), "callback");
        }
        
        [Test]
         public void ConcreteResolver_WaitForChildren_WaitsForChildrenAfterActionIsCalled()
        {
            //arrange
            ConcreteResolver sut = CreateConcreteResolver(ConcreteResolverSettings.Empty);

            //act
            sut.WaitForChildren(() => CallOrder.Add("callback"), ExpectedChildrenCount);

            //assert
            CallOrder.Should().ContainInOrder("callback", nameof(IChildWaiter.Wait));
        }
    }
}