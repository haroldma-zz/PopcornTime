using System;
using PopcornTime.Web.Enums;

namespace PopcornTime.Web.Helpers
{
    public static class VideoQualityHelper
    {
        public static VideoQuality Parse(string text)
        {
            text = text.Replace("p", "");
            if (!text.StartsWith("Q"))
                text = "Q" + text;

            VideoQuality quality;
            Enum.TryParse(text, out quality);
            return quality;
        }
    }
}