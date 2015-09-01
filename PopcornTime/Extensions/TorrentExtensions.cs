using System.Collections.Generic;
using Windows.Storage;
using Universal.Torrent.Client.Managers;
using Universal.Torrent.Client.Settings;
using Universal.Torrent.Common;

namespace PopcornTime.Extensions
{
    internal static class TorrentExtensions
    {
        public static TorrentManager CreateManager(this Torrent torrent, StorageFolder saveFolder)
        {
            return new TorrentManager(torrent, saveFolder, new TorrentSettings(4, 150, 0, 0)
            {
                UseDht = true,
                EnablePeerExchange = true
            });
        }
    }
}