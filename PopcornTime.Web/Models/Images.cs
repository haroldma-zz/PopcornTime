using Newtonsoft.Json;

namespace PopcornTime.Web.Models
{
    public class Images
    {
        [JsonProperty("background_image")]
        public string BackgroundImage { get; set; }

        [JsonIgnore]
        public string BackgroundImageOriginal => BackgroundImage.Replace("background.jpg", "background_original.jpg");

        [JsonProperty("small_cover_image")]
        public string SmallCoverImage { get; set; }

        [JsonProperty("medium_cover_image")]
        public string MediumCoverImage { get; set; }

        [JsonProperty("large_cover_image")]
        public string LargeCoverImage { get; set; }

        [JsonProperty("medium_screenshot_image1")]
        public string MediumScreenshotImage1 { get; set; }

        [JsonProperty("medium_screenshot_image2")]
        public string MediumScreenshotImage2 { get; set; }

        [JsonProperty("medium_screenshot_image3")]
        public string MediumScreenshotImage3 { get; set; }

        [JsonProperty("large_screenshot_image1")]
        public string LargeScreenshotImage1 { get; set; }

        [JsonProperty("large_screenshot_image2")]
        public string LargeScreenshotImage2 { get; set; }

        [JsonProperty("large_screenshot_image3")]
        public string LargeScreenshotImage3 { get; set; }
    }
}