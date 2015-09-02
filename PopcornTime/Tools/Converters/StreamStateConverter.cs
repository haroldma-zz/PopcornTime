using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using PopcornTime.Utilities;

namespace PopcornTime.Tools.Converters
{
    public class StreamStateConverter : IValueConverter
    {
        public TorrentStreamManager.State State { get; set; }
        public bool Not { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var state = (TorrentStreamManager.State) value;
            var b = !Not && State == state || Not && State != state;
            return b ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}