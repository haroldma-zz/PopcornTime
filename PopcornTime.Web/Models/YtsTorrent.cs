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

        public int Seeds { get; set; }
        public int Peers { get; set; }
        public string Size { get; set; }

        [JsonProperty("size_bytes")]
        public long SizeBytes { get; set; }

        [JsonProperty("date_uploaded")]
        public DateTime DateUploaded { get; set; }
    }
}