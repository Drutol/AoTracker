using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Domain.Enums;
using AoTracker.Infrastructure.Statics;
using AoTracker.Interfaces;
using AoTracker.Resources;
using GalaSoft.MvvmLight.Command;

namespace AoTracker.Infrastructure.ViewModels.Settings
{
    public class SettingsGeneralViewModel : ViewModelBase
    {
        private readonly ISettings _settings;
        private AppTheme _appTheme;

        public SettingsGeneralViewModel(ISettings settings)
        {
            _settings = settings;
            PageTitle = AppResources.PageTitle_SettingsGeneral;
        }

        public void NavigatedTo()
        {
            AppTheme = _settings.AppTheme;
        }

        public AppTheme AppTheme
        {
            get => _appTheme;
            set
            {
                Set(ref _appTheme, value);
                RaisePropertyChanged(() => HasThemeChanged);
            }
        }

        public bool HasThemeChanged => _settings.AppTheme != AppTheme;

        public RelayCommand ApplyThemeCommand => new RelayCommand(() =>
        {
            _settings.AppTheme = AppTheme;
            ResourceLocator.BeginNewLifetimeScope();
        });
    }
}
