using System;

namespace PopcornTime.Common
{
    public class StreamProgressEventArgs : EventArgs
    {
        public StreamProgressEventArgs(double prepareProgress, double progress, int seeds, double downloadSpeed)
        {
            PrepareProgress = prepareProgress;
            Progress = progress;
            Seeds = seeds;
            DownloadSpeed = downloadSpeed;
        }

        public double PrepareProgress { get; }
        public double Progress { get; }
        public int Seeds { get; }
        public double DownloadSpeed { get; set; }
    }
}