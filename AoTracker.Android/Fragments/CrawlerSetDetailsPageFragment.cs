using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Views;
using Android.Widget;
using AoLibs.Adapters.Android.Recycler;
using AoLibs.Navigation.Android.Navigation.Attributes;
using AoTracker.Crawlers.Enums;
using AoTracker.Domain.Enums;
using AoTracker.Domain.Models;
using AoTracker.Infrastructure.Models;
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
            CrawlersRecyclerView.SetAdapter(new ObservableRecyclerAdapter<CrawlerDescriptorViewModel, SurugayaCrawlerHolder>(ViewModel.CrawlerDescriptors, CrawlerDescriptorDataTemplate, LayoutInflater,
                Resource.Layout.item_surugaya_crawler ));
            CrawlersRecyclerView.SetLayoutManager(new LinearLayoutManager(Activity));

            AddCrawlersRecyclerView.SetAdapter(new ObservableRecyclerAdapter<CrawlerEntryViewModel, AddCrawlerHolder>(
                ViewModel.CrawlerEntries, AddCrawlerDataTemplate, LayoutInflater,
                Resource.Layout.item_add_crawler));
            AddCrawlersRecyclerView.SetLayoutManager(new GridLayoutManager(Activity, 3));
        }

        private void CrawlerDescriptorDataTemplate(CrawlerDescriptorViewModel item, SurugayaCrawlerHolder holder, int position)
        {

        }

        private void AddCrawlerDataTemplate(CrawlerEntryViewModel item, AddCrawlerHolder holder, int position)
        {
            holder.Subtitle.Text = item.BackingModel.Title;
            holder.Image.SetImageResource(GetResource());

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
            ViewModel.NavigatedTo(NavigationArguments as CrawlerSet);
        }

        #region Views

        private RecyclerView _crawlersRecyclerView;
        private RecyclerView _addCrawlersRecyclerView;

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

            public ImageView Image => _image ?? (_image = _view.FindViewById<ImageView>(Resource.Id.Image));
            public TextView Subtitle => _subtitle ?? (_subtitle = _view.FindViewById<TextView>(Resource.Id.Subtitle));
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
                Bindings.Add(
                    this.SetBinding(() => ViewModel.Title,
                        () => Title.Text));

                Bindings.Add(this.SetBinding(() => ViewModel.CrawlerSourceParameters.SearchQuery).WhenSourceChanges(
                    () =>
                    {
                        SearchPhrase.Text =
                            $"{AppResources.Item_CrawlerDescriptor_SearchPhrase} {ViewModel.CrawlerSourceParameters.SearchQuery}";
                    }));
            }

            private ImageView _image;
            private TextView _title;
            private TextView _searchPhrase;

            public ImageView Image => _image ?? (_image = _view.FindViewById<ImageView>(Resource.Id.Image));
            public TextView Title => _title ?? (_title = _view.FindViewById<TextView>(Resource.Id.Title));
            public TextView SearchPhrase => _searchPhrase ?? (_searchPhrase = _view.FindViewById<TextView>(Resource.Id.SearchPhrase));
        }

    }
}