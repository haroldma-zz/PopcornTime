using System;
using System.Threading.Tasks;
using Windows.Storage;
using PopcornTime.Common;
using Universal.Torrent.Client;
using Universal.Torrent.Client.Args;
using Universal.Torrent.Client.Managers;
using Universal.Torrent.Client.PiecePicking;
using Universal.Torrent.Common;

namespace PopcornTime.Utilities
{
    public class TorrentStreamManager
    {
        private const int MaxPrepareCount = 20;
        private const int MinPrepareCount = 2;
        private const int DefaultPrepareCount = 5;
        private readonly long _prepareSize;
        private readonly TorrentManager _torrentManager;
        private int _firstPieceIndex;
        private int _lastPieceIndex;
        private int _pieceToPrepare;
        private double _prepareProgress;
        private double _progressStep;
        private long _selectedFileIndex;
        
        public TorrentStreamManager(TorrentManager torrentManager, long prepareSize = 10*1024L*1024L)
        {
            _torrentManager = torrentManager;
            _prepareSize = prepareSize;
            
            // Select the largest file
            SetSelectedFile(-1);
        }

        public State CurrentState { get; set; }

        public event EventHandler StreamReady;
        public event EventHandler<StreamProgressEventArgs> StreamProgress;

        public Task<StorageFile> GetVideoFileAsync()
        {
            var torrentFile = _torrentManager.Torrent.Files[_selectedFileIndex];
            return StorageHelper.GetFileAsync(torrentFile.Path, torrentFile.TargetFolder);
        }

        public void Pause() => _torrentManager.Pause();
        public void Resume() => _torrentManager.Start();

        public void SetSelectedFile(int selectedFileIndex)
        {
            var files = _torrentManager.Torrent.Files;

            if (selectedFileIndex == -1)
            {
                long highestFileSize = 0;
                var selectedFile = -1;
                for (var i = 0; i < files.Length; i++)
                {
                    var fileSize = files[i].Length;
                    if (highestFileSize < fileSize)
                    {
                        highestFileSize = fileSize;

                        if (selectedFile > -1)
                            files[selectedFile].Priority = Priority.DoNotDownload;
                        selectedFile = i;
                        files[i].Priority = Priority.Normal;
                    }
                    else
                    {
                        files[i].Priority = Priority.DoNotDownload;
                    }
                }
                selectedFileIndex = selectedFile;
            }
            else
            {
                for (var i = 0; i < files.Length; i++)
                {
                    files[i].Priority = i == selectedFileIndex ? Priority.Normal : Priority.DoNotDownload;
                }
            }
            _selectedFileIndex = selectedFileIndex;

            var firstPieceIndex = _torrentManager.Torrent.Files[_selectedFileIndex].StartPieceIndex;
            var lastPieceIndex = _torrentManager.Torrent.Files[_selectedFileIndex].EndPieceIndex;

            var pieceCount = lastPieceIndex - firstPieceIndex + 1;
            var pieceLength = _torrentManager.Torrent.PieceLength;
            int activePieceCount;
            if (pieceLength > 0)
            {
                activePieceCount = (int) (_prepareSize/pieceLength);
                if (activePieceCount < MinPrepareCount)
                {
                    activePieceCount = MinPrepareCount;
                }
                else if (activePieceCount > MaxPrepareCount)
                {
                    activePieceCount = MaxPrepareCount;
                }
            }
            else
            {
                activePieceCount = DefaultPrepareCount;
            }

            if (pieceCount < activePieceCount)
            {
                activePieceCount = pieceCount/2;
            }

            _firstPieceIndex = firstPieceIndex;
            _lastPieceIndex = lastPieceIndex;
            _pieceToPrepare = activePieceCount;
        }

        public void StartDownload()
        {
            if (CurrentState == State.Streaming) return;
            CurrentState = State.Preparing;

            var slidingPicker = new SlidingWindowPicker(new PriorityPicker(new StandardPicker()))
            {
                HighPrioritySetStart = _firstPieceIndex,
                HighPrioritySetSize = _pieceToPrepare + _firstPieceIndex
            };
            _torrentManager.ChangePicker(slidingPicker);
            _torrentManager.PieceManager.BlockReceived -= PieceManagerOnBlockReceived;
            _torrentManager.PieceManager.BlockReceived += PieceManagerOnBlockReceived;

            var blockCount = _pieceToPrepare*_torrentManager.Torrent.PieceLength/
                             (double) Piece.BlockSize;
            _progressStep = 100/blockCount;

            _torrentManager.Start();
        }

        private void PieceManagerOnBlockReceived(object sender, BlockEventArgs args)
        {
            if (CurrentState == State.Preparing)
                CurrentState = State.Starting;
            
            if (args.Piece.Index >= _firstPieceIndex && args.Piece.Index <= _pieceToPrepare + _firstPieceIndex)
                _prepareProgress += _progressStep;
            OnStreamProgress(_prepareProgress, _torrentManager.Progress, _torrentManager.Peers.Seeds,
                _torrentManager.Monitor.DownloadSpeed);

            if (CurrentState == State.Starting && _prepareProgress.CompareTo(100) == 0)
            {
                StreamReady?.Invoke(this, EventArgs.Empty);
                CurrentState = State.Streaming;
            }

            /* // Check if the piece finished downloading
            if (!args.Piece.AllBlocksReceived || _prepareProgress < 100) return;

            // update high priority
            var slidingPicker = new SlidingWindowPicker(new StandardPicker())
            {
                HighPrioritySetStart = args.Piece.Index,
                HighPrioritySetSize = _pieceToPrepare + args.Piece.Index
            };
            _torrentManager.ChangePicker(slidingPicker);*/
        }

        protected virtual void OnStreamProgress(double prepareProgress, double progress, int seeds, double downloadSpeed)
        {
            StreamProgress?.Invoke(this, new StreamProgressEventArgs(prepareProgress, progress, seeds, downloadSpeed));
        }

        public enum State
        {
            Unknown,
            Preparing,
            Starting,
            Streaming
        }
    }
}