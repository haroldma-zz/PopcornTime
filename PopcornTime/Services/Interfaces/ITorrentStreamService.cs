using PopcornTime.Utilities;
using Universal.Torrent.Common;

namespace PopcornTime.Services.Interfaces
{
    public interface ITorrentStreamService
    {
        TorrentStreamManager StreamManager { get; }
        void CreateManager(Torrent torrent);
        void CreateManager(InfoHash hash);
        void CreateManager(MagnetLink link);
        void Stop();
    }
}