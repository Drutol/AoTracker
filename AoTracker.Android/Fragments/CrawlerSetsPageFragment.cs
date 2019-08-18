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
using AoLibs.Navigation.Android.Navigation.Attributes;
using AoLibs.Utilities.Android;
using AoLibs.Utilities.Android.Listeners;
using AoTracker.Android.Utils;
using AoTracker.Domain.Enums;
using AoTracker.Domain.Models;
using AoTracker.Infrastructure.ViewModels;
using GalaSoft.MvvmLight.Helpers;

namespace AoTracker.Android.Fragments
{
    [NavigationPage(PageIndex.CrawlerSets)]
    public class CrawlerSetsPageFragment : CustomFragmentBase<CrawlerSetsViewModel>
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
            if (item.Descriptors?.Any() ?? false)
            {
                holder.EmptyNotice.Visibility = ViewStates.Gone;
                holder.CrawlerSummaryRecyclerView.Visibility = ViewStates.Visible;
                holder.CrawlerSummaryRecyclerView.SetAdapter(
                    new ObservableRecyclerAdapter<CrawlerDescriptor, CrawlerSetSummaryEntryHolder>(
                        item.Descriptors,
                        CrawlerSummaryEntryDataTemplate,
                        LayoutInflater,
                        Resource.Layout.item_crawler_set_summary));
            }
            else
            {
                holder.EmptyNotice.Visibility = ViewStates.Visible;
                holder.CrawlerSummaryRecyclerView.Visibility = ViewStates.Gone;
            }

            holder.ClickSurface.SetOnClickCommand(ViewModel.NavigateSetCommand, item);
        }

        private void CrawlerSummaryEntryDataTemplate(CrawlerDescriptor item, CrawlerSetSummaryEntryHolder holder, int position)
        {
            holder.Image.SetImageResource(item.CrawlerDomain.ToImageResource());
            holder.SearchPhrase.Text = item.CrawlerSourceParameters.SearchQuery;
        }

        #region Views

        private RecyclerView _setsRecyclerView;
        private FloatingActionButton _addButton;

        public RecyclerView SetsRecyclerView => _setsRecyclerView ?? (_setsRecyclerView = FindViewById<RecyclerView>(Resource.Id.SetsRecyclerView));
        public FloatingActionButton AddButton => _addButton ?? (_addButton = FindViewById<FloatingActionButton>(Resource.Id.AddButton));

        #endregion

        class CrawlerSetHolder : RecyclerView.ViewHolder
        {
            private readonly View _view;

            public CrawlerSetHolder(View view) : base(view)
            {
                _view = view;


                CrawlerSummaryRecyclerView.AddItemDecoration(new DividerItemDecoration(view.Context,
                    DividerItemDecoration.Vertical));
                CrawlerSummaryRecyclerView.SetLayoutManager(new LinearLayoutManager(view.Context));
            }
            private TextView _title;
            private TextView _emptyNotice;
            private RecyclerView _crawlerSummaryRecyclerView;
            private LinearLayout _clickSurface;

            public TextView Title => _title ?? (_title = _view.FindViewById<TextView>(Resource.Id.Title));
            public TextView EmptyNotice => _emptyNotice ?? (_emptyNotice = _view.FindViewById<TextView>(Resource.Id.EmptyNotice));
            public RecyclerView CrawlerSummaryRecyclerView => _crawlerSummaryRecyclerView ?? (_crawlerSummaryRecyclerView = _view.FindViewById<RecyclerView>(Resource.Id.CrawlerSummaryRecyclerView));
            public LinearLayout ClickSurface => _clickSurface ?? (_clickSurface = _view.FindViewById<LinearLayout>(Resource.Id.ClickSurface));
        }


        class CrawlerSetSummaryEntryHolder : RecyclerView.ViewHolder
        {
            private readonly View _view;

            public CrawlerSetSummaryEntryHolder(View view) : base(view)
            {
                _view = view;
            }
            private ImageView _image;
            private TextView _searchPhrase;

            public ImageView Image => _image ?? (_image = _view.FindViewById<ImageView>(Resource.Id.Image));
            public TextView SearchPhrase => _searchPhrase ?? (_searchPhrase = _view.FindViewById<TextView>(Resource.Id.SearchPhrase));
        }

        class ItemTouchHelperCallback : ItemTouchHelper.Callback
        {
            private readonly CrawlerSetsPageFragment _parent;

            private int? _movedPosition;
            private int _lastTargetPosition;

            public ItemTouchHelperCallback(CrawlerSetsPageFragment parent)
            {
                _parent = parent;
            }

            public override bool IsItemViewSwipeEnabled { get; } = false;

            public override bool IsLongPressDragEnabled { get; } = true;

            public override bool CanDropOver(RecyclerView recyclerView, RecyclerView.ViewHolder current, RecyclerView.ViewHolder target)
            {
                return true;
            }

            public override int GetMovementFlags(RecyclerView p0, RecyclerView.ViewHolder p1)
            {
                int dragFlags = ItemTouchHelper.Up | ItemTouchHelper.Down | ItemTouchHelper.Start | ItemTouchHelper.End;
                return MakeMovementFlags(dragFlags, 0);
            }

            public override bool OnMove(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, RecyclerView.ViewHolder target)
            {
                if (_movedPosition == null)
                    _movedPosition = viewHolder.AdapterPosition;

                _lastTargetPosition = target.AdapterPosition;
                _parent.SetsRecyclerView.GetAdapter().NotifyItemMoved(viewHolder.AdapterPosition, target.AdapterPosition);
                return true;
            }


            public override void ClearView(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder)
            {
                if(_movedPosition.HasValue)
                    _parent.ViewModel.MoveCrawlerSet(_movedPosition.Value, _lastTargetPosition);
                _movedPosition = null;
            }

            public override void OnSwiped(RecyclerView.ViewHolder viewHolder, int direction)
            {

            }

        }
    }
}