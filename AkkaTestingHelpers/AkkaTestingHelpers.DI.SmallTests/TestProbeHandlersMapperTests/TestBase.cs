using System;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.TestProbeHandlersMapperTests
{
    public class TestBase
    {
        private readonly Func<Type> _typeGenerator;

        public TestBase()
        {
            _typeGenerator = TestUtils.RandomTypeGenerator();
        }

        internal (Type, Type, Func<object, object>) CreateSettingsHandler()
        {
            return (_typeGenerator(), _typeGenerator(), TestUtils.Create<Func<object, object>>());
        }

        internal TestProbeHandlersMapper CreateTestProbeHandlersMapper() => 
            new TestProbeHandlersMapper();
    }
}