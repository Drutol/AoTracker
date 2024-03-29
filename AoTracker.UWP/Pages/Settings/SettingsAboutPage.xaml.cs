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
    [NavigationPage(PageIndex.SettingsAbout)]
    public sealed partial class SettingsAboutPage
    {
        public SettingsAboutPage()
        {
            this.InitializeComponent();
        }

    }

    public class SettingsAboutPageBase : CustomPageBase<SettingsAboutViewModel>
    {

    }
}
