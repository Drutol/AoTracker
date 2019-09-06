using System;
using System.Collections.Generic;
using System.Text;
using AoLibs.Navigation.Core.Interfaces;
using AoTracker.Crawlers.Enums;
using AoTracker.Crawlers.Surugaya;
using AoTracker.Domain.Enums;
using AoTracker.Domain.Models;
using AoTracker.Infrastructure.Infrastructure;
using AoTracker.Infrastructure.Models.Messages;
using AoTracker.Infrastructure.Models.NavArgs;
using AoTracker.Infrastructure.ViewModels.Crawlers;
using AoTracker.Interfaces;
using AoTracker.Interfaces.Adapters;
using AoTracker.Resources;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace AoTracker.Infrastructure.ViewModels
{
    public class ConfigureSurugayaCrawlerViewModel : ConfigureCrawlerViewModelBase<SurugayaSourceParameters>
    {
        private bool _trimJapaneseQuotationMarks;

        public ConfigureSurugayaCrawlerViewModel(
            INavigationManager<PageIndex> navigationManager,
            ISnackbarProvider snackbarProvider,
            IPriceIncreasePresetsProvider priceIncreasePresetsProvider)
            : base(navigationManager, snackbarProvider, priceIncreasePresetsProvider)
        {
        }

        public bool TrimJapaneseQuotationMarks
        {
            get => _trimJapaneseQuotationMarks;
            set => Set(ref _trimJapaneseQuotationMarks, value);
        }

        public override PageIndex PageIdentifier { get; } = PageIndex.ConfigureSurugaya;
        protected override CrawlerDomain Domain { get; } = CrawlerDomain.Surugaya;

        protected override SurugayaSourceParameters FillInParameters(SurugayaSourceParameters parameters)
        {
            parameters.TrimJapaneseQuotationMarks = TrimJapaneseQuotationMarks;
            return parameters;
        }

        protected override void InitParameters(SurugayaSourceParameters parameters)
        {
            TrimJapaneseQuotationMarks = parameters.TrimJapaneseQuotationMarks;
        }
    }
}
