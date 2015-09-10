using System;
using System.Collections.Generic;
using System.Threading;
using Windows.System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Audiotica.Windows.Services.NavigationService;
using PopcornTime.Helpers;
using PopcornTime.IncrementalLoading;
using PopcornTime.Tools.Mvvm;
using PopcornTime.Utilities.Interfaces;
using PopcornTime.Views;
using PopcornTime.Web;
using PopcornTime.Web.Enums;
using PopcornTime.Web.Models;

namespace PopcornTime.ViewModels
{
    public class MoviesViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly ISettingsUtility _settingsUtility;

        private bool _init;
        private MoviesIncrementalLoadingCollection _movieCollection;
        private string _searchTerm;

        private int _selectedGenre;
        private int _selectedSort;
        private double _verticalOffset;

        public string[] Genres =
        {
            "All",
            "Action",
            "Adventure",
            "Animation",
            "Biography",
            "Comedy",
            "Crime",
            "Documentary",
            "Drama",
            "Family",
            "Fantasy",
            "Film-Noir",
            "History",
            "Horror",
            "Music",
            "Musical",
            "Mystery",
            "Romance",
            "Sci-Fi",
            "Short",
            "Sport",
            "Thriller",
            "War",
            "Western"
        };

        public string[] Sorters =
        {
            "Popularity",
            "Trending",
            "Last added",
            "Year",
            "Title",
            "Rating"
        };

        public MoviesViewModel(INavigationService navigationService, ISettingsUtility settingsUtility)
        {
            _navigationService = navigationService;
            _settingsUtility = settingsUtility;
            MovieClickCommand = new Command<ItemClickEventArgs>(MovieClickExecute);
            SearchEnterCommand = new Command<string>(SearchEnterExecute);
            HelpCommand = new Command(HelpExecute);
        }

        private async void HelpExecute()
        {
            var mailto = new Uri("mailto:?to=help@zumicts.com&subject=Popcorn Time App");
            await Launcher.LaunchUriAsync(mailto);
        }

        public Command HelpCommand { get; }

        public Command<string> SearchEnterCommand { get; }

        public int SelectedGenre
        {
            get { return _selectedGenre; }
            set
            {
                if (Set(ref _selectedGenre, value) && _init)
                    CreateMovieList();
            }
        }

        public int SelectedSort
        {
            get { return _selectedSort; }
            set
            {
                if (Set(ref _selectedSort, value) && _init)
                    CreateMovieList();
            }
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

        public string SearchTerm
        {
            get { return _searchTerm; }
            set { Set(ref _searchTerm, value); }
        }

        private void SearchEnterExecute(string text)
        {
            CreateMovieList();
        }

        private void CreateMovieList()
        {
            Sort sort;
            var genre = Genres[SelectedGenre];

            switch (SelectedSort)
            {
                default:
                    sort = Sort.Seeds;
                    break;
                case 1:
                    sort = Sort.TrendingScore;
                    break;
                case 2:
                    sort = Sort.DateAdded;
                    break;
                case 3:
                    sort = Sort.Year;
                    break;
                case 4:
                    sort = Sort.Title;
                    break;
                case 5:
                    sort = Sort.Rating;
                    break;
            }

            CreateMovieList(sort, genre, SearchTerm);
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
                MovieCollection =
                    ((MoviesIncrementalLoadingCollection.Surrogate) state["MovieCollection"]).FromSurrogate();
                VerticalOffset = (double) state["VerticalOffset"];
                SelectedGenre = int.Parse(state["SelectedGenre"].ToString());
                SelectedSort = int.Parse(state["SelectedSort"].ToString());
                SearchTerm = state["SearchTerm"] as string;
            }

            if (MovieCollection == null)
                CreateMovieList();
            _init = true;
        }

        public override void OnSaveState(bool suspending, Dictionary<string, object> state)
        {
            state["MovieCollection"] = MovieCollection.ToSurrogate();
            state["VerticalOffset"] = VerticalOffset;
            state["SelectedGenre"] = SelectedGenre;
            state["SelectedSort"] = SelectedSort;
            state["SearchTerm"] = SearchTerm;
        }

        public void CreateMovieList(Sort sort, string genre, string search)
        {
            if (genre == "All")
                genre = null;
            var request = new ListMoviesRequest().Limit(100).Genre(genre).SortBy(sort).Query(search);
            if (sort == Sort.Title)
                request.OrderBy(Order.Asc);
            MovieCollection = new MoviesIncrementalLoadingCollection(request);
        }
    }
}