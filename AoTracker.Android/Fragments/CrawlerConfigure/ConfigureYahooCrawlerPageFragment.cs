using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using AoLibs.Navigation.Android.Navigation.Attributes;
using AoLibs.Utilities.Android;
using AoTracker.Crawlers.Sites.Yahoo;
using AoTracker.Domain.Enums;
using AoTracker.Infrastructure.Models.NavArgs;
using AoTracker.Infrastructure.ViewModels.Crawlers;
using GalaSoft.MvvmLight.Helpers;

namespace AoTracker.Android.Fragments.CrawlerConfigure
{
    [NavigationPage(PageIndex.ConfigureYahoo, NavigationPageAttribute.PageProvider.Oneshot)]
    class ConfigureYahooCrawlerPageFragment : ConfigureCrawlerPageFragmentBase<ConfigureYahooCrawlerViewModel, YahooSourceParameters>
    {
        public override int LayoutResourceId { get; } = Resource.Layout.page_configure_mandarake_crawler;

        protected override void InitBindings()
        {
            base.InitBindings();
            DomainIcon.SetImageResource(Resource.Drawable.yahoo);
        }
    }
}