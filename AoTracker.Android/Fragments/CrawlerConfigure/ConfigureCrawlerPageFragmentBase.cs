using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using AoLibs.Utilities.Android;
using AoLibs.Utilities.Android.Listeners;
using AoTracker.Android.Utils;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Infrastructure.Models.NavArgs;
using AoTracker.Infrastructure.ViewModels.Crawlers;
using AoTracker.Resources;
using GalaSoft.MvvmLight.Helpers;

namespace AoTracker.Android.Fragments.CrawlerConfigure
{
    abstract class ConfigureCrawlerPageFragmentBase<TViewModel, TSourceParams> : CustomFragmentBase<TViewModel>
        where TViewModel : ConfigureCrawlerViewModelBase<TSourceParams>
        where TSourceParams : class, ICrawlerSourceParameters, new()
    {
        protected override void InitBindings()
        {
            EmptyStateIcon.SetImageResource(Resource.Drawable.icon_add);
            EmptyStateSubtitle.Text = AppResources.EmptyState_Subtitle_ConfigureCrawler;

            ViewModel.ExcludedKeywords.SetUpWithEmptyState(EmptyState);

            ExcludedKeywordsRecyclerView.SetAdapter(new RecyclerViewAdapterBuilder<string, ExcludedItemHolder>()
                .WithItems(ViewModel.ExcludedKeywords)
                .WithContentStretching()
                .WithResourceId(LayoutInflater, Resource.Layout.item_excluded_term)
                .WithDataTemplate(DataTemplate)
                .Build());
            ExcludedKeywordsRecyclerView.SetLayoutManager(new LinearLayoutManager(Activity));

            Bindings.Add(
                this.SetBinding(() => ViewModel.SearchQueryInput,
                    () => SearchQueryTextBox.Text, BindingMode.TwoWay));

            Bindings.Add(
                this.SetBinding(() => ViewModel.CostOffsetIncrease,
                    () => OffsetIncreaseTextBox.Text, BindingMode.TwoWay));

            Bindings.Add(
                this.SetBinding(() => ViewModel.CostPercentageIncrease,
                    () => PercentageIncreaseTextBox.Text, BindingMode.TwoWay));

            AddExcludedKeywordButton.SetOnClickListener(new OnClickListener(view =>
            {
                ViewModel.AddExcludedKeywordCommand.Execute(ExcludedKeywordInput.Text);
            }));
        }

        private void DataTemplate(string item, ExcludedItemHolder holder, int position)
        {
            holder.Text.Text = item;
            holder.DeleteButton.SetOnClickCommand(ViewModel.RemoveExcludedKeywordCommand, item);
        }

        public override void NavigatedTo()
        {
            base.NavigatedTo();
            ViewModel.NavigatedTo(NavigationArguments as ConfigureCrawlerPageNavArgs);
        }

        public override void NavigatedFrom()
        {
            base.NavigatedFrom();
            ViewModel.NavigatedFrom();
        }

        #region Views

        private ImageView _domainIcon;
        private TextInputEditText _searchQueryTextBox;
        private TextInputEditText _percentageIncreaseTextBox;
        private TextInputEditText _offsetIncreaseTextBox;
        private RecyclerView _excludedKeywordsRecyclerView;
        private ImageView _emptyStateIcon;
        private TextView _emptyStateSubtitle;
        private LinearLayout _emptyState;
        private TextInputEditText _excludedKeywordInput;
        private ImageButton _addExcludedKeywordButton;

        public ImageView DomainIcon => _domainIcon ?? (_domainIcon = FindViewById<ImageView>(Resource.Id.DomainIcon));
        public TextInputEditText SearchQueryTextBox => _searchQueryTextBox ?? (_searchQueryTextBox = FindViewById<TextInputEditText>(Resource.Id.SearchQueryTextBox));
        public TextInputEditText PercentageIncreaseTextBox => _percentageIncreaseTextBox ?? (_percentageIncreaseTextBox = FindViewById<TextInputEditText>(Resource.Id.PercentageIncreaseTextBox));
        public TextInputEditText OffsetIncreaseTextBox => _offsetIncreaseTextBox ?? (_offsetIncreaseTextBox = FindViewById<TextInputEditText>(Resource.Id.OffsetIncreaseTextBox));
        public RecyclerView ExcludedKeywordsRecyclerView => _excludedKeywordsRecyclerView ?? (_excludedKeywordsRecyclerView = FindViewById<RecyclerView>(Resource.Id.ExcludedKeywordsRecyclerView));
        public ImageView EmptyStateIcon => _emptyStateIcon ?? (_emptyStateIcon = FindViewById<ImageView>(Resource.Id.EmptyStateIcon));
        public TextView EmptyStateSubtitle => _emptyStateSubtitle ?? (_emptyStateSubtitle = FindViewById<TextView>(Resource.Id.EmptyStateSubtitle));
        public LinearLayout EmptyState => _emptyState ?? (_emptyState = FindViewById<LinearLayout>(Resource.Id.EmptyState));
        public TextInputEditText ExcludedKeywordInput => _excludedKeywordInput ?? (_excludedKeywordInput = FindViewById<TextInputEditText>(Resource.Id.ExcludedKeywordInput));
        public ImageButton AddExcludedKeywordButton => _addExcludedKeywordButton ?? (_addExcludedKeywordButton = FindViewById<ImageButton>(Resource.Id.AddExcludedKeywordButton));

        #endregion

        class ExcludedItemHolder : RecyclerView.ViewHolder
        {
            private readonly View _view;

            public ExcludedItemHolder(View view) : base(view)
            {
                _view = view;
            }
            private TextView _text;
            private ImageButton _deleteButton;

            public TextView Text => _text ?? (_text = _view.FindViewById<TextView>(Resource.Id.Text));
            public ImageButton DeleteButton => _deleteButton ?? (_deleteButton = _view.FindViewById<ImageButton>(Resource.Id.DeleteButton));
        }

    }
}