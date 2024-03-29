﻿using System;
using System.Collections.Generic;
using System.Text;
using AoLibs.Navigation.Core.Interfaces;
using AoTracker.Crawlers.Enums;
using AoTracker.Crawlers.Mandarake;
using AoTracker.Crawlers.Mandarake;
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
    public class ConfigureMandarakeCrawlerViewModel : ConfigureCrawlerViewModelBase<MandarakeSourceParameters>
    {
        public ConfigureMandarakeCrawlerViewModel(INavigationManager<PageIndex> navigationManager,
            ISnackbarProvider snackbarProvider,
            IPriceIncreasePresetsProvider priceIncreasePresetsProvider)
            : base(navigationManager, snackbarProvider, priceIncreasePresetsProvider)
        {

        }

        public override PageIndex PageIdentifier { get; } = PageIndex.ConfigureMandarake;
        protected override CrawlerDomain Domain { get; } = CrawlerDomain.Mandarake;

        protected override MandarakeSourceParameters FillInParameters(MandarakeSourceParameters parameters)
        {
            return parameters;
        }

        protected override void InitParameters(MandarakeSourceParameters parameters)
        {
          
        }
    }
}
