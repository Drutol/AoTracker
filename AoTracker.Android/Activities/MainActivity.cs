using System;
using System.Collections.Generic;
using System.Diagnostics;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using AoLibs.Navigation.Android.Navigation;
using AoLibs.Navigation.Core.Interfaces;
using AoTracker.Android.Themes;
using AoTracker.Domain.Enums;
using AoTracker.Domain.Messaging;
using AoTracker.Infrastructure.Statics;
using AoTracker.Infrastructure.ViewModels;
using Autofac;
using GalaSoft.MvvmLight.Helpers;
using Messenger = GalaSoft.MvvmLight.Messaging.Messenger;
using Toolbar = global::Android.Support.V7.Widget.Toolbar;

namespace AoTracker.Android.Activities
{
    [Activity(
        Label = "@string/app_name",
        Theme = "@style/AoTracker.Dark",
        Icon = "@mipmap/ic_launcher",
        RoundIcon = "@mipmap/ic_launcher_round",
        MainLauncher = true, LaunchMode = LaunchMode.SingleInstance,
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public partial class MainActivity : AppCompatActivity
    {
        private ActionBarDrawerToggle _hamburgerToggle;
        private MainViewModel ViewModel { get; set; }

        public static Activity Instance { get; set; }

        public List<Binding> Bindings { get; } = new List<Binding>();

        public MainActivity()
        {
            Instance = this;
        }

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.ApplyTheme();
            if(ThemeManager.IsDarkTheme)
                Window.SetStatusBarColor(ThemeManager.DarkBackgroundColour);
            SetContentView(Resource.Layout.activity_main);
            AppInitializationRoutines.InitializeDependencies();
            App.NavigationManager = new NavigationManager<PageIndex>(
                SupportFragmentManager,
                RootView,
                new ViewModelResolver());
            SetSupportActionBar(Toolbar);

            using (var scope = ResourceLocator.ObtainScope())
            {
                ViewModel = scope.Resolve<MainViewModel>();
            }

            ViewModel.Initialize();

            InitDrawer();
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
        private RecyclerView _navigationRecyclerView;
        private FrameLayout _settingsNavButton;
        private NavigationView _navigationView;
        private DrawerLayout _drawerLayout;

        public Toolbar Toolbar => _toolbar ?? (_toolbar = FindViewById<Toolbar>(Resource.Id.Toolbar));
        public FrameLayout RootView => _rootView ?? (_rootView = FindViewById<FrameLayout>(Resource.Id.RootView));

        public RecyclerView NavigationRecyclerView => _navigationRecyclerView ??
                                                      (_navigationRecyclerView =
                                                          FindViewById<RecyclerView>(Resource.Id.NavigationRecyclerView)
                                                      );

        public FrameLayout SettingsNavButton => _settingsNavButton ??
                                                (_settingsNavButton =
                                                    FindViewById<FrameLayout>(Resource.Id.SettingsNavButton));

        public NavigationView NavigationView => _navigationView ??
                                                (_navigationView =
                                                    FindViewById<NavigationView>(Resource.Id.NavigationView));

        public DrawerLayout DrawerLayout =>
            _drawerLayout ?? (_drawerLayout = FindViewById<DrawerLayout>(Resource.Id.DrawerLayout));

        #endregion

        private class ViewModelResolver : IViewModelResolver
        {
            public TViewModel Resolve<TViewModel>()
            {
                Log.Debug(nameof(App), $"Resolving ViewModel: {typeof(TViewModel).Name}");
                try
                {
                    return ResourceLocator.CurrentScope.Resolve<TViewModel>();
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