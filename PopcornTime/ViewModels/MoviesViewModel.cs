using System.Collections.Generic;
using Windows.UI.Xaml.Navigation;
using PopcornTime.IncrementalLoading;
using PopcornTime.Tools.Mvvm;
using PopcornTime.Web;
using PopcornTime.Web.Enums;

namespace PopcornTime.ViewModels
{
    public class MoviesViewModel : ViewModelBase
    {
        private MoviesIncrementalLoadingCollection _movieCollection;

        public MoviesIncrementalLoadingCollection MovieCollection
        {
            get { return _movieCollection; }
            set { Set(ref _movieCollection, value); }
        }

        public override void OnNavigatedTo(object parameter, NavigationMode mode, Dictionary<string, object> state)
        {
            CreateMovieList(Sort.Seeds);
        }

        public void CreateMovieList(Sort sort)
        {
            var request = new ListMoviesRequest().Limit(100).SortBy(sort);
            MovieCollection = new MoviesIncrementalLoadingCollection(request);
        }
    }
}