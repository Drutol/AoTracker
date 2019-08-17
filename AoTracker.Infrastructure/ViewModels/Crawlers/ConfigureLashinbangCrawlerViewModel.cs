using System;
using System.Collections.Generic;
using System.Text;
using AoLibs.Navigation.Core.Interfaces;
using AoTracker.Crawlers.Enums;
using AoTracker.Crawlers.Sites.Lashinbang;
using AoTracker.Domain.Enums;

namespace AoTracker.Infrastructure.ViewModels.Crawlers
{
    public class ConfigureLashinbangCrawlerViewModel : ConfigureCrawlerViewModelBase<LashinbangSourceParameters>
    {
        public ConfigureLashinbangCrawlerViewModel(INavigationManager<PageIndex> navigationManager) : base(navigationManager)
        {
        }

        protected override CrawlerDomain Domain { get; } = CrawlerDomain.Lashinbang;

        protected override LashinbangSourceParameters FillInParameters(LashinbangSourceParameters parameters)
        {
            return parameters;
        }

        protected override void InitParameters(LashinbangSourceParameters parameters)
        {
           
        }
    }
}
