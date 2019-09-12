using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using AoTracker.Domain.Enums;

namespace AoTracker.UWP.Converters.FeedItem
{
    public class PriceToColourConverter : IValueConverter
    {
        public Brush IncreaseColour { get; set; }
        public Brush DecreaseColour { get; set; }

        private static SolidColorBrush _transparentBrush = new SolidColorBrush(Colors.Transparent);

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var priceChange = (PriceChange)value;

            if (priceChange == PriceChange.Stale)
                return _transparentBrush;

            if (priceChange == PriceChange.Increase)
                return IncreaseColour; //up

            return DecreaseColour; //down
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
