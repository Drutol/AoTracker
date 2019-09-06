using System;
using System.Collections.Generic;
using System.Text;
using AoTracker.Domain.Enums;
using AoTracker.Resources;

namespace AoTracker.Infrastructure.ViewModels.Settings
{
    public class SettingsAboutViewModel : ViewModelBase
    {
        public override PageIndex PageIdentifier { get; } = PageIndex.SettingsAbout;

        public SettingsAboutViewModel()
        {
            PageTitle = AppResources.PageTitle_SettingsAbout;
        }
    }
}
