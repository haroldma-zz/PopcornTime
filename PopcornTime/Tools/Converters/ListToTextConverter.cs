using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Data;

namespace PopcornTime.Tools.Converters
{
    public class ListToTextConverter : IValueConverter
    {
        public string Seperator { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var list = value as IEnumerable<object>;
            return string.Join(Seperator, list.Select(p => p.ToString()));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}