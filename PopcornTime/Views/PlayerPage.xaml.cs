using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Audiotica.Windows.Services.NavigationService;
using PopcornTime.Services.Interfaces;
using PopcornTime.Utilities;
using Universal.Torrent.Common;
using NavigationEventArgs = Windows.UI.Xaml.Navigation.NavigationEventArgs;

namespace PopcornTime.Views
{
    public sealed partial class PlayerPage
    {
        public PlayerPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var streamService = App.Current.Kernel.Resolve<ITorrentStreamService>();

            if (streamService.StreamManager == null ||
                streamService.StreamManager.CurrentState != TorrentStreamManager.State.Streaming)
            {
                Dispose();
                return;
            }

            var torrentFile = streamService.StreamManager.TorrentVideoFile;
            var storageFile = await StorageHelper.GetFileAsync(torrentFile.Path, torrentFile.TargetFolder);
            MediaElement.SetSource(
                new TorrentRandomAccessStream(streamService.StreamManager), storageFile.ContentType);
        }

        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            Dispose();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            Dispose(false);
        }

        private void Dispose(bool navigateAway = true)
        {
            var streamService = App.Current.Kernel.Resolve<ITorrentStreamService>();
            if (streamService.StreamManager != null)
            {
                MediaElement.Stop();
                MediaElement.Source = null;
                streamService.Stop();
            }
            if (navigateAway)
            {
                var navService = App.Current.Kernel.Resolve<INavigationService>();
                navService.GoBack();
            }
        }
    }
}