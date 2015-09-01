using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PopcornTime.Web.Models
{
    public class YtsMovie
    {
        public int Id { get; set; }
        public string Url { get; set; }

        [JsonProperty("imdb_code")]
        public string ImdbCode { get; set; }

        public string Title { get; set; }

        [JsonProperty("title_long")]
        public string TitleLong { get; set; }

        public string Slug { get; set; }
        public int Year { get; set; }
        public double Rating { get; set; }
        public int Runtime { get; set; }
        public List<string> Genres { get; set; }
        public string Language { get; set; }

        [JsonProperty("mpa_rating")]
        public string MpaRating { get; set; }

        [JsonProperty("background_image")]
        public string BackgroundImage { get; set; }

        [JsonProperty("small_cover_image")]
        public string SmallCoverImage { get; set; }

        [JsonProperty("medium_cover_image")]
        public string MediumCoverImage { get; set; }

        public string State { get; set; }
        public List<YtsTorrent> Torrents { get; set; }

        [JsonProperty("date_uploaded")]
        public DateTime DateUploaded { get; set; }
    }
}