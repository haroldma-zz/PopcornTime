using System;
using Newtonsoft.Json;

namespace PopcornTime.Web.Models
{
    public class YtsMoviePartial : YtsMovieBase
    {

        [JsonProperty("background_image")]
        public string BackgroundImage { get; set; }

        [JsonProperty("small_cover_image")]
        public string SmallCoverImage { get; set; }

        [JsonProperty("medium_cover_image")]
        public string MediumCoverImage { get; set; }

        [JsonProperty("date_uploaded")]
        public DateTime DateUploaded { get; set; }
    }
}