using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage.Streams;
using Universal.Torrent.Common;

namespace PopcornTime.Utilities
{
    /// <summary>
    ///     Wraps the actual file stream, to buffer when the peice hasn't been downloaded.
    ///     Also slides the picker (priority)
    /// </summary>
    public sealed class TorrentRandomAccessStream : IRandomAccessStream
    {
        private readonly IRandomAccessStream _internalStream;
        private readonly TorrentStreamManager _manager;
        private readonly Torrent _torrent;
        private readonly TorrentFile _torrentFile;

        public TorrentRandomAccessStream(TorrentStreamManager manager)
        {
            _manager = manager;
            _torrentFile = manager.TorrentVideoFile;
            _torrent = manager.Torrent;
            var stream = File.Open(Path.Combine(_torrentFile.TargetFolder.Path, _torrentFile.Path), FileMode.Open,
                FileAccess.Read, FileShare.ReadWrite);
            _internalStream = stream.AsRandomAccessStream();
        }

        public void Dispose() => _internalStream.Dispose();

        public IAsyncOperationWithProgress<IBuffer, uint> ReadAsync(IBuffer buffer, uint count,
            InputStreamOptions options)
        {
            return AsyncInfo.Run<IBuffer, uint>(async (token, progress) =>
            {
                var pieceSize = _torrent.PieceLength;
                var pieceStart = (int) ((double) _internalStream.Position/pieceSize);
                var pieceEnd = (int) ((count + (double) _internalStream.Position)/pieceSize);
                var total = pieceEnd - pieceStart;
                if (total == 0)
                    total++;
                var index = 0;

                for (var i = pieceStart; i <= pieceEnd; i++)
                {
                    progress.Report(((uint) index/(uint) total)*100);
                    index++;

                    // missing 
                    while (!_torrentFile.BitField[i])
                    {
                        token.ThrowIfCancellationRequested();

                        _manager.SlidingPicker.HighPrioritySetStart = i;
                        _manager.SlidingPicker.HighPrioritySetSize = Math.Max(TorrentStreamManager.DefaultPrepareCount,
                            i - pieceEnd);
                        // TODO instead of looping, use event
                        await Task.Delay(500, token);
                    }
                }

                var notDownloaded = _torrentFile.BitField.FirstFalse(pieceEnd, _torrentFile.BitField.Length - pieceEnd);
                if (notDownloaded > -1)
                {
                    _manager.SlidingPicker.HighPrioritySetStart = notDownloaded;
                    _manager.SlidingPicker.HighPrioritySetSize = TorrentStreamManager.DefaultPrepareCount;
                }
                return await _internalStream.ReadAsync(buffer, count, options);
            });
        }

        public IAsyncOperationWithProgress<uint, uint> WriteAsync(IBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public IAsyncOperation<bool> FlushAsync()
        {
            throw new NotImplementedException();
        }

        public IInputStream GetInputStreamAt(ulong position)
        {
            throw new NotImplementedException();
        }

        public IOutputStream GetOutputStreamAt(ulong position)
        {
            throw new NotImplementedException();
        }

        public void Seek(ulong position)
        {
            _internalStream.Seek(position);
        }

        public IRandomAccessStream CloneStream()
        {
            throw new NotImplementedException();
        }

        public bool CanRead => true;
        public bool CanWrite => false;

        public ulong Position =>
            _internalStream.Position;

        public ulong Size
        {
            get { return _internalStream.Size; }
            set { throw new NotImplementedException(); }
        }
    }
}