using System;
using System.Collections.Generic;
using System.Text;
using AoLibs.Navigation.Core.Interfaces;
using AoTracker.Crawlers.Enums;
using AoTracker.Crawlers.Sites.Mercari;
using AoTracker.Domain.Enums;

namespace AoTracker.Infrastructure.ViewModels.Crawlers
{
    public class ConfigureMercariCrawlerViewModel : ConfigureCrawlerViewModelBase<MercariSourceParameters>
    {
        public ConfigureMercariCrawlerViewModel(INavigationManager<PageIndex> navigationManager) : base(navigationManager)
        {
        }

        protected override CrawlerDomain Domain { get; } = CrawlerDomain.Mercari;

        protected override MercariSourceParameters FillInParameters(MercariSourceParameters parameters)
        {
            return parameters;
        }

        protected override void InitParameters(MercariSourceParameters parameters)
        {
 
        }
    }
}
