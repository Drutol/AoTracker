using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using AoTracker.Crawlers.Enums;

namespace AoTracker.UWP.Converters
{
    public class CrawlerDomainToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var domain = (CrawlerDomain) value;

            switch (domain)
            {
                case CrawlerDomain.Surugaya:
                    return "/Assets/Sites/surugaya.png";
                case CrawlerDomain.Mandarake:
                    return "/Assets/Sites/mandarake.png";
                case CrawlerDomain.Yahoo:
                    return "/Assets/Sites/yahoo.png";
                case CrawlerDomain.Mercari:
                    return "/Assets/Sites/mercari.png";
                case CrawlerDomain.Lashinbang:
                    return "/Assets/Sites/lashinbang.png";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
