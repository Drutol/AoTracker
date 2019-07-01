using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Infrastructure.Models;
using GalaSoft.MvvmLight.Command;

namespace AoTracker.Infrastructure.ViewModels.Item
{
    public class CrawlerEntryViewModel : ItemViewModelBase<CrawlerEntry>
    {
        private readonly CrawlerSetDetailsViewModel _parent;

        public CrawlerEntryViewModel(CrawlerEntry item, CrawlerSetDetailsViewModel parent) : base(item)
        {
            _parent = parent;
        }

        public RelayCommand<CrawlerEntry> SelectCommand => _parent.AddCrawlerCommand;
    }
}
