﻿using System;
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
using AoTracker.Domain.Enums;
using AoTracker.Infrastructure.Models.NavArgs;
using AoTracker.Infrastructure.ViewModels;
using GalaSoft.MvvmLight.Helpers;

namespace AoTracker.Android.Fragments.CrawlerConfigure
{
    [NavigationPage(PageIndex.ConfigureSurugaya)]
    class ConfigureSurugayaCrawlerPageFragment : CustomFragmentBase<ConfigureSurugayaCrawlerViewModel>
    {
        public override int LayoutResourceId { get; } = Resource.Layout.page_configure_surugaya_crawler;

        protected override void InitBindings()
        {
            Bindings.Add(
                this.SetBinding(() => ViewModel.SearchQueryInput,
                    () => SearchQueryTextBox.Text, BindingMode.TwoWay));

            Bindings.Add(
                this.SetBinding(() => ViewModel.CostOffsetIncrease,
                    () => OffsetIncreaseTextBox.Text, BindingMode.TwoWay));

            Bindings.Add(
                this.SetBinding(() => ViewModel.CostPercentageIncrease,
                    () => PercentageIncreaseTextBox.Text, BindingMode.TwoWay));

            Bindings.Add(
                this.SetBinding(() => ViewModel.TrimJapaneseQuotationMarks,
                    () => RemoveFromQuotationMarksCheckbox.Checked, BindingMode.OneWay));

            SaveButton.SetOnClickCommand(ViewModel.SaveCommand);

        }

        public override void NavigatedTo()
        {
            ViewModel.NavigatedTo(NavigationArguments as ConfigureCrawlerPageNavArgs);
        }

        #region Views

        private TextInputEditText _searchQueryTextBox;
        private TextInputEditText _percentageIncreaseTextBox;
        private TextInputEditText _offsetIncreaseTextBox;
        private CheckBox _removeFromQuotationMarksCheckbox;
        private Button _saveButton;

        public TextInputEditText SearchQueryTextBox => _searchQueryTextBox ?? (_searchQueryTextBox = FindViewById<TextInputEditText>(Resource.Id.SearchQueryTextBox));
        public TextInputEditText PercentageIncreaseTextBox => _percentageIncreaseTextBox ?? (_percentageIncreaseTextBox = FindViewById<TextInputEditText>(Resource.Id.PercentageIncreaseTextBox));
        public TextInputEditText OffsetIncreaseTextBox => _offsetIncreaseTextBox ?? (_offsetIncreaseTextBox = FindViewById<TextInputEditText>(Resource.Id.OffsetIncreaseTextBox));
        public CheckBox RemoveFromQuotationMarksCheckbox => _removeFromQuotationMarksCheckbox ?? (_removeFromQuotationMarksCheckbox = FindViewById<CheckBox>(Resource.Id.RemoveFromQuotationMarksCheckbox));
        public Button SaveButton => _saveButton ?? (_saveButton = FindViewById<Button>(Resource.Id.SaveButton));

        #endregion
    }
}