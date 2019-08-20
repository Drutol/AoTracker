using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using AoLibs.Navigation.Android.Navigation.Attributes;
using AoLibs.Utilities.Android;
using AoTracker.Android.Utils;
using AoTracker.Domain.Enums;
using AoTracker.Infrastructure.Models;
using AoTracker.Infrastructure.ViewModels;
using AoTracker.Infrastructure.ViewModels.Settings;

namespace AoTracker.Android.Fragments
{
    [NavigationPage(PageIndex.SettingsIndex)]
    public class SettingsIndexPageFragment : CustomFragmentBase<SettingsIndexViewModel>
    {
        public override int LayoutResourceId { get; } = Resource.Layout.page_settings_index;

        protected override void InitBindings()
        {
            RecyclerView.SetAdapter(new RecyclerViewAdapterBuilder<SettingsIndexEntry, SettingsIndexEntryHolder>()
                .WithItems(ViewModel.Entries)
                .WithDataTemplate(EntryDataTemplate)
                .WithResourceId(LayoutInflater, Resource.Layout.item_settings_entry)
                .WithContentStretching()
                .Build());
            RecyclerView.SetLayoutManager(new LinearLayoutManager(Activity));
        }

        private void EntryDataTemplate(SettingsIndexEntry item, SettingsIndexEntryHolder holder, int position)
        {
            holder.Icon.SetImageResource(item.Page.ToIconResource());
            holder.Title.Text = item.Title;
            holder.Subtitle.Text = item.Subtitle;
            holder.ClickSurface.SetOnClickCommand(ViewModel.SelectEntryCommand, item);
        }

        #region Views

        private RecyclerView _recyclerView;

        public RecyclerView RecyclerView => _recyclerView ?? (_recyclerView = FindViewById<RecyclerView>(Resource.Id.RecyclerView));

        #endregion

        class SettingsIndexEntryHolder : RecyclerView.ViewHolder
        {
            private readonly View _view;

            public SettingsIndexEntryHolder(View view) : base(view)
            {
                _view = view;
            }
            private ImageView _icon;
            private TextView _title;
            private TextView _subtitle;
            private FrameLayout _clickSurface;

            public ImageView Icon => _icon ?? (_icon = _view.FindViewById<ImageView>(Resource.Id.Icon));
            public TextView Title => _title ?? (_title = _view.FindViewById<TextView>(Resource.Id.Title));
            public TextView Subtitle => _subtitle ?? (_subtitle = _view.FindViewById<TextView>(Resource.Id.Subtitle));
            public FrameLayout ClickSurface => _clickSurface ?? (_clickSurface = _view.FindViewById<FrameLayout>(Resource.Id.ClickSurface));
        }


    }
}