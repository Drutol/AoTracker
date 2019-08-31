using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using AoLibs.Navigation.Core;
using AoLibs.Navigation.Core.Interfaces;
using AoTracker.Crawlers.Enums;
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
        private readonly ITelemetryProvider _telemetryProvider;
        private readonly ISettings _settings;
        private readonly INavigationManager<PageIndex> _navigationManager;
        private readonly IUserDataProvider _userDataProvider;
        private ObservableCollection<HamburgerMenuEntryViewModel> _hamburgerItems;

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

        private bool _isDrawerEnabled;

        public ObservableCollection<HamburgerMenuEntryViewModel> HamburgerItems
        {
            get => _hamburgerItems;
            set => Set(ref _hamburgerItems, value);
        }

        public bool IsDrawerEnabled
        {
            get => _isDrawerEnabled;
            set => Set(ref _isDrawerEnabled, value);
        }

        public MainViewModel(
            ILogger<MainViewModel> logger,
            ITelemetryProvider telemetryProvider,
            ISettings settings,
            INavigationManager<PageIndex> navigationManager,
            IUserDataProvider userDataProvider)
        {
            _logger = logger;
            _telemetryProvider = telemetryProvider;
            _settings = settings;
            _navigationManager = navigationManager;
            _userDataProvider = userDataProvider;
            HamburgerItems = new ObservableCollection<HamburgerMenuEntryViewModel>(_allEntries);


            _navigationManager.Navigated += NavigationManagerOnNavigated;
            _navigationManager.WentBack += NavigationManagerOnNavigated;
        }

        private void NavigationManagerOnNavigated(object sender, PageIndex e)
        {
            _logger.LogInformation($"Navigated to: {e}");
            _telemetryProvider.TrackEvent(TelemetryEvent.Navigation, e.ToString());

            IsDrawerEnabled = e != PageIndex.Welcome;

                SetSelectedItem(HamburgerItems
                .Concat(new[] {SettingsButtonViewModel})
                .FirstOrDefault(model => model.Page == e));
        }

        public async void Initialize()
        {
            await _userDataProvider.Initialize();

            if (!_settings.PassedWelcome)
            {
                _navigationManager.Navigate(PageIndex.Welcome, NavigationBackstackOption.SetAsRootPage);
            }
            else
            {
                if (_userDataProvider.CrawlingSets.Any())
                    _navigationManager.Navigate(PageIndex.Feed, NavigationBackstackOption.SetAsRootPage);
                else
                    _navigationManager.Navigate(PageIndex.CrawlerSets, NavigationBackstackOption.SetAsRootPage);
            }
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
