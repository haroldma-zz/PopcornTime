using PopcornTime.Utilities;
using Universal.Torrent.Common;

namespace PopcornTime.Services.Interfaces
{
    public interface ITorrentStreamService
    {
        TorrentStreamManager StreamManager { get; }
        void CreateManager(Torrent torrent);
        void Stop();
    }
}