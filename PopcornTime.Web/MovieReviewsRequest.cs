using PopcornTime.Web.Models;
using Universal.FluentRest.Extensions;
using Universal.FluentRest.Http;

namespace PopcornTime.Web
{
    /// <summary>
    /// Returns all the IMDb movie reviews for the specified movie.
    /// </summary>
    public class MovieReviewsRequest : RestRequestObject<YtsResponse<YtsReviewsData>>
    {
        public MovieReviewsRequest(uint movieId)
        {
            this.Url("https://yts.to/api/v2/movie_reviews.json").Query("movie_id", movieId);
        }
    }
}