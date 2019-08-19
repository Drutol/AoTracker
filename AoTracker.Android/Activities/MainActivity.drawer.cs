using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using AoTracker.Android.Themes;
using AoTracker.Android.Utils;
using AoTracker.Domain.Enums;
using AoTracker.Domain.Messaging;
using AoTracker.Infrastructure.Models;
using GalaSoft.MvvmLight.Helpers;
using Messenger = GalaSoft.MvvmLight.Messaging.Messenger;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace AoTracker.Android.Activities
{
    public partial class MainActivity
    {
        private void InitDrawer()
        {
            Messenger.Default.Register<PageTitleMessage>(this, OnNewPageTitle);
            Messenger.Default.Register<ToolbarRequestMessage>(this, OnNewToolbarRequest);

            NavigationRecyclerView.SetAdapter(
                new RecyclerViewAdapterBuilder<HamburgerMenuEntry, HamburgerEntryHolder>()
                .WithContentStretching()
                .WithItems(ViewModel.HamburgerItems)
                .WithResourceId(LayoutInflater, Resource.Layout.nav_item)
                .WithDataTemplate(HamburgerItemDataTemplate)
                .Build());

            var divider = new DividerItemDecoration(this, DividerItemDecoration.Vertical);
            divider.SetDrawable(Resources.GetDrawable(Resource.Drawable.separator_transparent, Theme));
            NavigationRecyclerView.AddItemDecoration(divider);
            NavigationRecyclerView.SetLayoutManager(new LinearLayoutManager(this));

            SetUpHamburgerButton();
            Toolbar.MenuItemClick += ToolbarOnMenuItemClick;
        }

        #region HamburgerItems

        private void HamburgerItemDataTemplate(HamburgerMenuEntry item, HamburgerEntryHolder holder, int position)
        {
            holder.Title.Text = item.Title;
            holder.Icon.SetImageResource(item.Page.ToIconResource());
        }

        private void SetUpHamburgerButton()
        {
            _hamburgerToggle = new ActionBarDrawerToggle(
                this,
                DrawerLayout,
                Toolbar,
                Resource.String.DrawerOpen,
                Resource.String.DrawerClose)
            {
                DrawerArrowDrawable =
                {
                    Color = ThemeManager.ToolbarTextColour
                }
            };

            DrawerLayout.AddDrawerListener(_hamburgerToggle);
            _hamburgerToggle.SyncState();
        }

        #endregion

        private void ToolbarOnMenuItemClick(object sender, Toolbar.MenuItemClickEventArgs e)
        {
            Messenger.Default.Send((ToolbarActionMessage)e.Item.ItemId);
        }

        #region MessengerSubscriptions

        private void OnNewToolbarRequest(ToolbarRequestMessage request)
        {
            switch (request)
            {
                case ToolbarRequestMessage.ShowSaveButton:
                    Toolbar.Menu.Add(0, (int)ToolbarActionMessage.ClickedSaveButton, 0, "Save")
                        .SetIcon(Resource.Drawable.icon_tick)
                        .SetShowAsAction(ShowAsAction.Always);
                    break;
                case ToolbarRequestMessage.ResetToolbar:
                    Toolbar.Menu.Clear();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(request), request, null);
            }

        }

        private void OnNewPageTitle(PageTitleMessage message)
        {
            SupportActionBar.Title = message.NewTitle;
        }

        #endregion


        class HamburgerEntryHolder : RecyclerView.ViewHolder
        {
            private readonly View _view;

            public HamburgerEntryHolder(View view) : base(view)
            {
                _view = view;
            }
            private ImageView _icon;
            private TextView _title;
            private FrameLayout _clickSurface;

            public ImageView Icon => _icon ?? (_icon = _view.FindViewById<ImageView>(Resource.Id.Icon));
            public TextView Title => _title ?? (_title = _view.FindViewById<TextView>(Resource.Id.Title));
            public FrameLayout ClickSurface => _clickSurface ?? (_clickSurface = _view.FindViewById<FrameLayout>(Resource.Id.ClickSurface));
        }


    }
}