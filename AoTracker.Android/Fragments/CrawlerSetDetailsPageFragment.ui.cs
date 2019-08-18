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
        private RecyclerView _crawlersRecyclerView;
        private RecyclerView _addCrawlersRecyclerView;

        public TextInputEditText TitleTextBox => _titleTextBox ?? (_titleTextBox = FindViewById<TextInputEditText>(Resource.Id.TitleTextBox));
        public RecyclerView CrawlersRecyclerView => _crawlersRecyclerView ?? (_crawlersRecyclerView = FindViewById<RecyclerView>(Resource.Id.CrawlersRecyclerView));
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

        class SurugayaCrawlerHolder : BindingViewHolderBase<CrawlerDescriptorViewModel<SurugayaItem>>, ICrawlerHolder
        {
            private readonly View _view;

            public object ViewModelProxy
            {
                get => ViewModel;
                set => ViewModel = (CrawlerDescriptorViewModel<SurugayaItem>)value;
            }

            public SurugayaCrawlerHolder(View view) : base(view)
            {
                _view = view;
            }

            protected override void SetBindings()
            {
                Bindings.Add(this.SetBinding(() => ViewModel.CrawlerSourceParameters).WhenSourceChanges(() =>
                {
                    var param = ViewModel.CrawlerSourceParameters as SurugayaSourceParameters;

                    SearchPhrase.Text = param.SearchQuery;
                    if (param.PercentageIncrease == 0 && param.OffsetIncrease == 0)
                    {
                        PriceIncreaseSection.Visibility = PriceIncreaseSectionDivider.Visibility = ViewStates.Gone;
                    }
                    else
                    {
                        PriceIncreaseSection.Visibility = PriceIncreaseSectionDivider.Visibility = ViewStates.Visible;
                        PriceIncrease.Text = $"+{param.OffsetIncrease}¥  +{param.PercentageIncrease}%";
                    }
                    RemovesQuotationMarksIndicator.Visibility =
                        param.TrimJapaneseQuotationMarks ? ViewStates.Visible : ViewStates.Gone;
                }));
            }

            private ImageView _image;
            private TextView _searchPhrase;
            private View _priceIncreaseSectionDivider;
            private TextView _priceIncrease;
            private LinearLayout _priceIncreaseSection;
            private LinearLayout _removesQuotationMarksIndicator;
            private LinearLayout _clickSurface;
            private object _viewModelProxy;

            public ImageView Image => _image ?? (_image = _view.FindViewById<ImageView>(Resource.Id.Image));
            public TextView SearchPhrase => _searchPhrase ?? (_searchPhrase = _view.FindViewById<TextView>(Resource.Id.SearchPhrase));
            public View PriceIncreaseSectionDivider => _priceIncreaseSectionDivider ?? (_priceIncreaseSectionDivider = _view.FindViewById<View>(Resource.Id.PriceIncreaseSectionDivider));
            public TextView PriceIncrease => _priceIncrease ?? (_priceIncrease = _view.FindViewById<TextView>(Resource.Id.PriceIncrease));
            public LinearLayout PriceIncreaseSection => _priceIncreaseSection ?? (_priceIncreaseSection = _view.FindViewById<LinearLayout>(Resource.Id.PriceIncreaseSection));
            public LinearLayout RemovesQuotationMarksIndicator => _removesQuotationMarksIndicator ?? (_removesQuotationMarksIndicator = _view.FindViewById<LinearLayout>(Resource.Id.RemovesQuotationMarksIndicator));
            public LinearLayout ClickSurface => _clickSurface ?? (_clickSurface = _view.FindViewById<LinearLayout>(Resource.Id.ClickSurface));
        }


        abstract class SharedCrawlerHolder<T> : BindingViewHolderBase<T> where T : CrawlerDescriptorViewModel
        {
            private View _view;

            public object ViewModelProxy
            {
                get => ViewModel;
                set => ViewModel = (T)value;
            }

            protected SharedCrawlerHolder(View view) : base(view)
            {
                _view = view;
            }

            protected override void SetBindings()
            {
                Image.SetImageResource(ViewModel.BackingModel.CrawlerDomain.ToImageResource());

                Bindings.Add(this.SetBinding(() => ViewModel.CrawlerSourceParameters).WhenSourceChanges(() =>
                {
                    var param = ViewModel.CrawlerSourceParameters;

                    SearchPhrase.Text = param.SearchQuery;
                    if (param.PercentageIncrease == 0 && param.OffsetIncrease == 0)
                    {
                        PriceIncreaseSection.Visibility = PriceIncreaseSectionDivider.Visibility = ViewStates.Gone;
                    }
                    else
                    {
                        PriceIncreaseSection.Visibility = PriceIncreaseSectionDivider.Visibility = ViewStates.Visible;
                        PriceIncrease.Text = $"+{param.OffsetIncrease}¥  +{param.PercentageIncrease}%";
                    }
                }));
            }
            private ImageView _image;
            private TextView _searchPhrase;
            private View _priceIncreaseSectionDivider;
            private TextView _priceIncrease;
            private LinearLayout _priceIncreaseSection;
            private LinearLayout _clickSurface;

            public ImageView Image => _image ?? (_image = _view.FindViewById<ImageView>(Resource.Id.Image));
            public TextView SearchPhrase => _searchPhrase ?? (_searchPhrase = _view.FindViewById<TextView>(Resource.Id.SearchPhrase));
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