using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using AoLibs.Adapters.Core.Interfaces;
using AoTracker.Domain.Enums;
using AoTracker.Infrastructure.Statics;
using AoTracker.Interfaces;
using AoTracker.Resources;
using GalaSoft.MvvmLight.Command;

namespace AoTracker.Infrastructure.ViewModels.Settings
{
    public class SettingsGeneralViewModel : ViewModelBase
    {
        private readonly IUriLauncherAdapter _uriLauncherAdapter;
        private readonly ISettings _settings;
        private readonly IFeedUpdateBackgroundServiceManager _updateBackgroundServiceManager;
        private AppTheme _appTheme;

        public override PageIndex PageIdentifier { get; } = PageIndex.SettingsGeneral;

        public SettingsGeneralViewModel(
            IUriLauncherAdapter uriLauncherAdapter,
            ISettings settings,
            IFeedUpdateBackgroundServiceManager updateBackgroundServiceManager)
        {
            _uriLauncherAdapter = uriLauncherAdapter;
            _settings = settings;
            _updateBackgroundServiceManager = updateBackgroundServiceManager;
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

        public bool AutoLoadFeedTab
        {
            get => _settings.AutoLoadFeedTab;
            set
            {
                _settings.AutoLoadFeedTab = value;
                RaisePropertyChanged();
            }
        }     
        
        public bool UsePriceIncreaseProxyPresets
        {
            get => _settings.UsePriceIncreaseProxyPresets;
            set
            {
                _settings.UsePriceIncreaseProxyPresets = value;
                RaisePropertyChanged();
            }
        }

        public bool GenerateFeedAggregate
        {
            get => _settings.GenerateFeedAggregate;
            set
            {
                _settings.GenerateFeedAggregate = value;
                RaisePropertyChanged();
            }
        }     
        
        public bool FeedUpdateJobScheduled
        {
            get => _settings.FeedUpdateJobScheduled;
            set
            {
                if (value)
                {
                    _updateBackgroundServiceManager.Schedule();
                }
                else
                {
                    _updateBackgroundServiceManager.Unschedule();
                }
            }
        }

        public ProxyDomain ProxyDomain
        {
            get => _settings.ProxyDomain;
            set
            {
                _settings.ProxyDomain = value;
                RaisePropertyChanged();
            }
        }

        public bool HasThemeChanged => _settings.AppTheme != AppTheme;

        public RelayCommand ApplyThemeCommand => new RelayCommand(() =>
        {
            _settings.AppTheme = AppTheme;
            ResourceLocator.BeginNewLifetimeScope();
        });

        public RelayCommand NavigateDontKillMyAppCommand => new RelayCommand(() =>
        {
            _uriLauncherAdapter.LaunchUri(new Uri("https://dontkillmyapp.com/"));
        });
    }
}
