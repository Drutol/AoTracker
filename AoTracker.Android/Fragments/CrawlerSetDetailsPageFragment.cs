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
using AoLibs.Utilities.Android.Listeners;
using AoTracker.Android.Utils;
using AoTracker.Crawlers.Enums;
using AoTracker.Crawlers.Mandarake;
using AoTracker.Crawlers.Surugaya;
using AoTracker.Domain.Enums;
using AoTracker.Domain.Models;
using AoTracker.Infrastructure.Models;
using AoTracker.Infrastructure.Models.NavArgs;
using AoTracker.Infrastructure.ViewModels;
using AoTracker.Infrastructure.ViewModels.Item;
using AoTracker.Resources;
using GalaSoft.MvvmLight.Helpers;
using PopupMenu = Android.Support.V7.Widget.PopupMenu;

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
                    new RecyclerViewAdapterBuilder<CrawlerDescriptorViewModel, RecyclerView.ViewHolder>()
                        .WithItems(ViewModel.CrawlerDescriptors)
                        .WithContentStretching()
                        .WithMultipleViews()
                        .WithGroup<CrawlerDescriptorViewModel<SurugayaItem>, SurugayaCrawlerHolder>(builder =>
                        {
                            builder.WithResourceId(LayoutInflater, Resource.Layout.item_surugaya_crawler);
                            builder.WithDataTemplate(SurugayaCrawlerDescriptorDataTemplate);
                        })
                        .WithGroup<CrawlerDescriptorViewModel<MandarakeItem>, MandarakeCrawlerHolder>(builder =>
                        {
                            builder.WithResourceId(LayoutInflater, Resource.Layout.item_mandarake_crawler);
                            builder.WithDataTemplate(MandarakeCrawlerDescriptorDataTemplate);
                        })
                        .Build());
            }));

            AddCrawlersRecyclerView.SetAdapter(
                new ObservableRecyclerAdapter<CrawlerEntryViewModel, AddCrawlerHolder>(
                    ViewModel.CrawlerEntries,
                    AddCrawlerDataTemplate,
                    LayoutInflater,
                    Resource.Layout.item_add_crawler));

            Bindings.Add(
                this.SetBinding(() => ViewModel.SetName,
                    () => TitleTextBox.Text, BindingMode.TwoWay));

            CrawlersRecyclerView.SetLayoutManager(new LinearLayoutManager(Activity));
            AddCrawlersRecyclerView.SetLayoutManager(new GridLayoutManager(Activity, 3));
        }

        private void SurugayaCrawlerDescriptorDataTemplate(CrawlerDescriptorViewModel<SurugayaItem> item, SurugayaCrawlerHolder holder, int position)
        {
            BaseCrawlerTemplate(item, holder);
        }

        private void MandarakeCrawlerDescriptorDataTemplate(CrawlerDescriptorViewModel<MandarakeItem> item, MandarakeCrawlerHolder holder, int position)
        {
            BaseCrawlerTemplate(item, holder);
        }

        private void BaseCrawlerTemplate(CrawlerDescriptorViewModel item, ICrawlerHolder holder)
        {
            holder.ClickSurface.SetOnClickCommand(ViewModel.SelectCrawlerDescriptorCommand, item);
            holder.ClickSurface.SetOnLongClickListener(new OnLongClickListener(view =>
            {
                var menu = new PopupMenu(Activity, holder.ItemView);
                menu.Menu.Add("Delete");
                menu.MenuItemClick += (sender, args) =>
                {
                    if (args.Item.ItemId == 0)
                    {
                        ViewModel.RemoveDescriptorCommand.Execute(item);
                    }
                };
                menu.Show();
            }));
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
            base.NavigatedTo();
            ViewModel.NavigatedTo(NavigationArguments as CrawlerSetDetailsPageNavArgs);
        }

        public override void NavigatedBack()
        {
            base.NavigatedBack();
            ViewModel.NavigatedBack();
        }

        public override void NavigatedFrom()
        {
            base.NavigatedFrom();
            ViewModel.NavigatedFrom();
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

        interface ICrawlerHolder
        {
            View ItemView { get; }
            LinearLayout ClickSurface { get; }
        }

        class SurugayaCrawlerHolder : BindingViewHolderBase<CrawlerDescriptorViewModel<SurugayaItem>>, ICrawlerHolder
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

        class MandarakeCrawlerHolder : BindingViewHolderBase<CrawlerDescriptorViewModel<MandarakeItem>>, ICrawlerHolder
        {
            private readonly View _view;

            public MandarakeCrawlerHolder(View view) : base(view)
            {
                _view = view;
            }

            protected override void SetBindings()
            {
                Bindings.Add(this.SetBinding(() => ViewModel.CrawlerSourceParameters).WhenSourceChanges(() =>
                {
                    var param = ViewModel.CrawlerSourceParameters as MandarakeSourceParameters;

                    SearchPhrase.Text = param.SearchQuery;
                    PriceIncrease.Text =
                        $"+{param.OffsetIncrease}¥  +{param.PercentageIncrease}%";
                }));
            }

            private TextView _searchPhrase;
            private TextView _priceIncrease;
            private LinearLayout _clickSurface;

            public TextView SearchPhrase => _searchPhrase ?? (_searchPhrase = _view.FindViewById<TextView>(Resource.Id.SearchPhrase));
            public TextView PriceIncrease => _priceIncrease ?? (_priceIncrease = _view.FindViewById<TextView>(Resource.Id.PriceIncrease));
            public LinearLayout ClickSurface => _clickSurface ?? (_clickSurface = _view.FindViewById<LinearLayout>(Resource.Id.ClickSurface));
        }


    }
}