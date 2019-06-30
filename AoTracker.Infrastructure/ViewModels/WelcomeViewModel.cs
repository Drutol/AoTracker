using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Domain.Enums;
using AoTracker.Interfaces;
using GalaSoft.MvvmLight.Command;
using Xamarin.Forms;

namespace AoTracker.Infrastructure.ViewModels
{
    public class WelcomeViewModel : ViewModelBase
    {
        private readonly INavigationManager _navigationManager;

        public WelcomeViewModel(INavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
            Title = "Welcome";
        }

        public void NavigatedTo()
        {
            
        }

        public RelayCommand StartCommand => new RelayCommand(() =>
        {
            _navigationManager.NavigateRoot(PageIndex.Feed);
        });
    }
}
