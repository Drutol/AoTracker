using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using AoTracker.Crawlers.Enums;
using AoTracker.Infrastructure.Models;
using AoTracker.Infrastructure.ViewModels.Item;
using AoTracker.Interfaces;

namespace AoTracker.UWP.Selectors
{
    public class FeedItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate YahooItemDataTemplate { get; set; }
        public DataTemplate SharedItemDataTemplate { get; set; }
        public DataTemplate ChangeGroupHeaderDataTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            return SelectTemplateCore(item);
        }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            var merchItem = (IMerchItem)item;
            if (merchItem is FeedItemViewModel feedItemViewModel)
            {
                switch (feedItemViewModel.BackingModel.Domain)
                {
                    case CrawlerDomain.Surugaya:
                    case CrawlerDomain.Mandarake:
                    case CrawlerDomain.Mercari:
                    case CrawlerDomain.Lashinbang:
                        return SharedItemDataTemplate;
                    case CrawlerDomain.Yahoo:
                        return YahooItemDataTemplate;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else if (merchItem is FeedChangeGroupItem changeGroupItem)
            {
                return ChangeGroupHeaderDataTemplate;
            }

            throw new Exception();
        }
    }
}
