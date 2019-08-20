using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using AoLibs.Adapters.Android.Recycler;
using AoLibs.Utilities.Android;
using AoTracker.Android.Themes;
using AoTracker.Android.Utils;
using AoTracker.Domain.Enums;
using AoTracker.Domain.Messaging;
using AoTracker.Infrastructure.Models;
using AoTracker.Resources;
using GalaSoft.MvvmLight.Helpers;
using Messenger = GalaSoft.MvvmLight.Messaging.Messenger;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace AoTracker.Android.Activities
{
    public partial class MainActivity
    {
        private HamburgerEntryHolder _settingsButtonHolder;

        private void InitDrawer()
        {
            Messenger.Default.Register<PageTitleMessage>(this, OnNewPageTitle);
            Messenger.Default.Register<ToolbarRequestMessage>(this, OnNewToolbarRequest);

            NavigationRecyclerView.SetAdapter(
                new RecyclerViewAdapterBuilder<HamburgerMenuEntryViewModel, HamburgerEntryHolder>()
                .WithContentStretching()
                .WithItems(ViewModel.HamburgerItems)
                .WithResourceId(LayoutInflater, Resource.Layout.nav_item)
                .WithDataTemplate(HamburgerItemDataTemplate)
                .WithHolderTemplate((parent, type, view) => new HamburgerEntryHolder(view, this))
                .Build());

            var divider = new DividerItemDecoration(this, DividerItemDecoration.Vertical);
            divider.SetDrawable(Resources.GetDrawable(Resource.Drawable.separator_transparent, Theme));
            NavigationRecyclerView.AddItemDecoration(divider);
            NavigationRecyclerView.SetLayoutManager(new LinearLayoutManager(this));

            SetUpHamburgerButton();
            Toolbar.MenuItemClick += ToolbarOnMenuItemClick;

            _settingsButtonHolder = new HamburgerEntryHolder(SettingsNavButton, this);
            _settingsButtonHolder.ViewModel = ViewModel.SettingsButtonViewModel;
        }

        #region HamburgerItems

        private void HamburgerItemDataTemplate(HamburgerMenuEntryViewModel item, HamburgerEntryHolder holder, int position)
        {

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


        class HamburgerEntryHolder : BindingViewHolderBase<HamburgerMenuEntryViewModel>
        {
            private readonly View _view;
            private readonly MainActivity _parent;

            public HamburgerEntryHolder(View view, MainActivity parent) : base(view)
            {
                _view = view;
                _parent = parent;
            }

            protected override void SetBindings()
            {
                Title.Text = ViewModel.Title;
                Icon.SetImageResource(ViewModel.Page.ToIconResource());

                Bindings.Add(this.SetBinding(() => ViewModel.IsSelected).WhenSourceChanges(() =>
                {
                    if (ViewModel.IsSelected)
                    {
                        Container.SetBackgroundResource(Resource.Drawable.border_accent_left_wide);
                        Icon.ImageTintList = ColorStateList.ValueOf(ThemeManager.AccentColour);
                        Title.SetTextColor(ThemeManager.AccentColour);
                    }
                    else
                    {
                        Container.SetBackgroundResource(Resource.Drawable.border_accent_left);
                        Icon.ImageTintList = ColorStateList.ValueOf(ThemeManager.TextColour);
                        Title.SetTextColor(ThemeManager.TextColour);
                    }
                }));

                ClickSurface.SetOnClickCommand(_parent.ViewModel.SelectHamburgerItemCommand, ViewModel);
            }

            private ImageView _icon;
            private TextView _title;
            private LinearLayout _container;
            private FrameLayout _clickSurface;

            public ImageView Icon => _icon ?? (_icon = _view.FindViewById<ImageView>(Resource.Id.Icon));
            public TextView Title => _title ?? (_title = _view.FindViewById<TextView>(Resource.Id.Title));
            public LinearLayout Container => _container ?? (_container = _view.FindViewById<LinearLayout>(Resource.Id.Container));
            public FrameLayout ClickSurface => _clickSurface ?? (_clickSurface = _view.FindViewById<FrameLayout>(Resource.Id.ClickSurface));
        }

    }
}