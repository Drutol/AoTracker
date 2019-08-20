using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Resources;

namespace AoTracker.Infrastructure.ViewModels.Settings
{
    public class SettingsAboutViewModel : ViewModelBase
    {
        public SettingsAboutViewModel()
        {
            PageTitle = AppResources.PageTitle_SettingsAbout;
        }
    }
}
