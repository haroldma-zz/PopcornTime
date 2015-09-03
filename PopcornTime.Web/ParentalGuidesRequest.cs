using PopcornTime.Web.Models;
using Universal.FluentRest.Extensions;
using Universal.FluentRest.Http;

namespace PopcornTime.Web
{
    /// <summary>
    ///     Returns all the parental guide ratings for the specified movie.
    /// </summary>
    public class ParentalGuidesRequest : RestRequestObject<YtsResponse<YtsParentalGuidesData>>
    {
        public ParentalGuidesRequest(uint movieId)
        {
            this.Url("https://yts.to/api/v2/movie_parental_guides.json").Query("movie_id", movieId);
        }
    }
}