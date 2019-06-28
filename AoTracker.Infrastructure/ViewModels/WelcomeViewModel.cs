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
        private readonly IOuterNavigationManager _outerNavigationManager;

        public WelcomeViewModel(IOuterNavigationManager outerNavigationManager)
        {
            _outerNavigationManager = outerNavigationManager;
        }

        public RelayCommand StartCommand => new RelayCommand(() =>
        {
            _outerNavigationManager.NavigateTo(OuterNavigationPage.Shell);
        });
    }
}
