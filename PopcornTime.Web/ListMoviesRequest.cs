using System;
using PopcornTime.Web.Enums;
using PopcornTime.Web.Extensions;
using PopcornTime.Web.Models;
using Universal.FluentRest.Extensions;
using Universal.FluentRest.Http;

namespace PopcornTime.Web
{
    /// <summary>
    ///     Used to list and search through out all the available movies. Can sort, filter, search and order the results.
    /// </summary>
    public class ListMoviesRequest : RestRequestObject<YtsResponse<YtsMovieListData>>
    {
        public ListMoviesRequest()
        {
            this.Url("https://yts.to/api/v2/list_movies.json");
        }

        /// <summary>
        ///     The limit of results per page that has been set. (Default: 20)
        /// </summary>
        /// <param name="limit">The limit.</param>
        /// <returns></returns>
        public ListMoviesRequest Limit(uint limit)
        {
            return this.Query("limit", limit);
        }

        /// <summary>
        ///     Used to see the next page of movies, eg limit=15 and page=2 will show you movies 15-30. (Default: 1)
        /// </summary>
        /// <param name="page">The page.</param>
        /// <returns></returns>
        public ListMoviesRequest Page(uint page)
        {
            return this.Query("page", page);
        }

        /// <summary>
        ///     Used to filter by a given quality.
        /// </summary>
        /// <param name="quality">The quality.</param>
        /// <returns></returns>
        public ListMoviesRequest Quality(VideoQuality quality)
        {
            var suffix = "p";
            if (quality == VideoQuality.Q3D)
                suffix = "";
            return this.Query("quality", quality.ToString().Replace("Q", "") + suffix);
        }

        /// <summary>
        ///     Used to filter by a given genre (See http://www.imdb.com/genre/ for full list).
        /// </summary>
        /// <param name="genre">The genere.</param>
        /// <returns></returns>
        public ListMoviesRequest Genre(string genre)
        {
            return this.Query("genre", genre);
        }

        /// <summary>
        ///     Used to filter movie by a given minimum IMDb rating.
        /// </summary>
        /// <param name="rating">The rating.</param>
        /// <returns></returns>
        public ListMoviesRequest MinimumRating(uint rating)
        {
            rating = Math.Min(rating, 9);
            return this.Query("minimum_rating", rating);
        }

        /// <summary>
        ///     Sorts the results by chosen value. (Default: DateAdded)
        /// </summary>
        /// <param name="sort">The sort.</param>
        /// <returns></returns>
        public ListMoviesRequest SortBy(Sort sort)
        {
            return this.Query("sort_by", sort.ToString().ToUnderscoreCase().ToLower());
        }

        /// <summary>
        ///     Orders the results by either Ascending or Descending order. (Default: Desc)
        /// </summary>
        /// <param name="order">The order.</param>
        /// <returns></returns>
        public ListMoviesRequest OrderBy(Order order)
        {
            return this.Query("order_by", order.ToString().ToLower());
        }

        /// <summary>
        ///     Used for movie search, matching on: Movie Title/IMDb Code, Actor Name/IMDb Code, Director Name/IMDb Code
        /// </summary>
        /// <param name="term">The term.</param>
        /// <returns></returns>
        public ListMoviesRequest Query(string term)
        {
            return this.Query("query_term", term);
        }

        /// <summary>
        ///     Returns the list with the Rotten Tomatoes rating included.
        /// </summary>
        /// <returns></returns>
        public ListMoviesRequest WithRottenTomatoesRatings()
        {
            return this.Query("with_rt_ratings", "true");
        }
    }
}