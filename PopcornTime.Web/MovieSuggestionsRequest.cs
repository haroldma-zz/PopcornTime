using PopcornTime.Web.Models;
using Universal.FluentRest.Extensions;
using Universal.FluentRest.Http;

namespace PopcornTime.Web
{
    /// <summary>
    ///     Returns 4 related movies as suggestions for the user.
    /// </summary>
    public class MovieSuggestionsRequest : RestRequestObject<YtsResponse<YtsMovieSuggestionData>>
    {
        public MovieSuggestionsRequest(uint movieId)
        {
            this.Url("https://yts.to/api/v2/movie_suggestions.json").Query("movie_id", movieId);
        }
    }
}