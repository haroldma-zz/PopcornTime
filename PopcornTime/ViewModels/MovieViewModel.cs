using System.Collections.Generic;
using Windows.UI.Xaml.Navigation;
using Audiotica.Windows.Services.NavigationService;
using PopcornTime.Common;
using PopcornTime.Tools.Mvvm;
using PopcornTime.Web;
using PopcornTime.Web.Models;

namespace PopcornTime.ViewModels
{
    public class MovieViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private bool _isLoading;
        private YtsMovieFull _movie;

        public MovieViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public YtsMovieFull Movie
        {
            get { return _movie; }
            set { Set(ref _movie, value); }
        }

        public bool IsLoading
        {
            get { return _isLoading; }
            set { Set(ref _isLoading, value); }
        }

        public override async void OnNavigatedTo(object parameter, NavigationMode mode, Dictionary<string, object> state)
        {
            if (state.ContainsKey("movie"))
                Movie = state["movie"] as YtsMovieFull;

            if (Movie == null)
            {
                IsLoading = true;
                var id = uint.Parse(parameter.ToString());
                var response = await new MovieDetailsRequest(id).WithCast().WithImages().ToResponseAsync();
                IsLoading = false;
                if (response.IsSuccessStatusCode)
                    Movie = response.DeserializedResponse.Data;
                else
                {
                    _navigationService.GoBack();
                    CurtainPrompt.ShowError(response.DeserializedResponse?.StatusMessage ??
                                            "Problem loading movie details.");
                }
            }
        }

        public override void OnSaveState(bool suspending, Dictionary<string, object> state)
        {
            state["movie"] = Movie;
        }
    }
}