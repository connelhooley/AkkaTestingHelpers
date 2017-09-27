using Akka.Configuration;
using Newtonsoft.Json;

namespace ConnelHooley.AkkaTestingHelpers.DI.MediumTests
{
    public static class AkkaConfig
    {
        public static Config Config => ConfigurationFactory.FromObject(new RootJson
        {
            Akka =  new AkkaJson
            {
                SuppressJsonSerializerWarning = true,
                Test = new TestJson
                {
                    SingleExpectDefault = 100,
                    DefaultTimeout = 100,
#if DEBUG
                    Timefactor = 1,
#else
                    Timefactor = 40,
#endif
                }
            }
        });

        public class RootJson
        {
            [JsonProperty("akka")]
            public AkkaJson Akka { get; set; }
        }

        public class AkkaJson
        {
            [JsonProperty("suppress-json-serializer-warning")]
            public bool SuppressJsonSerializerWarning { get; set; }

            [JsonProperty("test")]
            public TestJson Test { get; set; }
        }

        public class TestJson
        {
            [JsonProperty("single-expect-default")]
            public int SingleExpectDefault { get; set; }

            [JsonProperty("timefactor")]
            public double Timefactor { get; set; }

            [JsonProperty("default-timeout")]
            public int DefaultTimeout { get; set; }
        }
    }
}