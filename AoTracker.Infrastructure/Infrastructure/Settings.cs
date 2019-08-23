using System;
using System.Collections.Generic;
using System.Text;
using AoLibs.Adapters.Core.Interfaces;
using AoTracker.Domain.Enums;
using AoTracker.Interfaces;

namespace AoTracker.Infrastructure.Infrastructure
{
    public class Settings : ISettings
    {
        private readonly ISettingsProvider _settingsProvider;

        public Settings(ISettingsProvider settingsProvider)
        {
            _settingsProvider = settingsProvider;
        }

        public bool PassedWelcome
        {
            get => _settingsProvider.GetBool(nameof(PassedWelcome)) ?? false;
            set => _settingsProvider.SetBool(nameof(PassedWelcome), value);
        }

        public bool GenerateFeedAggregate
        {
            get => _settingsProvider.GetBool(nameof(GenerateFeedAggregate)) ?? true;
            set => _settingsProvider.SetBool(nameof(GenerateFeedAggregate), value);
        }

        public bool AutoLoadFeedTab
        {
            get => _settingsProvider.GetBool(nameof(AutoLoadFeedTab)) ?? true;
            set => _settingsProvider.SetBool(nameof(AutoLoadFeedTab), value);
        }

        public AppTheme AppTheme
        {
            get => (AppTheme) (_settingsProvider.GetInt(nameof(AppTheme)) ?? (int?) (AppTheme.Dark | AppTheme.Lime));
            set => _settingsProvider.SetInt(nameof(AppTheme), (int) value);
        }

        public ProxyDomain ProxyDomain
        {
            get => (ProxyDomain) (_settingsProvider.GetInt(nameof(ProxyDomain)) ?? (int?) (ProxyDomain.None));
            set => _settingsProvider.SetInt(nameof(ProxyDomain), (int) value);
        }
    }
}
