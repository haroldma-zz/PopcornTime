using Newtonsoft.Json;

namespace PopcornTime.Web.Models
{
    public class Director
    {
        public string Name { get; set; }
        public string SmallImage { get; set; }
        [JsonProperty("medium_image")]
        public string MediumImage { get; set; }
        [JsonProperty("imdb_code")]
        public string ImdbCode { get; set; }
    }
}