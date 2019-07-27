using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using AoTracker.Domain.Messaging;
using Messenger = GalaSoft.MvvmLight.Messaging.Messenger;

namespace AoTracker.Android.Activities
{
    public partial class MainActivity
    {
        private void InitDrawer()
        {
            Messenger.Default.Register<PageTitleMessage>(this, OnNewPageTitle);

            _hamburgerToggle = new ActionBarDrawerToggle(this, DrawerLayout, Toolbar,
                Resource.String.DrawerOpen, Resource.String.DrawerClose);
            DrawerLayout.AddDrawerListener(_hamburgerToggle);
            _hamburgerToggle.SyncState();

            
        }

        private void OnNewPageTitle(PageTitleMessage message)
        {
            SupportActionBar.Title = message.NewTitle;
        }
    }
}