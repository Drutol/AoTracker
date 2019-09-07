using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoLibs.Navigation.Core.Interfaces;
using AoTracker.Crawlers.Enums;
using AoTracker.Crawlers.Surugaya;
using AoTracker.Domain.Enums;
using AoTracker.Infrastructure.Infrastructure;
using AoTracker.Infrastructure.ViewModels.Crawlers;
using AoTracker.Interfaces.Adapters;

namespace AoTracker.UWP.Utils.Designtime
{
    class DummyConfigureCrawlerViewModel : ConfigureCrawlerViewModelBase<SurugayaSourceParameters>
    {
        public DummyConfigureCrawlerViewModel(INavigationManager<PageIndex> navigationManager, ISnackbarProvider snackbarProvider, IPriceIncreasePresetsProvider priceIncreasePresetsProvider) : base(navigationManager, snackbarProvider, priceIncreasePresetsProvider)
        {
        }

        public override PageIndex PageIdentifier { get; }
        protected override CrawlerDomain Domain { get; }

        protected override SurugayaSourceParameters FillInParameters(SurugayaSourceParameters parameters)
        {
            throw new NotImplementedException();
        }

        protected override void InitParameters(SurugayaSourceParameters parameters)
        {
            throw new NotImplementedException();
        }
    }
}
