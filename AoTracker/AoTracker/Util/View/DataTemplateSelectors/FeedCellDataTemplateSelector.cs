using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Crawlers.Enums;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Infrastructure.ViewModels.Item;
using Xamarin.Forms;

namespace AoTracker.Util.View.DataTemplateSelectors
{
    public class FeedCellDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate SurugayaDataTemplate { get; set; }
        public DataTemplate MandarakeDataTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var feedViewModel = (FeedItemViewModel) item;
            switch (feedViewModel.BackingModel.Domain)
            {
                case CrawlerDomain.Surugaya:
                    return SurugayaDataTemplate;
                case CrawlerDomain.Mandarake:
                    return MandarakeDataTemplate;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
