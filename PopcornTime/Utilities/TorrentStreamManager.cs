using System;
using PopcornTime.Common;
using Universal.Torrent.Client;
using Universal.Torrent.Client.Args;
using Universal.Torrent.Client.Managers;
using Universal.Torrent.Client.PiecePicking;
using Universal.Torrent.Common;

namespace PopcornTime.Utilities
{
    public class TorrentStreamManager : IDisposable
    {
        public enum State
        {
            Unknown,
            Metadata,
            Preparing,
            Starting,
            Streaming,
            Error
        }

        private const int MaxPrepareCount = 20;
        private const int MinPrepareCount = 2;
        public const int DefaultPrepareCount = 5;
        private readonly long _prepareSize;
        private int _firstPieceIndex;
        private int _lastPieceIndex;
        private int _pieceToPrepare;
        private double _prepareProgress;
        private double _progressStep;
        private long _selectedFileIndex;

        public TorrentStreamManager(TorrentManager torrentManager, long prepareSize = 10*1024L*1024L)
        {
            TorrentManager = torrentManager;
            _prepareSize = prepareSize;

            TorrentManager.TorrentStateChanged += TorrentManager_TorrentStateChanged;
        }

        public TorrentManager TorrentManager { get; }
        public SlidingWindowPicker SlidingPicker { get; private set; }

        public State CurrentState { get; set; }

        public TorrentFile TorrentVideoFile => TorrentManager.Torrent.Files[_selectedFileIndex];
        public Torrent Torrent => TorrentManager.Torrent;

        public void Dispose()
        {
            TorrentManager.Stop();
            CurrentState = State.Unknown;
        }

        public event EventHandler Error;
        public event EventHandler StreamReady;
        public event EventHandler<StreamProgressEventArgs> StreamProgress;

        public void Pause() => TorrentManager.Pause();
        public void Resume() => TorrentManager.Start();

        public void SetSelectedFile(int selectedFileIndex)
        {
            var files = TorrentManager.Torrent.Files;

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

            var firstPieceIndex = TorrentManager.Torrent.Files[_selectedFileIndex].StartPieceIndex;
            var lastPieceIndex = TorrentManager.Torrent.Files[_selectedFileIndex].EndPieceIndex;

            var pieceCount = lastPieceIndex - firstPieceIndex + 1;
            var pieceLength = TorrentManager.Torrent.PieceLength;
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

        private void TorrentManager_TorrentStateChanged(object sender, TorrentStateChangedEventArgs e)
        {
            if (e.NewState == TorrentState.Error)
            {
                CurrentState = State.Error;
                Error?.Invoke(this, EventArgs.Empty);
                return;
            }

            if (e.OldState != TorrentState.Metadata) return;
            CurrentState = State.Preparing;
            OnStreamProgress(0, 0, 0, 0);

            SetSelectedFile(-1);

            SlidingPicker = new SlidingWindowPicker(new PriorityPicker(new StandardPicker()))
            {
                HighPrioritySetStart = _firstPieceIndex,
                HighPrioritySetSize = _pieceToPrepare
            };
            TorrentManager.ChangePicker(SlidingPicker);
            TorrentManager.PieceManager.BlockReceived -= PieceManagerOnBlockReceived;
            TorrentManager.PieceManager.BlockReceived += PieceManagerOnBlockReceived;

            var blockCount = _pieceToPrepare*TorrentManager.Torrent.PieceLength/
                             (double) Piece.BlockSize;
            _progressStep = 100/blockCount;
        }

        public void StartDownload()
        {
            if (CurrentState == State.Streaming) return;

            if (TorrentManager.Torrent != null)
                TorrentManager_TorrentStateChanged(null,
                    new TorrentStateChangedEventArgs(TorrentManager, TorrentState.Metadata, TorrentState.Hashing));
            else
                CurrentState = State.Metadata;
            TorrentManager.Start();
        }

        private void PieceManagerOnBlockReceived(object sender, BlockEventArgs args)
        {
            if (CurrentState == State.Preparing)
                CurrentState = State.Starting;

            if (args.Piece.Index >= _firstPieceIndex && args.Piece.Index <= _pieceToPrepare + _firstPieceIndex)
                _prepareProgress += _progressStep;


            OnStreamProgress(_prepareProgress, TorrentManager.Progress, TorrentManager.Peers.Seeds,
                TorrentManager.Monitor.DownloadSpeed);

            if (CurrentState == State.Starting && _prepareProgress.CompareTo(100) >= 0)
            {
                CurrentState = State.Streaming;
                StreamReady?.Invoke(this, EventArgs.Empty);
            }
        }

        protected virtual void OnStreamProgress(double prepareProgress, double progress, int seeds, double downloadSpeed)
        {
            StreamProgress?.Invoke(this, new StreamProgressEventArgs(prepareProgress, progress, seeds, downloadSpeed));
        }
    }
}