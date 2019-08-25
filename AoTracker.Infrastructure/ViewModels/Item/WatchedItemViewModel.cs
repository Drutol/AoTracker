using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using AoLibs.Adapters.Core.Interfaces;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Infrastructure.Models;
using AoTracker.Infrastructure.ViewModels.Feed;
using AoTracker.Interfaces;
using GalaSoft.MvvmLight.Command;

namespace AoTracker.Infrastructure.ViewModels.Item
{
    public class WatchedItemViewModel : ItemViewModelBase<WatchedItemDataEntry>, IMerchItem
    {
        private readonly IUriLauncherAdapter _uriLauncherAdapter;
        private readonly IDomainLinkHandlerManager _domainLinkHandlerManager;
        private readonly IWatchedItemsManager _watchedItemsManager;
        private bool _isLoading;

        public WatchedItemViewModel(WatchedItemDataEntry item,
            IUriLauncherAdapter uriLauncherAdapter,
            IDomainLinkHandlerManager domainLinkHandlerManager,
            IWatchedItemsManager watchedItemsManager) : base(item)
        {
            _uriLauncherAdapter = uriLauncherAdapter;
            _domainLinkHandlerManager = domainLinkHandlerManager;
            _watchedItemsManager = watchedItemsManager;

            watchedItemsManager.ItemDetailsFetched += WatchedItemsManagerOnItemDetailsFetched;
        }

        private void WatchedItemsManagerOnItemDetailsFetched(object sender, WatchedItemDataEntry e)
        {
            if (e == BackingModel)
            {
                Price = e.Data.Price;
            }
        }

        public ICrawlerResultItem Item => BackingModel.Data ?? BackingModel.DataProxy;

        public bool IsLoading
        {
            get => _isLoading;
            set => Set(ref _isLoading, value);
        }

        public float Price
        {
            get => (BackingModel.Data ?? BackingModel.DataProxy).Price;
            set
            {
                BackingModel.Data.Price = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand NavigateItemWebsiteCommand => new RelayCommand(() =>
        {
            _uriLauncherAdapter.LaunchUri(new Uri(_domainLinkHandlerManager.GenerateWebsiteLink(Item)));
        });
    }

    public class WatchedItemViewModel<T> : WatchedItemViewModel where T : ICrawlerResultItem
    {
        public WatchedItemViewModel(WatchedItemDataEntry item, IUriLauncherAdapter uriLauncherAdapter,
            IDomainLinkHandlerManager domainLinkHandlerManager, IWatchedItemsManager watchedItemsManager) : base(item,
            uriLauncherAdapter, domainLinkHandlerManager, watchedItemsManager)
        {
        }
    }
}
