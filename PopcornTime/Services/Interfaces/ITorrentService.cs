using PopcornTime.Utilities;
using Universal.Torrent.Common;

namespace PopcornTime.Services.Interfaces
{
    internal interface ITorrentService
    {
        TorrentStreamManager CreateStreamManager(Torrent torrent);
    }
}