using System;
using System.Collections.Generic;
using System.Linq;
using Windows.System;
using Windows.UI.Xaml.Navigation;
using Audiotica.Windows.Services.NavigationService;
using PopcornTime.Common;
using PopcornTime.Tools.Mvvm;
using PopcornTime.Views;
using PopcornTime.Web;
using PopcornTime.Web.Enums;
using PopcornTime.Web.Models;

namespace PopcornTime.ViewModels
{
    public class MovieViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private bool _is1080;
        private bool _isLoading;
        private bool _isQualityToggleEnabled;
        private YtsMovieFull _movie;
        private YtsTorrent _selectedTorrent;

        public MovieViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            PlayCommand = new Command(PlayExecute);
            TrailerCommand = new Command(TrailerExecute);
            QualityToggledCommand = new Command(QualityToggledExecute);
        }

        public Command TrailerCommand { get; }

        public Command QualityToggledCommand { get; }

        public Command PlayCommand { get; }

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

        public YtsTorrent SelectedTorrent
        {
            get { return _selectedTorrent; }
            set { Set(ref _selectedTorrent, value); }
        }

        public bool Is1080
        {
            get { return _is1080; }
            set { Set(ref _is1080, value); }
        }

        public bool IsQualityToggleEnabled
        {
            get { return _isQualityToggleEnabled; }
            set { Set(ref _isQualityToggleEnabled, value); }
        }

        private async void TrailerExecute()
        {
            await Launcher.LaunchUriAsync(new Uri($"https://youtu.be/{Movie.YoutubeTrailerCode}"));
        }

        private void QualityToggledExecute()
        {
            // Is1080 will have previous state, event called before the binding updates
            SelectedTorrent = Movie.Torrents.FirstOrDefault(p => p.Quality == (!Is1080
                ? VideoQuality.Q1080
                : VideoQuality.Q720));
        }

        private void PlayExecute()
        {
            _navigationService.Navigate(typeof (StartingPage), new PlaybackTorrent
            {
                Title = Movie.TitleLong,
                //TorrentUrl = SelectedTorrent.Url,
                TorrentHash = SelectedTorrent.Hash,
                BackgroundImageUrl = Movie.Images.BackgroundImageOriginal
            });
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

            if (Movie != null)
            {
                var high = Movie.Torrents.FirstOrDefault(p => p.Quality == VideoQuality.Q1080);
                var low = Movie.Torrents.FirstOrDefault(p => p.Quality == VideoQuality.Q720);
                if (high != null)
                {
                    Is1080 = true;
                    IsQualityToggleEnabled = low != null;
                    SelectedTorrent = high;
                }
                else
                    SelectedTorrent = low;
            }
        }

        public override void OnSaveState(bool suspending, Dictionary<string, object> state)
        {
            state["movie"] = Movie;
        }
    }
}