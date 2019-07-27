using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using AoLibs.Navigation.Core;
using AoLibs.Navigation.Core.Interfaces;
using AoTracker.Domain;
using AoTracker.Domain.Enums;
using AoTracker.Infrastructure.Models;
using AoTracker.Interfaces;

namespace AoTracker.Infrastructure.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ISettings _settings;
        private readonly INavigationManager<PageIndex> _outerNavigationManager;
        private ObservableCollection<HamburgerMenuEntry> _hamburgerItems;
        private HamburgerMenuEntry _selectedItem;

        private List<HamburgerMenuEntry> _allEntries = new List<HamburgerMenuEntry>
        {
            new HamburgerMenuEntry
            {
                Title = "Feed",
                Page = PageIndex.Feed,
            },
            new HamburgerMenuEntry
            {
                Title = "Sets",
                Page = PageIndex.CrawlerSets,
            },
        };

        public ObservableCollection<HamburgerMenuEntry> HamburgerItems
        {
            get => _hamburgerItems;
            set => Set(ref _hamburgerItems, value);
        }

        public HamburgerMenuEntry SelectedItem
        {
            get => _selectedItem;
            set => Set(ref _selectedItem, value, OnHamburgerSelectionChanged);
        }

        public MainViewModel(ISettings settings, INavigationManager<PageIndex> outerNavigationManager)
        {
            _settings = settings;
            _outerNavigationManager = outerNavigationManager;
            HamburgerItems = new ObservableCollection<HamburgerMenuEntry>(_allEntries);

        }

        public void Initialize()
        {
            if (!_settings.PassedWelcome)
                _outerNavigationManager.Navigate(PageIndex.Feed, NavigationBackstackOption.SetAsRootPage);
        }

        private void OnHamburgerSelectionChanged(HamburgerMenuEntry entry)
        {
            _outerNavigationManager.Navigate(entry.Page, NavigationBackstackOption.SetAsRootPage);
        }
    }
}
