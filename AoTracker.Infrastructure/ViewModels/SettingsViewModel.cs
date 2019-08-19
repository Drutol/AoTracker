using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Resources;

namespace AoTracker.Infrastructure.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        public SettingsViewModel()
        {
            Title = AppResources.PageTitle_Settings;
        }
    }
}
