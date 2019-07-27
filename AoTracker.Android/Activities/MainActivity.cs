using System;
using System.Diagnostics;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using AoLibs.Navigation.Android.Navigation;
using AoLibs.Navigation.Core.Interfaces;
using AoTracker.Domain.Enums;
using AoTracker.Infrastructure.Statics;
using AoTracker.Infrastructure.ViewModels;
using Autofac;
using Toolbar = global::Android.Support.V7.Widget.Toolbar;

namespace AoTracker.Android
{
    [Activity(
        Label = "@string/app_name",
        Theme = "@style/AppTheme",
        MainLauncher = true, LaunchMode = LaunchMode.SingleInstance,
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class MainActivity : AppCompatActivity
    {
        private ActionBarDrawerToggle _hamburgerToggle;

        public static Activity Instance { get; set; }

        public MainActivity()
        {
            Instance = this;
        }

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_main);

            App.NavigationManager = new NavigationManager<PageIndex>(
                SupportFragmentManager,
                RootView,
                new ViewModelResolver());
            SetSupportActionBar(Toolbar);

            _hamburgerToggle = new ActionBarDrawerToggle(this, DrawerLayout, Toolbar,
                Resource.String.DrawerOpen, Resource.String.DrawerClose);
            DrawerLayout.AddDrawerListener(_hamburgerToggle);
            _hamburgerToggle.SyncState();

            NavigationView.Menu.Add("Test");

            using (var scope = ResourceLocator.ObtainScope())
            {
                scope.Resolve<MainViewModel>().Initialize();
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (_hamburgerToggle.OnOptionsItemSelected(item))
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);

        }

        public override void OnBackPressed()
        {
            if (!App.NavigationManager.OnBackRequested())
            {
                MoveTaskToBack(true);
            }
        }
        #region Views

        private Toolbar _toolbar;
        private FrameLayout _rootView;
        private NavigationView _navigationView;
        private DrawerLayout _drawerLayout;

        public Toolbar Toolbar => _toolbar ?? (_toolbar = FindViewById<Toolbar>(Resource.Id.Toolbar));
        public FrameLayout RootView => _rootView ?? (_rootView = FindViewById<FrameLayout>(Resource.Id.RootView));
        public NavigationView NavigationView => _navigationView ?? (_navigationView = FindViewById<NavigationView>(Resource.Id.NavigationView));
        public DrawerLayout DrawerLayout => _drawerLayout ?? (_drawerLayout = FindViewById<DrawerLayout>(Resource.Id.DrawerLayout));

        #endregion

        private class ViewModelResolver : IViewModelResolver
        {
            public TViewModel Resolve<TViewModel>()
            {
                Log.Debug(nameof(App), $"Resolving ViewModel: {typeof(TViewModel).Name}");
                try
                {
                    using (var scope = ResourceLocator.ObtainScope())
                    {
                        return scope.Resolve<TViewModel>();
                    }
                }
                catch (Exception e)
                {
                    Debugger.Break();
                    throw;
                }
            }
        }


    }
}