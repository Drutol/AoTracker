using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Resources;

namespace AoTracker.Infrastructure.ViewModels.Settings
{
    public class SettingsGeneralViewModel : ViewModelBase
    {
        public SettingsGeneralViewModel()
        {
            PageTitle = AppResources.PageTitle_SettingsGeneral;
        }
    }
}
