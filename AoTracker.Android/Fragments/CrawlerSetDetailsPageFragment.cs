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
using Android.Text;
using Android.Views;
using Android.Widget;
using AoLibs.Adapters.Android.Recycler;
using AoLibs.Navigation.Android.Navigation.Attributes;
using AoLibs.Utilities.Android;
using AoTracker.Crawlers.Enums;
using AoTracker.Crawlers.Surugaya;
using AoTracker.Domain.Enums;
using AoTracker.Domain.Models;
using AoTracker.Infrastructure.Models;
using AoTracker.Infrastructure.Models.NavArgs;
using AoTracker.Infrastructure.ViewModels;
using AoTracker.Infrastructure.ViewModels.Item;
using AoTracker.Resources;
using GalaSoft.MvvmLight.Helpers;

namespace AoTracker.Android.Fragments
{
    [NavigationPage(PageIndex.CrawlerSetDetails)]
    public class CrawlerSetDetailsPageFragment  : CustomFragmentBase<CrawlerSetDetailsViewModel>
    {
        public override int LayoutResourceId { get; } = Resource.Layout.page_crawler_set_details;

        protected override void InitBindings()
        {
            Bindings.Add(this.SetBinding(() => ViewModel.CrawlerDescriptors).WhenSourceChanges(() =>
            {
                CrawlersRecyclerView.SetAdapter(
                    new ObservableRecyclerAdapter<CrawlerDescriptorViewModel, SurugayaCrawlerHolder>(
                        ViewModel.CrawlerDescriptors,
                        CrawlerDescriptorDataTemplate,
                        LayoutInflater,
                        Resource.Layout.item_surugaya_crawler) {StretchContentHorizonatally = true});
            }));

            AddCrawlersRecyclerView.SetAdapter(
                new ObservableRecyclerAdapter<CrawlerEntryViewModel, AddCrawlerHolder>(
                    ViewModel.CrawlerEntries,
                    AddCrawlerDataTemplate,
                    LayoutInflater,
                    Resource.Layout.item_add_crawler));

            Bindings.Add(
                this.SetBinding(() => ViewModel.Title,
                    () => TitleTextBox.Text, BindingMode.TwoWay));



            CrawlersRecyclerView.SetLayoutManager(new LinearLayoutManager(Activity));
            AddCrawlersRecyclerView.SetLayoutManager(new GridLayoutManager(Activity, 3));
        }

        private void CrawlerDescriptorDataTemplate(CrawlerDescriptorViewModel item, SurugayaCrawlerHolder holder, int position)
        {
            holder.ClickSurface.SetOnClickCommand(ViewModel.SelectCrawlerDescriptorCommand, item);
        }

        private void AddCrawlerDataTemplate(CrawlerEntryViewModel item, AddCrawlerHolder holder, int position)
        {
            holder.Subtitle.Text = item.BackingModel.Title;
            holder.Image.SetImageResource(GetResource());
            holder.ClickSurface.SetOnClickCommand(ViewModel.AddCrawlerCommand, item);

            int GetResource()
            {
                if (item.BackingModel.CrawlerDomain == CrawlerDomain.Mandarake)
                    return Resource.Drawable.mandarake;
                if (item.BackingModel.CrawlerDomain == CrawlerDomain.Surugaya)
                    return Resource.Drawable.surugaya;
                return 0;
            }
        }

        public override void NavigatedTo()
        {
            ViewModel.NavigatedTo(NavigationArguments as CrawlerSetDetailsPageNavArgs);
        }

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


        class SurugayaCrawlerHolder : BindingViewHolderBase<CrawlerDescriptorViewModel>
        {
            private readonly View _view;

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
                    PriceIncrease.Text =
                        $"+{param.OffsetIncrease}¥  +{param.PercentageIncrease}%";
                    RemovesQuotationMarksIndicator.Visibility =
                        param.TrimJapaneseQuotationMarks ? ViewStates.Visible : ViewStates.Gone;
                }));
            }

            private ImageView _image;
            private TextView _searchPhrase;
            private TextView _priceIncrease;
            private LinearLayout _removesQuotationMarksIndicator;
            private LinearLayout _clickSurface;

            public ImageView Image => _image ?? (_image = _view.FindViewById<ImageView>(Resource.Id.Image));
            public TextView SearchPhrase => _searchPhrase ?? (_searchPhrase = _view.FindViewById<TextView>(Resource.Id.SearchPhrase));
            public TextView PriceIncrease => _priceIncrease ?? (_priceIncrease = _view.FindViewById<TextView>(Resource.Id.PriceIncrease));
            public LinearLayout RemovesQuotationMarksIndicator => _removesQuotationMarksIndicator ?? (_removesQuotationMarksIndicator = _view.FindViewById<LinearLayout>(Resource.Id.RemovesQuotationMarksIndicator));
            public LinearLayout ClickSurface => _clickSurface ?? (_clickSurface = _view.FindViewById<LinearLayout>(Resource.Id.ClickSurface));
        }

    }
}