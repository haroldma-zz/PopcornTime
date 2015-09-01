using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PopcornTime.Common;
using PopcornTime.Web;
using PopcornTime.Web.Models;

namespace PopcornTime.IncrementalLoading
{
    public class MoviesIncrementalLoadingCollection : IncrementalLoadingBase<YtsMovie>
    {
        private readonly ListMoviesRequest _request;
        private YtsResponse<YtsMovieListData> _currentResponse;
        private int _maxCount = -1;

        public MoviesIncrementalLoadingCollection(ListMoviesRequest request)
        {
            _request = request;
        }

        protected override async Task<IList<YtsMovie>> LoadMoreItemsOverrideAsync(CancellationToken c, uint count)
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
    }
}