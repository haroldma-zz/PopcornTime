using Newtonsoft.Json;

namespace PopcornTime.Web.Models
{
    public class Actor
    {
        public string Name { get; set; }
        [JsonProperty("character_name")]
        public string CharacterName { get; set; }
        [JsonProperty("small_image")]
        public string SmallImage { get; set; }
        [JsonProperty("medium_image")]
        public string MediumImage { get; set; }
        [JsonProperty("imdb_code")]
        public string ImdbCode { get; set; }
    }
}