using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Interfaces;
using GalaSoft.MvvmLight.Command;

namespace AoTracker.Infrastructure.ViewModels.Item
{
    public class FeedItemViewModel : ItemViewModelBase<ICrawlerResultItem>, IFeedItem
    {
        private readonly FeedViewModel _parent;

        public FeedItemViewModel(ICrawlerResultItem item, FeedViewModel parent) : base(item)
        {
            _parent = parent;
        }

        public RelayCommand<ICrawlerResultItem> TapCommand => _parent.SelectFeedItemCommand;
    }
}
