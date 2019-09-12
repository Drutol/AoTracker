using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using AoTracker.Domain.Enums;

namespace AoTracker.UWP.Converters.FeedItem
{
    public class PriceToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var priceChange = (PriceChange) value;

            if (priceChange == PriceChange.Stale)
                return "";

            if (priceChange == PriceChange.Increase)
                return "\uFD98"; //up

            return "\uFD95"; //down
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
