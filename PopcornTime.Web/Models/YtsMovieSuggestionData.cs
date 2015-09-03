using System.Collections.Generic;
using Newtonsoft.Json;

namespace PopcornTime.Web.Models
{
    public class YtsMovieSuggestionData
    {
        [JsonProperty("movie_suggestions_count")]
        public int MovieSuggestionsCount { get; set; }

        [JsonProperty("movie_suggestions")]
        public List<YtsMoviePartial> MovieSuggestions { get; set; }
    }
}