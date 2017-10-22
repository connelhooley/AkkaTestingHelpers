using System;
using Akka.TestKit.Xunit2;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.TestProbeHandlersMapperTests
{
    public class TestBase : TestKit
    {
        private readonly Func<Type> _typeGenerator;

        public TestBase() : base(AkkaConfig.Config)
        {
            _typeGenerator = TestUtils.RandomTypeGenerator();
        }

        internal (Type, Type, Func<object, object>) CreateSettingsHandler() => 
            (_typeGenerator(), _typeGenerator(), TestUtils.Create<Func<object, object>>());

        internal TestProbeHandlersMapper CreateTestProbeHandlersMapper() => 
            new TestProbeHandlersMapper();
    }
}