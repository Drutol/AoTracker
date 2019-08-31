using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using AoLibs.Navigation.Core;
using AoLibs.Navigation.Core.Interfaces;
using AoTracker.Domain.Enums;
using AoTracker.Domain.Models;
using AoTracker.Interfaces;
using AoTracker.Resources;
using GalaSoft.MvvmLight.Command;

namespace AoTracker.Infrastructure.ViewModels
{
    public class WelcomeViewModel : ViewModelBase
    {
        private readonly INavigationManager<PageIndex> _navigationManager;
        private readonly ISettings _settings;

        public WelcomeViewModel(
            INavigationManager<PageIndex> navigationManager,
            ISettings settings)
        {
            _navigationManager = navigationManager;
            _settings = settings;

            PageTitle = AppResources.PageTitle_Welcome;
        }

        public List<WelcomeTabEntry> WelcomeTabEntries { get; } = new List<WelcomeTabEntry>
        {
            new WelcomeTabEntry
            {
                WelcomeStage = WelcomeStage.Hello,
                Title = "Mock",
                Subtitle = "Test"
            },
            new WelcomeTabEntry
            {
                WelcomeStage = WelcomeStage.Hello,
                Title = "Mock2",
                Subtitle = "Test2"
            },
            new WelcomeTabEntry
            {
                WelcomeStage = WelcomeStage.Hello,
                Title = "Mock3",
                Subtitle = "Test3"
            }
        };

        public void NavigatedTo()
        {


        }

        public RelayCommand FinishCommand => new RelayCommand(() =>
        {
            _navigationManager.Navigate(PageIndex.CrawlerSets, NavigationBackstackOption.SetAsRootPage);
            _settings.PassedWelcome = true;
        });
    }
}
