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
using AoTracker.Infrastructure.Models.NavArgs;
using AoTracker.Infrastructure.ViewModels;
using AoTracker.UWP.Utils;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AoTracker.UWP.Pages.Crawlers
{

    [NavigationPage(PageIndex.CrawlerSetDetails)]
    public sealed partial class CrawlerSetDetailsPage 
    {
        public CrawlerSetDetailsPage()
        {
            this.InitializeComponent();
        }

        public override void NavigatedTo()
        {
            base.NavigatedTo();
            ViewModel.NavigatedTo(NavigationArguments as CrawlerSetDetailsPageNavArgs);
        }
    }

    public class CrawlerSetDetailsPageBase : CustomPageBase<CrawlerSetDetailsViewModel>
    {

    }
}
