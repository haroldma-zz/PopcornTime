using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using PopcornTime.Services.Interfaces;
using Universal.Torrent.Common;
using System.IO;

namespace PopcornTime.Views
{
    public sealed partial class PlayerPage : Page
    {
        public PlayerPage()
        {
            InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            var streamService = App.Current.Kernel.Resolve<ITorrentStreamService>();
            var torrentFile = streamService.StreamManager.GetTorrentFileAsync();
            var storageFile = await StorageHelper.GetFileAsync(torrentFile.Path, torrentFile.TargetFolder);
            var stream = File.Open(storageFile.Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            MediaElement.SetSource(stream.AsRandomAccessStream(), storageFile.ContentType);
        }
    }
}