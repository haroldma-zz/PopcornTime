using System.Collections.Generic;
using Newtonsoft.Json;

namespace PopcornTime.Web.Models
{
    public class YtsMovieFull : YtsMovieBase
    {
        [JsonProperty("download_count")]
        public int DownloadCount { get; set; }
        [JsonProperty("like_count")]
        public int LikeCount { get; set; }
        [JsonProperty("rt_critics_score")]
        public int RottenTomatoesCriticsScore { get; set; }
        [JsonProperty("rt_critics_rating")]
        public string RottenTomatoesCriticsRating { get; set; }
        [JsonProperty("rt_audience_score")]
        public int RottenTomatoesAudienceScore { get; set; }
        [JsonProperty("rt_audience_rating")]
        public string RottenTomatoesAudienceRating { get; set; }
        [JsonProperty("description_intro")]
        public string DescriptionIntro { get; set; }
        [JsonProperty("description_full")]
        public string DescriptionFull { get; set; }
        [JsonProperty("yt_trailer_code")]
        public string YoutubeTrailerCode { get; set; }
        public List<Director> Directors { get; set; }
        public List<Actor> Actors { get; set; }
        public Images Images { get; set; }
    }
}