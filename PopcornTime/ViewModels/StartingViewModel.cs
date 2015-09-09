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
using Universal.Torrent.Common;

namespace PopcornTime.ViewModels
{
    public class PlaybackTorrent
    {
        public string BackgroundImageUrl { get; set; }
        public string Title { get; set; }
        public string TorrentUrl { get; set; }
        public string MagnetLink { get; set; }
        public string TorrentHash { get; set; }
    }

    public class StartingViewModel : ViewModelBase
    {
        private readonly IDispatcherUtility _dispatcherUtility;
        private readonly INavigationService _navigationService;
        private readonly ITorrentStreamService _torrentStreamService;
        private string _downloadSpeed;
        private int _peers;
        private PlaybackTorrent _playbackTorrent;
        private double _prepareProgress;
        private TorrentStreamManager.State _state;

        public StartingViewModel(ITorrentStreamService torrentStreamService, INavigationService navigationService,
            IDispatcherUtility dispatcherUtility)
        {
            _torrentStreamService = torrentStreamService;
            _navigationService = navigationService;
            _dispatcherUtility = dispatcherUtility;
        }

        public TorrentStreamManager.State State
        {
            get { return _state; }
            set { Set(ref _state, value); }
        }

        public PlaybackTorrent PlaybackTorrent
        {
            get { return _playbackTorrent; }
            set { Set(ref _playbackTorrent, value); }
        }

        public string DownloadSpeed
        {
            get { return _downloadSpeed; }
            set { Set(ref _downloadSpeed, value); }
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

        public override bool KeepOnBackstack => false;

        public override string SimplifiedParameter(object parameter)
        {
            var playback = (PlaybackTorrent) parameter;
            return playback.Title;
        }

        public override async void OnNavigatedTo(object parameter, NavigationMode mode, Dictionary<string, object> state)
        {
            if (mode == NavigationMode.Back)
            {
                _navigationService.GoBack();
                return;
            }

            PlaybackTorrent = (PlaybackTorrent) parameter;
            if (!string.IsNullOrEmpty(PlaybackTorrent.TorrentUrl))
            {
                State = TorrentStreamManager.State.Metadata;
                var torrent = await Torrent.LoadAsync(new Uri(PlaybackTorrent.TorrentUrl), "");
                _torrentStreamService.CreateManager(torrent);
            }
            else if (!string.IsNullOrEmpty(PlaybackTorrent.MagnetLink))
            {
                var magnetLink = new MagnetLink(PlaybackTorrent.MagnetLink);
                _torrentStreamService.CreateManager(magnetLink);
            }
            else
            {
                var hash = InfoHash.FromHex(PlaybackTorrent.TorrentHash);
                _torrentStreamService.CreateManager(hash);
            }
            _torrentStreamService.StreamManager.StreamProgress += StreamManagerOnStreamProgress;
            _torrentStreamService.StreamManager.StreamReady += StreamManagerOnStreamReady;
            _torrentStreamService.StreamManager.Error += StreamManagerOnError;
            _torrentStreamService.StreamManager.StartDownload();
            State = _torrentStreamService.StreamManager.CurrentState;
        }

        private void StreamManagerOnError(object sender, EventArgs eventArgs)
        {
            _torrentStreamService.Stop();
            _dispatcherUtility.Run(() =>
            {
                CurtainPrompt.ShowError("Problem starting playback.");
                _navigationService.GoBack();
            });
        }

        public override void OnNavigatedFrom()
        {
            if (_torrentStreamService.StreamManager != null
                &&
                (_torrentStreamService.StreamManager.CurrentState == TorrentStreamManager.State.Preparing ||
                 _torrentStreamService.StreamManager.CurrentState == TorrentStreamManager.State.Starting))
            {
                // user aborted
                _torrentStreamService.Stop();
            }
        }

        private void StreamManagerOnStreamReady(object sender, EventArgs e)
        {
            _dispatcherUtility.Run(() =>
            {
                _torrentStreamService.StreamManager.StreamProgress -= StreamManagerOnStreamProgress;
                _torrentStreamService.StreamManager.StreamReady -= StreamManagerOnStreamReady;
                _torrentStreamService.StreamManager.Error -= StreamManagerOnError;
                _navigationService.Navigate(typeof (PlayerPage));
            });
        }

        private void StreamManagerOnStreamProgress(object sender, StreamProgressEventArgs e)
        {
            _dispatcherUtility.Run(() =>
            {
                State = _torrentStreamService.StreamManager.CurrentState;
                Peers = e.Seeds;
                PrepareProgress = e.PrepareProgress;
                DownloadSpeed = e.DownloadSpeed.Bytes().ToString("0.##") + "/s";
            });
        }
    }
}