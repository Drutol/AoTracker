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
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Widget;
using AoLibs.Adapters.Android.Recycler;
using AoLibs.Navigation.Android.Navigation.Attributes;
using AoLibs.Utilities.Android;
using AoLibs.Utilities.Android.Listeners;
using AoTracker.Android.Utils;
using AoTracker.Crawlers.Enums;
using AoTracker.Crawlers.Interfaces;
using AoTracker.Crawlers.Mandarake;
using AoTracker.Crawlers.Sites.Lashinbang;
using AoTracker.Crawlers.Sites.Mercari;
using AoTracker.Crawlers.Sites.Yahoo;
using AoTracker.Crawlers.Surugaya;
using AoTracker.Domain.Enums;
using AoTracker.Domain.Models;
using AoTracker.Infrastructure.Models;
using AoTracker.Infrastructure.Models.NavArgs;
using AoTracker.Infrastructure.ViewModels;
using AoTracker.Infrastructure.ViewModels.Item;
using AoTracker.Resources;
using Com.Google.Android.Flexbox;
using GalaSoft.MvvmLight.Helpers;
using PopupMenu = Android.Support.V7.Widget.PopupMenu;

namespace AoTracker.Android.Fragments
{
    [NavigationPage(PageIndex.CrawlerSetDetails)]
    public partial class CrawlerSetDetailsPageFragment  : CustomFragmentBase<CrawlerSetDetailsViewModel>
    {
        public override int LayoutResourceId { get; } = Resource.Layout.page_crawler_set_details;

        protected override void InitBindings()
        {
            CrawlersRecyclerView.SetAdapter(
                new RecyclerViewAdapterBuilder<CrawlerDescriptorViewModel, RecyclerView.ViewHolder>()
                    .WithItems(ViewModel.CrawlerDescriptors)
                    .WithContentStretching()
                    .WithMultipleViews()
                    .WithGroup<CrawlerDescriptorViewModel<SurugayaItem>, SurugayaCrawlerHolder>(builder =>
                    {
                        builder.WithResourceId(LayoutInflater, Resource.Layout.item_surugaya_crawler);
                        builder.WithDataTemplate(SharedCrawlerDescriptorDataTemplate);
                    })
                    .WithGroup<CrawlerDescriptorViewModel<MandarakeItem>, MandarakeCrawlerHolder>(builder =>
                    {
                        builder.WithResourceId(LayoutInflater, Resource.Layout.item_mandarake_crawler);
                        builder.WithDataTemplate(SharedCrawlerDescriptorDataTemplate);
                    })
                    .WithGroup<CrawlerDescriptorViewModel<MercariItem>, MercariCrawlerHolder>(builder =>
                    {
                        builder.WithResourceId(LayoutInflater, Resource.Layout.item_mandarake_crawler);
                        builder.WithDataTemplate(SharedCrawlerDescriptorDataTemplate);
                    })
                    .WithGroup<CrawlerDescriptorViewModel<YahooItem>, YahooCrawlerHolder>(builder =>
                    {
                        builder.WithResourceId(LayoutInflater, Resource.Layout.item_mandarake_crawler);
                        builder.WithDataTemplate(SharedCrawlerDescriptorDataTemplate);
                    })
                    .WithGroup<CrawlerDescriptorViewModel<LashinbangItem>, LashinbangCrawlerHolder>(builder =>
                    {
                        builder.WithResourceId(LayoutInflater, Resource.Layout.item_mandarake_crawler);
                        builder.WithDataTemplate(SharedCrawlerDescriptorDataTemplate);
                    })
                    .Build());
            ViewModel.CrawlerDescriptors.SetUpWithEmptyState(EmptyState);

            AddCrawlersRecyclerView.SetAdapter(
                new ObservableRecyclerAdapter<CrawlerEntryViewModel, AddCrawlerHolder>(
                    ViewModel.CrawlerEntries,
                    AddCrawlerDataTemplate,
                    LayoutInflater,
                    Resource.Layout.item_add_crawler));

            Bindings.Add(
                this.SetBinding(() => ViewModel.SetName,
                    () => TitleTextBox.Text, BindingMode.TwoWay));


            EmptyStateIcon.SetImageResource(Resource.Drawable.icon_playlist_plus_huge);
            EmptyStateSubtitle.Text = AppResources.EmptyState_Subtitle_CrawlerSetDetails;

            CrawlersRecyclerView.SetLayoutManager(new LinearLayoutManager(Activity));
            AddCrawlersRecyclerView.SetLayoutManager(new CustomFlexboxLayoutManager(Activity)
            {
                FlexDirection = FlexDirection.Row,
                FlexWrap = FlexWrap.Wrap
            });
        }

        private void SharedCrawlerDescriptorDataTemplate(CrawlerDescriptorViewModel item, ICrawlerHolder holder, int position)
        {
            BaseCrawlerTemplate(item, holder);
        }

        private void BaseCrawlerTemplate(CrawlerDescriptorViewModel item, ICrawlerHolder holder)
        {
            holder.ViewModelProxy = item;
            holder.ClickSurface.SetOnClickCommand(ViewModel.SelectCrawlerDescriptorCommand, item);
            holder.ClickSurface.SetOnLongClickListener(new OnLongClickListener(view =>
            {
                var menu = new PopupMenu(Activity, holder.ItemView);
                menu.Menu.Add(AppResources.Generic_Delete);
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
            holder.Image.SetImageResource(item.BackingModel.CrawlerDomain.ToImageResource());
            holder.ClickSurface.SetOnClickCommand(ViewModel.AddCrawlerCommand, item);
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
    }
}