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
using Android.Views;
using Android.Widget;
using AoLibs.Adapters.Android.Recycler;
using AoLibs.Navigation.Android.Navigation.Attributes;
using AoLibs.Utilities.Android;
using AoLibs.Utilities.Android.Listeners;
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
                SetsRecyclerView.SetAdapter(new ObservableRecyclerAdapter<CrawlerSet, CrawlerEntryHolder>(ViewModel.Sets,
                        DataTemplate, LayoutInflater,
                        Resource.Layout.item_crawler_set)
                    { StretchContentHorizonatally = true });
            }));

            SetsRecyclerView.SetLayoutManager(new LinearLayoutManager(Activity));

            AddButton.SetOnClickCommand(ViewModel.AddNewSetCommand);
        }

        public override void NavigatedTo()
        {
            base.NavigatedTo();
            ViewModel.NavigatedTo();
        }

        public override void NavigatedBack()
        {
            ViewModel.NavigatedTo();
        }

        private void DataTemplate(CrawlerSet item, CrawlerEntryHolder holder, int position)
        {
            holder.Title.Text = item.Name;
            holder.ClickSurface.SetOnClickCommand(ViewModel.NavigateSetCommand, item);
        }

        #region Views

        private RecyclerView _setsRecyclerView;
        private FloatingActionButton _addButton;

        public RecyclerView SetsRecyclerView => _setsRecyclerView ?? (_setsRecyclerView = FindViewById<RecyclerView>(Resource.Id.SetsRecyclerView));
        public FloatingActionButton AddButton => _addButton ?? (_addButton = FindViewById<FloatingActionButton>(Resource.Id.AddButton));

        #endregion

        class CrawlerEntryHolder : RecyclerView.ViewHolder
        {
            private readonly View _view;

            public CrawlerEntryHolder(View view) : base(view)
            {
                _view = view;
            }

            private ImageView _imageLeft;
            private TextView _title;
            private LinearLayout _clickSurface;

            public ImageView ImageLeft => _imageLeft ?? (_imageLeft = _view.FindViewById<ImageView>(Resource.Id.ImageLeft));
            public TextView Title => _title ?? (_title = _view.FindViewById<TextView>(Resource.Id.Title));
            public LinearLayout ClickSurface => _clickSurface ?? (_clickSurface = _view.FindViewById<LinearLayout>(Resource.Id.ClickSurface));
        }


    }
}