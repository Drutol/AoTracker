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
using AoTracker.Infrastructure.ViewModels.Feed;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AoTracker.UWP.Pages
{
    [NavigationPage(PageIndex.Feed)]
    public sealed partial class FeedPage : FeedPageBase
    {
        public FeedPage()
        {
            this.InitializeComponent();
        }
    }

    public class FeedPageBase : PageBase<FeedViewModel>
    {

    }
}
