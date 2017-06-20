using System;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;
using Xunit;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.TestProbeHandlersMapperTests
{
    internal class TestBase
    {
        private readonly Func<Type> _typeGenerator;

        public TestBase()
        {
            _typeGenerator = TestUtils.RandomTypeGenerator();
        }

        protected (Type, Type, Func<object, object>) CreateSettingsHandler()
        {
            return (_typeGenerator(), _typeGenerator(), TestUtils.Create<Func<object, object>>());
        }

        protected TestProbeHandlersMapper CreateTestProbeHandlersMapper() => 
            new TestProbeHandlersMapper();
    }
}