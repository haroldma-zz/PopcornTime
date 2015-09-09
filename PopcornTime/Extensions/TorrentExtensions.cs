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

        public static TorrentManager CreateManager(this InfoHash hash, StorageFolder saveFolder)
        {
            return new TorrentManager(hash, saveFolder, new TorrentSettings(4, 150, 0, 0)
            {
                UseDht = true,
                EnablePeerExchange = true
            }, new List<RawTrackerTier>
            {
                new RawTrackerTier(new[]
                {
                    "udp://tracker.openbittorrent.com:80",
                    "udp://tracker.coppersurfer.tk:6969",
                    "udp://open.demonii.com:1337"
                })
            });
        }
    }
}