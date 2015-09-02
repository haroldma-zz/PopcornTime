using System;
using Newtonsoft.Json;
using PopcornTime.Web.Enums;
using PopcornTime.Web.Helpers;

namespace PopcornTime.Web.Models
{
    public class YtsTorrent
    {
        private VideoQuality? _quality;
        public string Url { get; set; }
        public string Hash { get; set; }

        [JsonProperty("quality")]
        public string QualityText { get; set; }

        [JsonIgnore]
        public VideoQuality Quality => _quality ?? (_quality = VideoQualityHelper.Parse(QualityText)).Value;

        public uint Seeds { get; set; }
        public uint Peers { get; set; }
        public string Size { get; set; }

        [JsonProperty("size_bytes")]
        public long SizeBytes { get; set; }

        [JsonProperty("date_uploaded")]
        public DateTime DateUploaded { get; set; }

        public TorrentHealth Health => CalculateHealth();
        public string HealthText => $"Health {Health} - Ratio: {(double)Seeds/ Peers:F} - Seeds: {Seeds} - Peers: {Peers}";
        public TorrentHealth CalculateHealth()
        {
            var seeds = Seeds;
            var peers = Peers;

            // First calculate the seed/peer ratio
            var ratio = peers > 0 ? (seeds/peers) : seeds;

            // Normalize the data. Convert each to a percentage
            // Ratio: Anything above a ratio of 5 is good
            var normalizedRatio = Math.Min(ratio/5*100, 100);
            // Seeds: Anything above 30 seeds is good
            var normalizedSeeds = Math.Min(seeds/30*100, 100);

            // Weight the above metrics differently
            // Ratio is weighted 60% whilst seeders is 40%
            var weightedRatio = normalizedRatio*0.6;
            var weightedSeeds = normalizedSeeds*0.4;
            var weightedTotal = weightedRatio + weightedSeeds;

            // Scale from [0, 100] to [0, 3]. Drops the decimal places
            var scaledTotal = (byte) ((weightedTotal*3)/100) | 0;

            return (TorrentHealth) scaledTotal;
        }
    }
}