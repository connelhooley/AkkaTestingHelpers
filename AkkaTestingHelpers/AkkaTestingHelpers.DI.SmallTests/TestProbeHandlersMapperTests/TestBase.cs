using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ConnelHooley.AkkaTestingHelpers.DI.Helpers.Concrete;
using NUnit.Framework;

namespace ConnelHooley.AkkaTestingHelpers.DI.SmallTests.TestProbeHandlersMapperTests
{
    internal class TestBase
    {
        private List<Type> _types;

        [SetUp]
        public void SetUp()
        {
            _types = Assembly
                .GetAssembly(typeof(Type))
                .GetExportedTypes()
                .ToList();
        }

        [TearDown]
        public void TearDown()
        {
            _types = null;
        }

        protected (Type, Type, Func<object, object>) CreateSettingsHandler() =>
            (_types.RandomlyTakeItem(), _types.RandomlyTakeItem(), TestUtils.Create<Func<object, object>>());

        protected TestProbeHandlersMapper CreateTestProbeHandlersMapper() => 
            new TestProbeHandlersMapper();
    }
}