using System;
using Akka.TestKit.Xunit2;
using ConnelHooley.AkkaTestingHelpers.Helpers.Concrete;
using ConnelHooley.TestHelpers;

namespace ConnelHooley.AkkaTestingHelpers.SmallTests.TestProbeHandlersMapperTests
{
    public class TestBase : TestKit
    {
        private readonly Func<Type> _typeGenerator;

        public TestBase() : base(AkkaConfig.Config)
        {
            _typeGenerator = TestHelper.GetRandomTypeGenerator();
        }

        internal (Type, Type, Func<object, object>) CreateSettingsHandler() => 
            (_typeGenerator(), _typeGenerator(), TestHelper.Generate<Func<object, object>>());

        internal TestProbeHandlersMapper CreateTestProbeHandlersMapper() => 
            new TestProbeHandlersMapper();
    }
}