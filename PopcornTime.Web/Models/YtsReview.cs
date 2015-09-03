using System;
using Newtonsoft.Json;

namespace PopcornTime.Web.Models
{
    public class YtsReview
    {
        public string Username { get; set; }

        [JsonProperty("user_rating")]
        public int UserRating { get; set; }

        [JsonProperty("user_location")]
        public string UserLocation { get; set; }

        [JsonProperty("review_summary")]
        public string ReviewSummary { get; set; }

        [JsonProperty("review_text")]
        public string ReviewText { get; set; }

        [JsonProperty("date_written")]
        public DateTime DateWritten { get; set; }
    }
}