using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Audiotica.Windows.Services.NavigationService;
using PopcornTime.IncrementalLoading;
using PopcornTime.Tools.Mvvm;
using PopcornTime.Views;
using PopcornTime.Web;
using PopcornTime.Web.Enums;
using PopcornTime.Web.Models;

namespace PopcornTime.ViewModels
{
    public class MoviesViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private MoviesIncrementalLoadingCollection _movieCollection;
        private double _verticalOffset;

        public MoviesViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            MovieClickCommand = new Command<ItemClickEventArgs>(MovieClickExecute);
        }

        public Command<ItemClickEventArgs> MovieClickCommand { get; }

        public double VerticalOffset
        {
            get { return _verticalOffset; }
            set { Set(ref _verticalOffset, value); }
        }

        public MoviesIncrementalLoadingCollection MovieCollection
        {
            get { return _movieCollection; }
            set { Set(ref _movieCollection, value); }
        }

        private void MovieClickExecute(ItemClickEventArgs e)
        {
            var movie = (YtsMoviePartial) e.ClickedItem;
            _navigationService.Navigate(typeof (MoviePage), movie.Id);
        }

        public override void OnNavigatedTo(object parameter, NavigationMode mode, Dictionary<string, object> state)
        {
            if (state.ContainsKey("MovieCollection"))
            {
                MovieCollection = ((MoviesIncrementalLoadingCollection.Surrogate) state["MovieCollection"]).FromSurrogate();
                VerticalOffset = (double) state["VerticalOffset"];
            }

            if (MovieCollection == null)
                CreateMovieList(Sort.Seeds);
        }

        public override void OnSaveState(bool suspending, Dictionary<string, object> state)
        {
            state["MovieCollection"] = MovieCollection.ToSurrogate();
            state["VerticalOffset"] = VerticalOffset;
        }

        public void CreateMovieList(Sort sort)
        {
            var request = new ListMoviesRequest().Limit(100).SortBy(sort);
            MovieCollection = new MoviesIncrementalLoadingCollection(request);
        }
    }
}