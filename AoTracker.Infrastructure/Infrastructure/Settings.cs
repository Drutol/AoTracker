using System;
using System.Collections.Generic;
using System.Text;
using AoLibs.Adapters.Core.Interfaces;
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
    }
}
