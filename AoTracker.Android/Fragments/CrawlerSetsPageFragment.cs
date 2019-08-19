using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.View.Menu;
using Android.Support.V7.Widget;
using Android.Support.V7.Widget.Helper;
using Android.Views;
using Android.Widget;
using AoLibs.Adapters.Android.Recycler;
using AoLibs.Navigation.Android.Navigation.Attributes;
using AoLibs.Utilities.Android;
using AoLibs.Utilities.Android.Listeners;
using AoTracker.Android.Utils;
using AoTracker.Domain.Enums;
using AoTracker.Domain.Models;
using AoTracker.Infrastructure.ViewModels;
using AoTracker.Resources;
using GalaSoft.MvvmLight.Helpers;
using PopupMenu = Android.Widget.PopupMenu;

namespace AoTracker.Android.Fragments
{
    [NavigationPage(PageIndex.CrawlerSets)]
    public partial class CrawlerSetsPageFragment : CustomFragmentBase<CrawlerSetsViewModel>
    {
        public override int LayoutResourceId { get; } = Resource.Layout.page_crawler_sets;

        protected override void InitBindings()
        {
            Bindings.Add(this.SetBinding(() => ViewModel.Sets).WhenSourceChanges(() =>
            {
                SetsRecyclerView.SetAdapter(new ObservableRecyclerAdapter<CrawlerSet, CrawlerSetHolder>(ViewModel.Sets,
                        DataTemplate, LayoutInflater,
                        Resource.Layout.item_crawler_set)
                    { StretchContentHorizonatally = true });
            }));

            SetsRecyclerView.SetLayoutManager(new LinearLayoutManager(Activity));
            var touchHelper = new ItemTouchHelper(new ItemTouchHelperCallback(this));
            touchHelper.AttachToRecyclerView(SetsRecyclerView);

            AddButton.SetOnClickCommand(ViewModel.AddNewSetCommand);
        }

        public override void NavigatedTo()
        {
            base.NavigatedTo();
            ViewModel.NavigatedTo();
        }

        public override void NavigatedBack()
        {
            base.NavigatedBack();
            ViewModel.NavigatedTo();
        }

        private void DataTemplate(CrawlerSet item, CrawlerSetHolder holder, int position)
        {
            holder.Title.Text = item.Name;
            holder.IndexIcon.SetImageResource(Util.IndexToIconResource(position + 1));
            if (item.Descriptors?.Any() ?? false)
            {
                holder.EmptyNotice.Visibility = ViewStates.Gone;
                holder.CrawlerSummaryRecyclerView.Visibility = ViewStates.Visible;
                holder.CrawlerSummaryRecyclerView.SetAdapter(
                    new ObservableRecyclerAdapter<CrawlerDescriptor, CrawlerSetSummaryEntryHolder>(
                        item.Descriptors,
                        CrawlerSummaryEntryDataTemplate,
                        LayoutInflater,
                        Resource.Layout.item_crawler_set_summary){StretchContentHorizonatally = true});
            }
            else
            {
                holder.EmptyNotice.Visibility = ViewStates.Visible;
                holder.CrawlerSummaryRecyclerView.Visibility = ViewStates.Gone;
            }

            holder.ClickSurface.SetOnClickCommand(ViewModel.NavigateSetCommand, item);

            holder.MoreButton.SetOnClickListener(new OnClickListener(view =>
            {
                var menuBuilder = new MenuBuilder(Activity);
                menuBuilder.Add(0, 0, 0, AppResources.Generic_Delete).SetIcon(Resource.Drawable.icon_delete);
                menuBuilder.SetCallback(new MenuCallback((sender, menuItem) =>
                {
                    if (menuItem.ItemId == 0)
                    {
                        ViewModel.RemoveSet(item);
                    }
                }));
                var menuPopupHelper = new MenuPopupHelper(Context, menuBuilder);
                menuPopupHelper.SetAnchorView(holder.MoreButton);
                menuPopupHelper.SetForceShowIcon(true);
                menuPopupHelper.Show();
            }));
        }

        private void CrawlerSummaryEntryDataTemplate(CrawlerDescriptor item, CrawlerSetSummaryEntryHolder holder, int position)
        {
            holder.Image.SetImageResource(item.CrawlerDomain.ToImageResource());
            holder.SearchPhrase.Text = item.CrawlerSourceParameters.SearchQuery;
        }
    }
}