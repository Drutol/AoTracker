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
using Android.Views;
using Android.Widget;
using AoTracker.Android.Themes;
using AoTracker.Domain.Enums;
using AoTracker.Domain.Messaging;
using GalaSoft.MvvmLight.Helpers;
using Messenger = GalaSoft.MvvmLight.Messaging.Messenger;

namespace AoTracker.Android.Activities
{
    public partial class MainActivity
    {
        private void InitDrawer()
        {
            Messenger.Default.Register<PageTitleMessage>(this, OnNewPageTitle);

            SetUpHamburgerButton();
            NavigationView.NavigationItemSelected += NavigationViewOnNavigationItemSelected;

            Bindings.Add(this.SetBinding(() => ViewModel.HamburgerItems).WhenSourceChanges(() =>
            {
                ViewModel.HamburgerItems.CollectionChanged += HamburgerItemsOnCollectionChanged;
                UpdateHamburgerItems();
            }));
        }

        private void NavigationViewOnNavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {
            ViewModel.SelectedItem =
                ViewModel.HamburgerItems.First(entry => entry.Page == (PageIndex) e.MenuItem.ItemId);
        }

        private void SetUpHamburgerButton()
        {
            _hamburgerToggle = new ActionBarDrawerToggle(this, DrawerLayout, Toolbar,
                Resource.String.DrawerOpen, Resource.String.DrawerClose)
            {
                DrawerArrowDrawable = {Color = ThemeManager.TextInvertedColour}
            };
            DrawerLayout.AddDrawerListener(_hamburgerToggle);
            _hamburgerToggle.SyncState();
        }

        private void HamburgerItemsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateHamburgerItems();
        }

        private void UpdateHamburgerItems()
        {
            NavigationView.Menu.Clear();

            foreach (var viewModelHamburgerItem in ViewModel.HamburgerItems)
            {
                NavigationView.Menu.Add(0, (int)viewModelHamburgerItem.Page, 0, viewModelHamburgerItem.Title);
            }
        }

        private void OnNewPageTitle(PageTitleMessage message)
        {
            SupportActionBar.Title = message.NewTitle;
        }
    }
}