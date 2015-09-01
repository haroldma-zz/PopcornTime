using System.Collections.Generic;
using Newtonsoft.Json;

namespace PopcornTime.Web.Models
{
    public class YtsMovieListData
    {
        [JsonProperty("movie_count")]
        public int MovieCount { get; set; }

        public int Limit { get; set; }

        [JsonProperty("page_number")]
        public uint PageNumber { get; set; }

        public List<YtsMoviePartial> Movies { get; set; }
    }
}