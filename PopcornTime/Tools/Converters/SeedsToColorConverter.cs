using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using PopcornTime.Utilities;
using PopcornTime.Web.Enums;

namespace PopcornTime.Tools.Converters
{
    public class SeedsToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var health = value as TorrentHealth?;
            if (health == null) return null;
            Color color;

            switch (health)
            {
                case TorrentHealth.Bad:
                    color = ColorUtility.FromHex("#d53f3f");
                    break;
                case TorrentHealth.Medium:
                    color = ColorUtility.FromHex("#ece523");
                    break;
                case TorrentHealth.Good:
                    color = ColorUtility.FromHex("#a3e147");
                    break;
                case TorrentHealth.Excellent:
                    color = ColorUtility.FromHex("#2ad942");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return new SolidColorBrush(color);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}