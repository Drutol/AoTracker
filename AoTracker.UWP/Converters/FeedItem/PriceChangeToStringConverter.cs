using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace AoTracker.UWP.Converters.FeedItem
{
    public class PriceChangeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var price = (float) value;

            if (Math.Abs(price) < 0.001)
                return string.Empty;

            return $"({(price > 0 ? "+" : "")}{price}¥)";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
