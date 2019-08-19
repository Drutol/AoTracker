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

namespace AoTracker.Android.Fragments
{
    public partial class CrawlerSetsPageFragment
    {
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

            private ImageView _indexIcon;
            private TextView _title;
            private TextView _emptyNotice;
            private RecyclerView _crawlerSummaryRecyclerView;
            private ImageButton _moreButton;
            private LinearLayout _clickSurface;

            public ImageView IndexIcon => _indexIcon ?? (_indexIcon = _view.FindViewById<ImageView>(Resource.Id.IndexIcon));
            public TextView Title => _title ?? (_title = _view.FindViewById<TextView>(Resource.Id.Title));
            public TextView EmptyNotice => _emptyNotice ?? (_emptyNotice = _view.FindViewById<TextView>(Resource.Id.EmptyNotice));
            public RecyclerView CrawlerSummaryRecyclerView => _crawlerSummaryRecyclerView ?? (_crawlerSummaryRecyclerView = _view.FindViewById<RecyclerView>(Resource.Id.CrawlerSummaryRecyclerView));
            public ImageButton MoreButton => _moreButton ?? (_moreButton = _view.FindViewById<ImageButton>(Resource.Id.MoreButton));
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
                if (_movedPosition.HasValue)
                    _parent.ViewModel.MoveCrawlerSet(_movedPosition.Value, _lastTargetPosition);
                _movedPosition = null;
            }

            public override void OnSwiped(RecyclerView.ViewHolder viewHolder, int direction)
            {

            }

        }
    }
}