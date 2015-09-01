using Newtonsoft.Json;
using PopcornTime.Web.Enums;

namespace PopcornTime.Web.Models
{
    public class YtsResponse<T>
    {
        public ResponseStatus Status { get; set; }

        [JsonProperty("status_message")]
        public string StatusMessage { get; set; }

        public T Data { get; set; }

        [JsonProperty("@meta")]
        public YtsMeta Meta { get; set; }
    }
}