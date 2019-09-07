using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using AoTracker.Crawlers.Enums;
using AoTracker.Infrastructure.ViewModels.Item;

namespace AoTracker.UWP.Selectors
{
    public class CrawlerTemplateSelector : DataTemplateSelector
    {
        public DataTemplate SurugayaTemplateSelector { get; set; }
        public DataTemplate MandarakeTemplateSelector { get; set; }
        public DataTemplate MercariTemplateSelector { get; set; }
        public DataTemplate LashinbangTemplateSelector { get; set; }
        public DataTemplate YahooTemplateSelector { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            switch (((CrawlerDescriptorViewModel)item).BackingModel.CrawlerDomain)
            {
                case CrawlerDomain.Surugaya:
                    return SurugayaTemplateSelector;
                case CrawlerDomain.Mandarake:
                    return MandarakeTemplateSelector;
                case CrawlerDomain.Yahoo:
                    return YahooTemplateSelector;
                case CrawlerDomain.Mercari:
                    return MercariTemplateSelector;
                case CrawlerDomain.Lashinbang:
                    return LashinbangTemplateSelector;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
