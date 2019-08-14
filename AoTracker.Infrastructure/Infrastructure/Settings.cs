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

        public AppTheme AppTheme
        {
            get => (AppTheme) (_settingsProvider.GetInt(nameof(AppTheme)) ?? (int?) (AppTheme.Dark | AppTheme.Orange));
            set => _settingsProvider.SetInt(nameof(AppTheme), (int) value);
        }
    }
}
