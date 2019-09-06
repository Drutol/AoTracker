using System;
using System.Collections.Generic;
using System.Text;
using AoLibs.Navigation.Core.Interfaces;
using AoTracker.Crawlers.Enums;
using AoTracker.Crawlers.Sites.Yahoo;
using AoTracker.Domain.Enums;
using AoTracker.Infrastructure.Infrastructure;
using AoTracker.Interfaces.Adapters;

namespace AoTracker.Infrastructure.ViewModels.Crawlers
{
    public class ConfigureYahooCrawlerViewModel : ConfigureCrawlerViewModelBase<YahooSourceParameters>
    {
        public ConfigureYahooCrawlerViewModel(INavigationManager<PageIndex> navigationManager,
            ISnackbarProvider snackbarProvider,
            IPriceIncreasePresetsProvider priceIncreasePresetsProvider)
            : base(navigationManager, snackbarProvider, priceIncreasePresetsProvider)
        {
        }

        public override PageIndex PageIdentifier { get; } = PageIndex.ConfigureYahoo;
        protected override CrawlerDomain Domain { get; } = CrawlerDomain.Yahoo;

        protected override YahooSourceParameters FillInParameters(YahooSourceParameters parameters)
        {
            return parameters;
        }

        protected override void InitParameters(YahooSourceParameters parameters)
        {
            
        }
    }
}
