using PopcornTime.Web.Models;
using Universal.FluentRest.Extensions;
using Universal.FluentRest.Http;

namespace PopcornTime.Web
{
    /// <summary>
    ///     Returns the information about a specific movie.
    /// </summary>
    public class MovieDetailsRequest : RestRequestObject<YtsResponse<YtsMovieFull>>
    {
        public MovieDetailsRequest(uint movieId)
        {
            this.Url("https://yts.to/api/v2/movie_details.json").Query("movie_id", movieId);
        }

        /// <summary>
        ///     When set the data returned will include the added image URLs.
        /// </summary>
        /// <returns></returns>
        public MovieDetailsRequest WithImages()
        {
            return this.Query("with_images", "true");
        }

        /// <summary>
        ///     When set the data returned will include the added information about the cast.
        /// </summary>
        /// <returns></returns>
        public MovieDetailsRequest WithCast()
        {
            return this.Query("with_cast", "true");
        }
    }
}