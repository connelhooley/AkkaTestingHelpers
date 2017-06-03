using System;
using System.Collections.Generic;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;
using NUnit.Framework;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.TestProbeHandlersMapperTests
{
    internal class TestBase
    {
        private Func<Type> _typeGenerator;

        [SetUp]
        public void SetUp()
        {
            _typeGenerator = TestUtils.RandomTypeGenerator();
        }

        [TearDown]
        public void TearDown()
        {
            _typeGenerator = null;
        }

        protected (Type, Type, Func<object, object>) CreateSettingsHandler()
        {
            return (_typeGenerator(), _typeGenerator(), TestUtils.Create<Func<object, object>>());
        }

        protected TestProbeHandlersMapper CreateTestProbeHandlersMapper() => 
            new TestProbeHandlersMapper();
    }
}