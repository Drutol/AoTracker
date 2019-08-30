using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using AoLibs.Navigation.Core;
using AoLibs.Navigation.Core.Interfaces;
using AoTracker.Domain;
using AoTracker.Domain.Enums;
using AoTracker.Infrastructure.Models;
using AoTracker.Interfaces;
using AoTracker.Resources;
using GalaSoft.MvvmLight.Command;
using Microsoft.AppCenter.Crashes;
using Microsoft.Extensions.Logging;

namespace AoTracker.Infrastructure.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ILogger<MainViewModel> _logger;
        private readonly ISettings _settings;
        private readonly INavigationManager<PageIndex> _navigationManager;
        private readonly IUserDataProvider _userDataProvider;
        private ObservableCollection<HamburgerMenuEntryViewModel> _hamburgerItems;
        private HamburgerMenuEntryViewModel _selectedItem;

        public HamburgerMenuEntryViewModel SettingsButtonViewModel { get; } = new HamburgerMenuEntryViewModel
        {
            Title = AppResources.Hamburger_Item_Settings,
            Page = PageIndex.SettingsIndex
        };

        private List<HamburgerMenuEntryViewModel> _allEntries = new List<HamburgerMenuEntryViewModel>
        {
            new HamburgerMenuEntryViewModel
            {
                Title = AppResources.Hamburger_Item_Feed,
                Page = PageIndex.Feed,
            },
            new HamburgerMenuEntryViewModel
            {
                Title = AppResources.Hamburger_Item_CrawlerSets,
                Page = PageIndex.CrawlerSets,
            },
            new HamburgerMenuEntryViewModel
            {
                Title = AppResources.Hamburger_Item_WatchedItems,
                Page = PageIndex.WatchedItems,
            },
            new HamburgerMenuEntryViewModel
            {
                Title = AppResources.Hamburger_Item_IgnoredItems,
                Page = PageIndex.IgnoredItems,
            },
        };

        public ObservableCollection<HamburgerMenuEntryViewModel> HamburgerItems
        {
            get => _hamburgerItems;
            set => Set(ref _hamburgerItems, value);
        }

        public MainViewModel(
            ILogger<MainViewModel> logger,
            ISettings settings,
            INavigationManager<PageIndex> navigationManager,
            IUserDataProvider userDataProvider)
        {
            _logger = logger;
            _settings = settings;
            _navigationManager = navigationManager;
            _userDataProvider = userDataProvider;
            HamburgerItems = new ObservableCollection<HamburgerMenuEntryViewModel>(_allEntries);


            _navigationManager.Navigated += NavigationManagerOnNavigated;
            _navigationManager.WentBack += NavigationManagerOnNavigated;
        }

        private void NavigationManagerOnNavigated(object sender, PageIndex e)
        {
            SetSelectedItem(HamburgerItems
                .Concat(new[] {SettingsButtonViewModel})
                .FirstOrDefault(model => model.Page == e));
        }

        public async void Initialize()
        {
            _logger.LogDebug("App management passed to ViewModel layer.");
            Crashes.GenerateTestCrash();
            await _userDataProvider.Initialize();

            if (!_settings.PassedWelcome)
                _navigationManager.Navigate(PageIndex.CrawlerSets, NavigationBackstackOption.SetAsRootPage);
        }

        public RelayCommand<HamburgerMenuEntryViewModel> SelectHamburgerItemCommand =>
            new RelayCommand<HamburgerMenuEntryViewModel>(item =>
            {
                SetSelectedItem(item);
                _navigationManager.Navigate(item.Page, NavigationBackstackOption.SetAsRootPage);
            });

        private void SetSelectedItem(HamburgerMenuEntryViewModel item)
        {
            foreach (var hamburgerMenuEntry in HamburgerItems.Concat(new [] {SettingsButtonViewModel}))
            {
                hamburgerMenuEntry.IsSelected = false;
            }
            if(item != null)
                item.IsSelected = true;
        }
    }
}
