using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Support.V7.Widget.Helper;
using Android.Views;
using Android.Widget;
using AoLibs.Navigation.Android.Navigation.Attributes;
using AoTracker.Android.Utils;
using AoTracker.Domain.Enums;
using AoTracker.Domain.Models;
using AoTracker.Infrastructure.ViewModels;
using AoTracker.Resources;
using FFImageLoading;

namespace AoTracker.Android.Fragments
{
    [NavigationPage(PageIndex.IgnoredItems)]
    public class IgnoredItemsPageFragment : CustomFragmentBase<IgnoredItemsViewModel>
    {
        public override int LayoutResourceId { get; } = Resource.Layout.page_ignored_items;

        protected override void InitBindings()
        {
            RecyclerView.SetAdapter(new RecyclerViewAdapterBuilder<IgnoredItemEntry, IgnoredEntryHolder>()
                .WithContentStretching()
                .WithItems(ViewModel.IgnoredItems)
                .WithResourceId(LayoutInflater, Resource.Layout.item_ignored)
                .WithDataTemplate(IgnoredItemDataTemplate)
                .Build());
            RecyclerView.SetLayoutManager(new LinearLayoutManager(Activity));
            ViewModel.IgnoredItems.SetUpWithEmptyState(EmptyState);
            var touchHelper = new ItemTouchHelper(new ItemTouchHelperCallback(this));
            touchHelper.AttachToRecyclerView(RecyclerView);
        }

        private void IgnoredItemDataTemplate(IgnoredItemEntry item, IgnoredEntryHolder holder, int position)
        {
            holder.Title.Text = item.Name;
            holder.IgnoredAtLabel.Text =
                string.Format(AppResources.Item_IgnoredItem_IgnoredAt, item.IgnoredAt.ToString("d"));
            ImageService.Instance.LoadUrl(item.ImageUrl).Into(holder.Image);
        }

        public override void NavigatedTo()
        {
            base.NavigatedTo();
            ViewModel.NavigatedTo();
        }

        #region Views

        private RecyclerView _recyclerView;
        private ImageView _emptyStateIcon;
        private LinearLayout _emptyState;

        public RecyclerView RecyclerView => _recyclerView ?? (_recyclerView = FindViewById<RecyclerView>(Resource.Id.RecyclerView));
        public ImageView EmptyStateIcon => _emptyStateIcon ?? (_emptyStateIcon = FindViewById<ImageView>(Resource.Id.EmptyStateIcon));
        public LinearLayout EmptyState => _emptyState ?? (_emptyState = FindViewById<LinearLayout>(Resource.Id.EmptyState));

        #endregion

        class IgnoredEntryHolder : RecyclerView.ViewHolder
        {
            private readonly View _view;

            public IgnoredEntryHolder(View view) : base(view)
            {
                _view = view;
            }
            private ImageView _image;
            private TextView _title;
            private TextView _ignoredAtLabel;
            private LinearLayout _clickSurface;

            public ImageView Image => _image ?? (_image = _view.FindViewById<ImageView>(Resource.Id.Image));
            public TextView Title => _title ?? (_title = _view.FindViewById<TextView>(Resource.Id.Title));
            public TextView IgnoredAtLabel => _ignoredAtLabel ?? (_ignoredAtLabel = _view.FindViewById<TextView>(Resource.Id.IgnoredAtLabel));
            public LinearLayout ClickSurface => _clickSurface ?? (_clickSurface = _view.FindViewById<LinearLayout>(Resource.Id.ClickSurface));
        }

        class ItemTouchHelperCallback : ItemTouchHelper.Callback
        {
            private readonly IgnoredItemsPageFragment _parent;

            public ItemTouchHelperCallback(IgnoredItemsPageFragment parent)
            {
                _parent = parent;
            }

            public override bool IsItemViewSwipeEnabled { get; } = true;

            public override bool IsLongPressDragEnabled { get; } = false;

            public override bool CanDropOver(RecyclerView recyclerView, RecyclerView.ViewHolder current, RecyclerView.ViewHolder target)
            {
                return true;
            }

            public override int GetMovementFlags(RecyclerView p0, RecyclerView.ViewHolder p1)
            {
                int dragFlags = ItemTouchHelper.End;
                return MakeMovementFlags(0, dragFlags);
            }

            public override bool OnMove(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, RecyclerView.ViewHolder target)
            {
                return false;
            }

            public override void OnSwiped(RecyclerView.ViewHolder viewHolder, int direction)
            {
                _parent.ViewModel.RemoveIgnoredItem.Execute(viewHolder.AdapterPosition);
            }
        }
    }
}