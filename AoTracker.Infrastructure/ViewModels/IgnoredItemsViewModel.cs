using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using AoLibs.Utilities.Shared;
using AoTracker.Domain.Models;
using AoTracker.Infrastructure.Models;
using AoTracker.Interfaces;
using AoTracker.Resources;
using GalaSoft.MvvmLight.Command;

namespace AoTracker.Infrastructure.ViewModels
{
    public class IgnoredItemsViewModel : ViewModelBase
    {
        private readonly IIgnoredItemsManager _ignoredItemsManager;
        private ObservableCollection<IgnoredItemEntry> _ignoredItems;


        public SmartObservableCollection<IgnoredItemEntry> IgnoredItems { get; } =
            new SmartObservableCollection<IgnoredItemEntry>();

        public IgnoredItemsViewModel(IIgnoredItemsManager ignoredItemsManager)
        {
            _ignoredItemsManager = ignoredItemsManager;

            PageTitle = AppResources.PageTitle_IgnoredItems;
        }

        public void NavigatedTo()
        {
            IgnoredItems.Clear();
            IgnoredItems.AddRange(_ignoredItemsManager.IgnoredEntries);
        }

        public RelayCommand<int> RemoveIgnoredItem => new RelayCommand<int>(position =>
        {
            var entry = IgnoredItems[position];
            IgnoredItems.Remove(entry);
            _ignoredItemsManager.RemoveIgnoredItem(entry);
        });
    }
}
