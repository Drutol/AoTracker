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
using Android.Support.V7.Widget.Helper;
using Android.Views;
using Android.Widget;
using AoLibs.Adapters.Android.Recycler;
using AoTracker.Android.Utils;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Crawlers.Mandarake;
using AoTracker.Crawlers.Sites.Lashinbang;
using AoTracker.Crawlers.Sites.Mercari;
using AoTracker.Crawlers.Sites.Yahoo;
using AoTracker.Crawlers.Surugaya;
using AoTracker.Infrastructure.ViewModels.Item;
using GalaSoft.MvvmLight.Helpers;

namespace AoTracker.Android.Fragments
{
    public partial class CrawlerSetDetailsPageFragment
    {
        #region Views

        private TextInputEditText _titleTextBox;
        private TextInputLayout _titleInputLayout;
        private RecyclerView _crawlersRecyclerView;
        private ImageView _emptyStateIcon;
        private TextView _emptyStateSubtitle;
        private LinearLayout _emptyState;
        private RecyclerView _addCrawlersRecyclerView;

        public TextInputEditText TitleTextBox => _titleTextBox ?? (_titleTextBox = FindViewById<TextInputEditText>(Resource.Id.TitleTextBox));
        public TextInputLayout TitleInputLayout => _titleInputLayout ?? (_titleInputLayout = FindViewById<TextInputLayout>(Resource.Id.TitleInputLayout));
        public RecyclerView CrawlersRecyclerView => _crawlersRecyclerView ?? (_crawlersRecyclerView = FindViewById<RecyclerView>(Resource.Id.CrawlersRecyclerView));
        public ImageView EmptyStateIcon => _emptyStateIcon ?? (_emptyStateIcon = FindViewById<ImageView>(Resource.Id.EmptyStateIcon));
        public TextView EmptyStateSubtitle => _emptyStateSubtitle ?? (_emptyStateSubtitle = FindViewById<TextView>(Resource.Id.EmptyStateSubtitle));
        public LinearLayout EmptyState => _emptyState ?? (_emptyState = FindViewById<LinearLayout>(Resource.Id.EmptyState));
        public RecyclerView AddCrawlersRecyclerView => _addCrawlersRecyclerView ?? (_addCrawlersRecyclerView = FindViewById<RecyclerView>(Resource.Id.AddCrawlersRecyclerView));

        #endregion

        class AddCrawlerHolder : RecyclerView.ViewHolder
        {
            private readonly View _view;

            public AddCrawlerHolder(View view) : base(view)
            {
                _view = view;
            }
            private ImageView _image;
            private TextView _subtitle;
            private FrameLayout _clickSurface;

            public ImageView Image => _image ?? (_image = _view.FindViewById<ImageView>(Resource.Id.Image));
            public TextView Subtitle => _subtitle ?? (_subtitle = _view.FindViewById<TextView>(Resource.Id.Subtitle));
            public FrameLayout ClickSurface => _clickSurface ?? (_clickSurface = _view.FindViewById<FrameLayout>(Resource.Id.ClickSurface));
        }

        interface ICrawlerHolder
        {
            View ItemView { get; }
            LinearLayout ClickSurface { get; }
            object ViewModelProxy { get; set; }
        }

        class SurugayaCrawlerHolder : SharedCrawlerHolder<CrawlerDescriptorViewModel<SurugayaItem>>, ICrawlerHolder
        {

            public SurugayaCrawlerHolder(View view) : base(view)
            {

            }

            protected override void OnParamsChanged(ICrawlerSourceParameters parameters)
            {
                var param = (SurugayaSourceParameters) parameters;
                RemovesQuotationMarksIndicator.Visibility =
                    param.TrimJapaneseQuotationMarks ? ViewStates.Visible : ViewStates.Gone;
            }

            private LinearLayout _removesQuotationMarksIndicator;

            public LinearLayout RemovesQuotationMarksIndicator => _removesQuotationMarksIndicator ?? (_removesQuotationMarksIndicator = _view.FindViewById<LinearLayout>(Resource.Id.RemovesQuotationMarksIndicator));
            
        }



        abstract class SharedCrawlerHolder<T> : BindingViewHolderBase<T> where T : CrawlerDescriptorViewModel
        {
            // For code generation purpose
            // ReSharper disable once InconsistentNaming
            protected readonly View _view;

            public object ViewModelProxy
            {
                get => ViewModel;
                set => ViewModel = (T)value;
            }

            protected SharedCrawlerHolder(View view) : base(view)
            {
                _view = view;
            }

            protected virtual void OnParamsChanged(ICrawlerSourceParameters parameters)
            {

            }

            protected override void SetBindings()
            {
                Image.SetImageResource(ViewModel.BackingModel.CrawlerDomain.ToImageResource());

                Bindings.Add(this.SetBinding(() => ViewModel.CrawlerSourceParameters).WhenSourceChanges(() =>
                {
                    var param = ViewModel.CrawlerSourceParameters;

                    SearchPhrase.Text = param.SearchQuery;
                    if (Math.Abs(param.PercentageIncrease) < 0.001 && Math.Abs(param.OffsetIncrease) < 0.001)
                    {
                        PriceIncreaseSection.Visibility = PriceIncreaseSectionDivider.Visibility = ViewStates.Gone;
                    }
                    else
                    {
                        PriceIncreaseSection.Visibility = PriceIncreaseSectionDivider.Visibility = ViewStates.Visible;
                        PriceIncrease.Text = $"+{param.OffsetIncrease}¥  +{param.PercentageIncrease}%";
                    }   
                    
                    if (!param.ExcludedKeywords?.Any() ?? true)
                    {
                        ExcludedKeywordsSection.Visibility = ExcludedKeywordsSectionDivider.Visibility = ViewStates.Gone;
                    }
                    else
                    {
                        ExcludedKeywordsSection.Visibility = ExcludedKeywordsSectionDivider.Visibility = ViewStates.Visible;
                        ExcludedKeywords.Text = string.Join(", ", param.ExcludedKeywords);
                    }

                    OnParamsChanged(param);
                }));
            }
            private ImageView _image;
            private TextView _searchPhrase;
            private View _excludedKeywordsSectionDivider;
            private TextView _excludedKeywords;
            private LinearLayout _excludedKeywordsSection;
            private View _priceIncreaseSectionDivider;
            private TextView _priceIncrease;
            private LinearLayout _priceIncreaseSection;
            private LinearLayout _clickSurface;

            public ImageView Image => _image ?? (_image = _view.FindViewById<ImageView>(Resource.Id.Image));
            public TextView SearchPhrase => _searchPhrase ?? (_searchPhrase = _view.FindViewById<TextView>(Resource.Id.SearchPhrase));
            public View ExcludedKeywordsSectionDivider => _excludedKeywordsSectionDivider ?? (_excludedKeywordsSectionDivider = _view.FindViewById<View>(Resource.Id.ExcludedKeywordsSectionDivider));
            public TextView ExcludedKeywords => _excludedKeywords ?? (_excludedKeywords = _view.FindViewById<TextView>(Resource.Id.ExcludedKeywords));
            public LinearLayout ExcludedKeywordsSection => _excludedKeywordsSection ?? (_excludedKeywordsSection = _view.FindViewById<LinearLayout>(Resource.Id.ExcludedKeywordsSection));
            public View PriceIncreaseSectionDivider => _priceIncreaseSectionDivider ?? (_priceIncreaseSectionDivider = _view.FindViewById<View>(Resource.Id.PriceIncreaseSectionDivider));
            public TextView PriceIncrease => _priceIncrease ?? (_priceIncrease = _view.FindViewById<TextView>(Resource.Id.PriceIncrease));
            public LinearLayout PriceIncreaseSection => _priceIncreaseSection ?? (_priceIncreaseSection = _view.FindViewById<LinearLayout>(Resource.Id.PriceIncreaseSection));
            public LinearLayout ClickSurface => _clickSurface ?? (_clickSurface = _view.FindViewById<LinearLayout>(Resource.Id.ClickSurface));
        }



        class MandarakeCrawlerHolder : SharedCrawlerHolder<CrawlerDescriptorViewModel<MandarakeItem>>, ICrawlerHolder
        {
            public MandarakeCrawlerHolder(View view) : base(view)
            {

            }
        }

        class YahooCrawlerHolder : SharedCrawlerHolder<CrawlerDescriptorViewModel<YahooItem>>, ICrawlerHolder
        {
            public YahooCrawlerHolder(View view) : base(view)
            {

            }
        }

        class MercariCrawlerHolder : SharedCrawlerHolder<CrawlerDescriptorViewModel<MercariItem>>, ICrawlerHolder
        {
            public MercariCrawlerHolder(View view) : base(view)
            {

            }
        }

        class LashinbangCrawlerHolder : SharedCrawlerHolder<CrawlerDescriptorViewModel<LashinbangItem>>, ICrawlerHolder
        {
            public LashinbangCrawlerHolder(View view) : base(view)
            {

            }
        }
    }
}