using System;
using PopcornTime.Helpers;
using PopcornTime.Services.Interfaces;
using PopcornTime.Utilities;
using Universal.Torrent.Client;
using Universal.Torrent.Common;

namespace PopcornTime.Services.RunTime
{
    internal class TorrentService : ITorrentService
    {
        private readonly ClientEngine _engine;

        public TorrentService(ClientEngine engine)
        {
            _engine = engine;
        }
        
        public TorrentStreamManager CreateStreamManager(Torrent torrent)
        {
            throw new NotImplementedException();
        }
    }
}