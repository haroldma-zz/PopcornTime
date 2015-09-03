using System.Collections.Generic;
using Newtonsoft.Json;

namespace PopcornTime.Web.Models
{
    public class YtsParentalGuidesData
    {
        [JsonProperty("parental_guide_count")]
        public int ParentalGuideCount { get; set; }
        [JsonProperty("parental_guides")]
        public List<YtsParentalGuide> ParentalGuides { get; set; }
    }
}