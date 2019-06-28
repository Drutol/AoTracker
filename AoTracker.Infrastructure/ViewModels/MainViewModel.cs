using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Domain;
using AoTracker.Domain.Enums;
using AoTracker.Interfaces;
using Xamarin.Forms;

namespace AoTracker.Infrastructure.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ISettings _settings;
        private readonly IOuterNavigationManager _outerNavigationManager;
        private FlyoutBehavior _flyoutBehavior;

        public FlyoutBehavior FlyoutBehavior
        {
            get => _flyoutBehavior;
            set => Set(ref _flyoutBehavior, value);
        }

        public MainViewModel(ISettings settings, IOuterNavigationManager outerNavigationManager)
        {
            _settings = settings;
            _outerNavigationManager = outerNavigationManager;
        }

        public void Initialize()
        {
            if (!_settings.PassedWelcome)
                _outerNavigationManager.NavigateTo(OuterNavigationPage.Welcome);
        }
    }
}
