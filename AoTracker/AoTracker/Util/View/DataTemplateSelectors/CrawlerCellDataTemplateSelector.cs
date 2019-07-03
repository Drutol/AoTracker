using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Crawlers.Enums;
using AoTracker.Domain.Models;
using AoTracker.Infrastructure.ViewModels.Item;
using Xamarin.Forms;

namespace AoTracker.Util.View.DataTemplateSelectors
{
    public class CrawlerCellDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate SurugayaTemplate { get; set; }
        public DataTemplate MandarakeTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var descriptor = (CrawlerDescriptorViewModel) item;
            switch (descriptor.BackingModel.CrawlerDomain)
            {
                case CrawlerDomain.Surugaya:
                    return SurugayaTemplate;
                case CrawlerDomain.Mandarake:
                    return MandarakeTemplate;
            }

            throw new ArgumentOutOfRangeException(nameof(item));
        }
    }
}
