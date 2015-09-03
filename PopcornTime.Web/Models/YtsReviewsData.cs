using System.Collections.Generic;
using Newtonsoft.Json;

namespace PopcornTime.Web.Models
{
    public class YtsReviewsData
    {
        [JsonProperty("review_count")]
        public int ReviewCount { get; set; }

        public List<YtsReview> Reviews { get; set; }
    }
}