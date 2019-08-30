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
using AoTracker.Crawlers.Surugaya;
using AoTracker.Domain.Enums;
using AoTracker.Infrastructure.Models.NavArgs;
using AoTracker.Infrastructure.ViewModels;
using GalaSoft.MvvmLight.Helpers;

namespace AoTracker.Android.Fragments.CrawlerConfigure
{
    [NavigationPage(PageIndex.ConfigureSurugaya, NavigationPageAttribute.PageProvider.Oneshot)]
    class ConfigureSurugayaCrawlerPageFragment : ConfigureCrawlerPageFragmentBase<ConfigureSurugayaCrawlerViewModel, SurugayaSourceParameters>
    {
        public override int LayoutResourceId { get; } = Resource.Layout.page_configure_surugaya_crawler;

        protected override void InitBindings()
        {
            base.InitBindings();
            DomainIcon.SetImageResource(Resource.Drawable.surugaya);
            Bindings.Add(
                this.SetBinding(() => ViewModel.TrimJapaneseQuotationMarks,
                    () => RemoveFromQuotationMarksCheckbox.Checked, BindingMode.TwoWay));
        }

        #region Views

        private CheckBox _removeFromQuotationMarksCheckbox;

        public CheckBox RemoveFromQuotationMarksCheckbox => _removeFromQuotationMarksCheckbox ?? (_removeFromQuotationMarksCheckbox = FindViewById<CheckBox>(Resource.Id.RemoveFromQuotationMarksCheckbox));
       
        #endregion
    }
}