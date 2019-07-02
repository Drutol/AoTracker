using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using AoTracker.Crawlers.Enums;
using Xamarin.Forms;

namespace AoTracker.Util.View.Converters
{
    public class CrawlerDomainToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((CrawlerDomain)value)
            {
                case CrawlerDomain.Surugaya:
                    return ImageSource.FromFile("surugaya.png");
                case CrawlerDomain.Mandarake:
                    return ImageSource.FromFile("mandarake.png");
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
