﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using AoLibs.Navigation.UWP.Attributes;
using AoTracker.Domain.Enums;
using AoTracker.Infrastructure.ViewModels.Settings;
using AoTracker.UWP.Utils;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AoTracker.UWP.Pages.Settings
{
    [NavigationPage(PageIndex.SettingsGeneral)]
    public sealed partial class SettingsGeneralPage
    {
        public SettingsGeneralPage()
        {
            this.InitializeComponent();
        }

        public override void NavigatedTo()
        {
            base.NavigatedTo();
            ViewModel.NavigatedTo();
        }
    }

    public class SettingsGeneralPageBase : CustomPageBase<SettingsGeneralViewModel>
    {

    }
}
