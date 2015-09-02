using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Navigation;
using Audiotica.Windows.Services.NavigationService;
using Humanizer;
using PopcornTime.Common;
using PopcornTime.Services.Interfaces;
using PopcornTime.Tools.Mvvm;
using PopcornTime.Utilities;
using PopcornTime.Utilities.Interfaces;
using PopcornTime.Views;
using PopcornTime.Web;
using PopcornTime.Web.Models;
using Universal.Torrent.Common;

namespace PopcornTime.ViewModels
{
    public class MovieViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly ITorrentStreamService _torrentStreamService;
        private readonly IDispatcherUtility _dispatcherUtility;
        private string _downloadSpeed;
        private bool _isLoading;
        private YtsMovieFull _movie;
        private int _peers;
        private double _prepareProgress;
        private TorrentStreamManager.State _state;

        public MovieViewModel(INavigationService navigationService, ITorrentStreamService torrentStreamService, IDispatcherUtility dispatcherUtility)
        {
            _navigationService = navigationService;
            _torrentStreamService = torrentStreamService;
            _dispatcherUtility = dispatcherUtility;

            PlayCommand = new Command(PlayExecute);
        }

        public Command PlayCommand { get; set; }

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

        public double PrepareProgress
        {
            get { return _prepareProgress; }
            set { Set(ref _prepareProgress, value); }
        }

        public int Peers
        {
            get { return _peers; }
            set { Set(ref _peers, value); }
        }

        public string DownloadSpeed
        {
            get { return _downloadSpeed; }
            set { Set(ref _downloadSpeed, value); }
        }

        public TorrentStreamManager.State State
        {
            get { return _state; }
            set { Set(ref _state, value); }
        }

        private async void PlayExecute()
        {
            var torrent = await Torrent.LoadAsync(new Uri(Movie.Torrents[0].Url), "");
            _torrentStreamService.CreateManager(torrent);
            _torrentStreamService.StreamManager.StreamProgress += StreamManagerOnStreamProgress;
            _torrentStreamService.StreamManager.StreamReady += StreamManagerOnStreamReady;
            _torrentStreamService.StreamManager.StartDownload();
            State = _torrentStreamService.StreamManager.CurrentState;
        }

        private void StreamManagerOnStreamReady(object sender, EventArgs eventArgs)
        {
            _dispatcherUtility.Run(() =>
            {
                _torrentStreamService.StreamManager.StreamProgress -= StreamManagerOnStreamProgress;
                _torrentStreamService.StreamManager.StreamReady -= StreamManagerOnStreamReady;
                _navigationService.Navigate(typeof (PlayerPage));
            });
        }

        private void StreamManagerOnStreamProgress(object sender, StreamProgressEventArgs streamProgressEventArgs)
        {
            _dispatcherUtility.Run(() =>
            {
                State = _torrentStreamService.StreamManager.CurrentState;
                Peers = streamProgressEventArgs.Seeds;
                PrepareProgress = streamProgressEventArgs.PrepareProgress;
                DownloadSpeed = streamProgressEventArgs.DownloadSpeed.Bytes().ToString("0.##") + "/s";
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
        }

        public override void OnSaveState(bool suspending, Dictionary<string, object> state)
        {
            state["movie"] = Movie;
        }

        public override void OnNavigatedFrom()
        {
            if (_torrentStreamService.StreamManager != null 
                && (_torrentStreamService.StreamManager.CurrentState == TorrentStreamManager.State.Preparing || _torrentStreamService.StreamManager.CurrentState == TorrentStreamManager.State.Starting))
            {
                // user aborted
                _torrentStreamService.Stop();
            }
        }
    }
}