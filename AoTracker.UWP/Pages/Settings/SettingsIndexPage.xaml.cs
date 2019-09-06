using System;
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
using AoLibs.Navigation.UWP.Pages;
using AoTracker.Domain.Enums;
using AoTracker.Infrastructure.ViewModels.Settings;
using AoTracker.UWP.Utils;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AoTracker.UWP.Pages.Settings
{
    [NavigationPage(PageIndex.SettingsIndex)]
    public sealed partial class SettingsIndexPage
    {
        public SettingsIndexPage()
        {
            this.InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            App.NavigationManager.GoBack(PageIndex.SettingsIndex);
        }
    }

    public class SettingsIndexPageBase : CustomPageBase<SettingsIndexViewModel>
    {

    }
}
