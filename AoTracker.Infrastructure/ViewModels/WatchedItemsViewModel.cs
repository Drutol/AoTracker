﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using AoLibs.Utilities.Shared;
using AoTracker.Crawlers.Enums;
using AoTracker.Crawlers.Sites.Yahoo;
using AoTracker.Infrastructure.Models;
using AoTracker.Infrastructure.Util;
using AoTracker.Infrastructure.ViewModels.Item;
using AoTracker.Interfaces;
using AoTracker.Resources;
using Autofac;

namespace AoTracker.Infrastructure.ViewModels
{
    public class WatchedItemsViewModel : ViewModelBase
    {
        private readonly IWatchedItemsManager _watchedItemsManager;
        private readonly ILifetimeScope _lifetimeScope;

        public SmartObservableCollection<WatchedItemViewModel> WatchedItems { get; } =
            new SmartObservableCollection<WatchedItemViewModel>();

        public WatchedItemsViewModel(
            IWatchedItemsManager watchedItemsManager,
            ILifetimeScope lifetimeScope)
        {
            _watchedItemsManager = watchedItemsManager;
            _lifetimeScope = lifetimeScope;

            PageTitle = AppResources.PageTitle_WatchedItems;
        }


        public void NavigatedTo()
        {
            WatchedItems.Clear();
            WatchedItems.AddRange(_watchedItemsManager.Entries.Select(entry =>
            {
                var vmType = entry.Domain == CrawlerDomain.Yahoo
                    ? typeof(WatchedItemViewModel<YahooItem>)
                    : typeof(WatchedItemViewModel);
                var vm = _lifetimeScope.TypedResolve<WatchedItemViewModel>(vmType, entry);
                vm.IsLoading = true;
                return vm;
            }));
            _watchedItemsManager.StartAggregatingWatchedItemsData();
        }
    }
}
