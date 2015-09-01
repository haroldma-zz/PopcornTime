using System.Collections.Generic;
using Newtonsoft.Json;

namespace PopcornTime.Web.Models
{
    public class YtsMovieBase
    {
        public uint Id { get; set; }
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


        public List<YtsTorrent> Torrents { get; set; }
    }
}