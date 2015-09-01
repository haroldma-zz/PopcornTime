using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PopcornTime.Common;
using PopcornTime.Web;
using PopcornTime.Web.Models;

namespace PopcornTime.IncrementalLoading
{
    public class MoviesIncrementalLoadingCollection : IncrementalLoadingBase<YtsMoviePartial>
    {
        private readonly ListMoviesRequest _request;
        private YtsResponse<YtsMovieListData> _currentResponse;
        private int _maxCount = -1;

        internal MoviesIncrementalLoadingCollection(Surrogate surrogate)
        {
            foreach (var moviePartial in surrogate.Items)
                Add(moviePartial);
            _request = surrogate.Request;
            _currentResponse = surrogate.CurrentResponse;
            _maxCount = surrogate.CurrentResponse?.Data?.MovieCount ?? -1;
        }

        public MoviesIncrementalLoadingCollection(ListMoviesRequest request)
        {
            _request = request;
        }

        protected override async Task<IList<YtsMoviePartial>> LoadMoreItemsOverrideAsync(CancellationToken c, uint count)
        {
            var page = _currentResponse?.Data.PageNumber + 1 ?? 1;
            var response = await _request.Page(page).ToResponseAsync();
            if (!response.IsSuccessStatusCode) return null;

            _currentResponse = response.DeserializedResponse;
            _maxCount = _currentResponse.Data.MovieCount;

            return _currentResponse.Data.Movies;
        }

        protected override bool HasMoreItemsOverride()
        {
            // first request
            if (_maxCount == -1)
                return true;
            return Count < _maxCount;
        }

        public Surrogate ToSurrogate()
        {
            return new Surrogate(this);
        }


        // and contains the same data as MoviesIncrementalLoadingCollection
        /// <summary>
        ///     Intermediate class that can be serialized by JSON.net
        /// </summary>
        public class Surrogate
        {
            public Surrogate()
            {
            }

            internal Surrogate(MoviesIncrementalLoadingCollection collection)
            {
                Items = collection.ToList();
                CurrentResponse = collection._currentResponse;
                Request = collection._request;
            }

            public List<YtsMoviePartial> Items { get; set; }
            public YtsResponse<YtsMovieListData> CurrentResponse { get; set; }

            public ListMoviesRequest Request { get; set; }

            public MoviesIncrementalLoadingCollection FromSurrogate()
            {
                return new MoviesIncrementalLoadingCollection(this);
            }
        }
    }
}