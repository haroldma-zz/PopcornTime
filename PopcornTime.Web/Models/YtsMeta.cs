using Newtonsoft.Json;

namespace PopcornTime.Web.Models
{
    public class YtsMeta
    {
        [JsonProperty("server_time")]
        public int ServerTime { get; set; }

        [JsonProperty("server_timezone")]
        public string ServerTimezone { get; set; }

        [JsonProperty("api_version")]
        public int ApiVersion { get; set; }

        [JsonProperty("execution_time")]
        public string ExecutionTime { get; set; }
    }
}