using Newtonsoft.Json;

namespace PopcornTime.Web.Models
{
    public class YtsParentalGuide
    {
        public string Type { get; set; }
        [JsonProperty("parental_guide_text")]
        public string ParentalGuideText { get; set; }
    }
}