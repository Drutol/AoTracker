using System;
using System.Collections.Generic;
using System.Text;
using AoLibs.Navigation.Core;
using AoLibs.Navigation.Core.Interfaces;
using AoTracker.Domain.Enums;
using AoTracker.Interfaces;
using GalaSoft.MvvmLight.Command;
using Xamarin.Forms;

namespace AoTracker.Infrastructure.ViewModels
{
    public class WelcomeViewModel : ViewModelBase
    {
        private readonly INavigationManager<PageIndex> _navigationManager;

        public WelcomeViewModel(INavigationManager<PageIndex> navigationManager)
        {
            _navigationManager = navigationManager;
            Title = "Welcome";
        }

        public void NavigatedTo()
        {
            
        }

        public RelayCommand StartCommand => new RelayCommand(() =>
        {
            _navigationManager.Navigate(PageIndex.Feed, NavigationBackstackOption.SetAsRootPage);
        });
    }
}
